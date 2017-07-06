using System.Windows;
using System.Windows.Input;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Kinect;
using LightBuzz.Vitruvius;
using System;

namespace Mousenect
{
    public partial class MainWindow : Window
    {
        #region Deklaration für TrackingView
        KinectSensor _sensor;
        MultiSourceFrameReader _reader;
        PlayersController _playersController;
        #endregion

        //Objekt der Klasse KinectControl erzeugen
        KinectControl kinectCtrl = new KinectControl();

        public MainWindow()
        {
            InitializeComponent();

            #region Konstruktor-Teil für TrackingView
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
            #endregion
        }
        
        private void MouseSensitivity_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (MouseSensitivity.IsLoaded)
            {
                kinectCtrl.mouseSensitivity = (float)MouseSensitivity.Value;
                txtMouseSensitivity.Text = kinectCtrl.mouseSensitivity.ToString("f2");
                
                Properties.Settings.Default.MouseSensitivity = kinectCtrl.mouseSensitivity;
                Properties.Settings.Default.Save();
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
        }

        private void chkNoClick_Checked(object sender, RoutedEventArgs e)
        {
            chkNoClickChange();
        }


        public void chkNoClickChange()
        {
            kinectCtrl.doClick = !chkNoClick.IsChecked.Value;
            Properties.Settings.Default.DoClick = kinectCtrl.doClick;
            Properties.Settings.Default.Save();
        }

        private void chkNoClick_Unchecked(object sender, RoutedEventArgs e)
        {
            chkNoClickChange();
        }

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
            }
        }

        #region Methoden für TrackingView
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
            // A new user has entered the scene.
        }

        void UserReporter_BodyLeft(object sender, PlayersControllerEventArgs e)
        {
            // A user has left the scene.
            viewer.Clear();
        }
        #endregion
    }
}