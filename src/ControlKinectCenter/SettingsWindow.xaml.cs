using System;
using System.Windows;

namespace ControlKinectCenter
{
    /// <summary>
    /// Interaktionslogik für SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        public SettingsWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            MouseSensitivity.Value = Properties.Settings.Default.MouseSensitivity;
            chkNoClick.IsChecked = !Properties.Settings.Default.DoClick;
            chkNoNotify.IsChecked = !Properties.Settings.Default.ShowToast;
            CursorSmoothing.Value = Properties.Settings.Default.CursorSmoothing;

        }

        private void MouseSensitivity_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (MouseSensitivity.IsLoaded)
            {
                txtMouseSensitivity.Text = MouseSensitivity.Value.ToString("f2");
                Properties.Settings.Default.MouseSensitivity = (float)MouseSensitivity.Value;
                Properties.Settings.Default.Save();
                Console.WriteLine("Mouse Sensitivity wurde verändert");
            }
        }

        private void btnDefault_Click(object sender, RoutedEventArgs e)
        {
            MouseSensitivity.Value = KinectControl.MOUSE_SENSITIVITY;
            CursorSmoothing.Value = KinectControl.CURSOR_SMOOTHING;
            chkNoClick.IsChecked = !KinectControl.DO_CLICK;
            chkNoNotify.IsChecked = false;
            Console.WriteLine("Werte auf Standard zurückgesetzt");
        }

        #region NoClick-Checker
        private void chkNoClick_Checked(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.DoClick = false;
            Properties.Settings.Default.Save();
            Console.WriteLine("NoClick wurde verändert");
        }

        private void chkNoClick_Unchecked(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.DoClick = true;
            Properties.Settings.Default.Save();
            Console.WriteLine("NoClick wurde verändert");

        }
        #endregion

        private void CursorSmoothing_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (CursorSmoothing.IsLoaded)
            {
                txtCursorSmoothing.Text = CursorSmoothing.Value.ToString("f2");
                Properties.Settings.Default.CursorSmoothing = (float)CursorSmoothing.Value;
                Properties.Settings.Default.Save();
            }
        }

        private void btn_close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.Close();
        }

        private void chkNoNotify_Checked(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Keine Benachrichtigungen geklickt");
            Properties.Settings.Default.ShowToast = false;
            Properties.Settings.Default.Save();
        }

        private void chkNoNotify_Unchecked(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Benachrichtigungen geklickt");
            Properties.Settings.Default.ShowToast = true;
            Properties.Settings.Default.Save();
        }
    }
}
