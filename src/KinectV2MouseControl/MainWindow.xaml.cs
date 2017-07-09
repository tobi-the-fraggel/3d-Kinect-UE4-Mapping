using System.Windows;
using Microsoft.Kinect;
using LightBuzz.Vitruvius;
using System;

namespace Mousenect
{
    public partial class MainWindow : Window
    {
        KinectSensor _sensor;
        MultiSourceFrameReader _reader;
        PlayersController _playersController;

        //Objekt der Klasse KinectControl erzeugen
        KinectControl kinectCtrl = new KinectControl();

        public MainWindow()
        {
            InitializeComponent();

            _sensor = KinectSensor.GetDefault();

            if (_sensor != null)
            {
                _sensor.Open();

                _reader = _sensor.OpenMultiSourceFrameReader(FrameSourceTypes.Color | FrameSourceTypes.Depth | FrameSourceTypes.Infrared | FrameSourceTypes.Body);
                _reader.MultiSourceFrameArrived += Reader_MultiSourceFrameArrived;

                _playersController = new PlayersController();
                _playersController.BodyEntered += UserReporter_BodyEntered;
                _playersController.BodyLeft += UserReporter_BodyLeft;
                _playersController.Start();
            }
        }
        
        private void MouseSensitivity_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (MouseSensitivity.IsLoaded)
            {
                kinectCtrl.mouseSensitivity = (float)MouseSensitivity.Value;
                txtMouseSensitivity.Text = kinectCtrl.mouseSensitivity.ToString("f2");
                
                Properties.Settings.Default.MouseSensitivity = kinectCtrl.mouseSensitivity;
                Properties.Settings.Default.Save();

                Console.WriteLine("Mouse Sensitivity wurde verändert");
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            MouseSensitivity.Value = Properties.Settings.Default.MouseSensitivity;
            chkNoClick.IsChecked = !Properties.Settings.Default.DoClick;
            CursorSmoothing.Value = Properties.Settings.Default.CursorSmoothing;
        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            if (_playersController != null)
            {
                _playersController.Stop();
            }

            if (_reader != null)
            {
                _reader.Dispose();
            }

            if (_sensor != null)
            {
                _sensor.Close();
            }
        }

        private void btnDefault_Click(object sender, RoutedEventArgs e)
        {
            MouseSensitivity.Value = KinectControl.MOUSE_SENSITIVITY;
            CursorSmoothing.Value = KinectControl.CURSOR_SMOOTHING;
            chkNoClick.IsChecked = !KinectControl.DO_CLICK;
            Console.WriteLine("Werte auf Standard zurückgesetzt");
        }

        #region NoClick-Checker
        private void chkNoClick_Checked(object sender, RoutedEventArgs e)
        {
            chkNoClickChange();
        }

        public void chkNoClickChange()
        {
            kinectCtrl.doClick = !chkNoClick.IsChecked.Value;
            Properties.Settings.Default.DoClick = kinectCtrl.doClick;
            Properties.Settings.Default.Save();
            Console.WriteLine("NoClick wurde verändert");
        }

        private void chkNoClick_Unchecked(object sender, RoutedEventArgs e)
        {
            chkNoClickChange();
        }
        #endregion

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            System.Environment.Exit(0);
        }

        private void CursorSmoothing_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (CursorSmoothing.IsLoaded)
            {
                kinectCtrl.cursorSmoothing = (float)CursorSmoothing.Value;
                txtCursorSmoothing.Text = kinectCtrl.cursorSmoothing.ToString("f2");

                Properties.Settings.Default.CursorSmoothing = kinectCtrl.cursorSmoothing;
                Properties.Settings.Default.Save();

                Console.WriteLine("CursorSmoothing wurde verändert");
            }
        }

        #region Methoden für Kinect-Anzeige
        void Reader_MultiSourceFrameArrived(object sender, MultiSourceFrameArrivedEventArgs e)
        {
            var reference = e.FrameReference.AcquireFrame();

            // Color
            using (var frame = reference.ColorFrameReference.AcquireFrame())
            {
                if (frame != null)
                {
                    if (viewer.Visualization == Visualization.Color)
                    {
                        viewer.Image = frame.ToBitmap();
                    }
                }
            }

            // Body
            using (var frame = reference.BodyFrameReference.AcquireFrame())
            {
                if (frame != null)
                {
                    var closest = frame.Bodies().Closest();
                    _playersController.Update(frame.Bodies());

                    if (closest != null)
                    {
                        viewer.DrawBody(closest);
                        HL_State.Content = closest.HandLeftState.ToString();
                        HR_State.Content = closest.HandRightState.ToString();
                    }
                    else
                        viewer.Clear();
                }
            }
        }

        void UserReporter_BodyEntered(object sender, PlayersControllerEventArgs e)
        {
            Console.WriteLine("Neuer Body in der Szene");
            // A new user has entered the scene.
        }

        void UserReporter_BodyLeft(object sender, PlayersControllerEventArgs e)
        {
            // A user has left the scene.
            Console.WriteLine("Body aus der Szene entfernt");
            viewer.Clear();
        }
        #endregion

        private void ComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            switch (Programm_Auswahl.SelectedIndex)
            {
                case 0:
                    kinectCtrl.setProgramm(1);
                    Console.WriteLine("Maus wurde ausgewählt");
                    break;
                case 1:
                    kinectCtrl.setProgramm(2);
                    Console.WriteLine("Powerpoint wurde ausgewählt");
                    break;
                default:
                    kinectCtrl.setProgramm(1);
                    Console.WriteLine("FEHLER");
                    break;
            }
        }
    }
}