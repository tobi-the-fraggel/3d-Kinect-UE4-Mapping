using System.Windows;
using System.Windows.Input;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace KinectV2MouseControl
{
    public partial class MainWindow : Window
    {
        KinectControl kinectCtrl = new KinectControl();

        public MainWindow()
        {
            InitializeComponent();
            TrackingView view1 = new TrackingView(kinectCtrl.sensor);
            view1.Show();
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



        private void txtMouseSensitivity_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                float v;
                if (float.TryParse(txtMouseSensitivity.Text, out v))
                {
                    MouseSensitivity.Value = v;
                    kinectCtrl.mouseSensitivity = (float)MouseSensitivity.Value;
                }
            }
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            MouseSensitivity.Value = Properties.Settings.Default.MouseSensitivity;
            chkNoClick.IsChecked = !Properties.Settings.Default.DoClick;
            CursorSmoothing.Value = Properties.Settings.Default.CursorSmoothing;


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
            kinectCtrl.Close();
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


    }


}
