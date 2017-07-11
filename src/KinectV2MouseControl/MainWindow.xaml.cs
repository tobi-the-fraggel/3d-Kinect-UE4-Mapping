using System.Windows;
using Microsoft.Kinect;
using LightBuzz.Vitruvius;
using System;
using System.Windows.Controls;
using System.Windows.Media;

namespace Mousenect
{
    public partial class MainWindow : Window
    {
        KinectSensor _sensor;
        MultiSourceFrameReader _reader;
        PlayersController _playersController;
        SettingsWindow settings;

        //Objekt der Klasse KinectControl erzeugen
        KinectControl kinectCtrl = new KinectControl();

        public MainWindow()
        {
            InitializeComponent();

            _sensor = kinectCtrl.sensor;
            settings = new SettingsWindow(kinectCtrl);

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

        
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            System.Environment.Exit(0);
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
                        HL_State.Text = closest.HandLeftState.ToString();
                        HR_State.Text = closest.HandRightState.ToString();
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

        //Programmauswahl über Menü
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            MenuItem mi = e.Source as MenuItem;

            MI_Maus.IsChecked = false;
            MI_PP.IsChecked = false;
            MI_Zeichnung.IsChecked = false;

            kinectCtrl.setSteeringActive(false);

                switch(mi.Header)
            {
                case "Maus":
                    MI_Maus.IsChecked = true;
                    kinectCtrl.setProgramm(1);
                    break;
                case "PowerPoint":
                    MI_PP.IsChecked = true;
                    kinectCtrl.setProgramm(2);
                    break;
                case "Zeichnen":
                    MI_Zeichnung.IsChecked = true;
                    kinectCtrl.setProgramm(3);
                    break;
                default:
                    MI_Maus.IsChecked = true;
                    kinectCtrl.setProgramm(1);
                    Console.WriteLine("Fehler im MenuItem_Click");
                    break;
            }
        }

        //Einstellungen öffnen
        private void OpenSettings(object sender, RoutedEventArgs e)
        {
            settings.Show();
        }

        private void App_Close(object sender, RoutedEventArgs e)
        {
            System.Environment.Exit(0);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            kinectCtrl.setSteeringActive(false);
            kinectCtrl.setProgramm(1);
        }

        private void btn_activate_Click(object sender, RoutedEventArgs e)
        {
            Button btn = e.Source as Button;
            if(btn.Content.ToString() == "Steuerung aktivieren")
            {
                kinectCtrl.setSteeringActive(true);
                btn_activate.Background = Brushes.Red;
                btn_activate.Content = "Steuerung deaktivieren";
            }
            else
            {
                kinectCtrl.setSteeringActive(false);
                btn_activate.Background = Brushes.ForestGreen;
                btn_activate.Content = "Steuerung aktivieren";
            }
        }
    }
}