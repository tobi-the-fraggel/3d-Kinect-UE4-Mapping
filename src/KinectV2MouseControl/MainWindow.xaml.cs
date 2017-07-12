using System.Windows;
using Microsoft.Kinect;
using LightBuzz.Vitruvius;
using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.ComponentModel;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using System.IO;


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
            settings = new SettingsWindow();
            Properties.Settings.Default.PropertyChanged += MainWindow_PropertyChanged;

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

        private void MainWindow_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Programm")
            {
                MI_Maus.IsChecked = false;
                MI_PP.IsChecked = false;
                MI_Zeichnung.IsChecked = false;
                MI_Media.IsChecked = false;

                switch (Properties.Settings.Default.Programm)
                {
                    case 1:
                        MI_Maus.IsChecked = true;
                        break;
                    case 2:
                        MI_PP.IsChecked = true;
                        break;
                    case 3:
                        MI_Zeichnung.IsChecked = true;
                        break;
                    case 4:
                        MI_Media.IsChecked = true;
                        break;
                    default:
                        Console.WriteLine("MainWindows: FEHLER Programm war über der Anzahl");
                        MI_Maus.IsChecked = true;
                        Properties.Settings.Default.Programm = 1;
                        break;
                }

                Properties.Settings.Default.Steering_Active = false;
                Properties.Settings.Default.Save();

            }
            else if(e.PropertyName == "Steering_Active")
            {
                if(Properties.Settings.Default.Steering_Active == false)
                {
                    btn_activate.Background = Brushes.ForestGreen;
                    btn_activate.Content = "Steuerung aktivieren";
                }
                else
                {
                    btn_activate.Background = Brushes.Red;
                    btn_activate.Content = "Steuerung deaktivieren";
                }

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

            // Depth
            using (var frame = reference.DepthFrameReference.AcquireFrame())
            {
                if (frame != null)
                {
                    if (viewer.Visualization == Visualization.Depth)
                    {
                        viewer.Image = frame.ToBitmap();
                    }
                }
            }

            // Infrared
            using (var frame = reference.InfraredFrameReference.AcquireFrame())
            {
                if (frame != null)
                {
                    if (viewer.Visualization == Visualization.Infrared)
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
                        Heigth.Text = "Persongröße: " + closest.Height().ToString("f2") + " m";
                    }
                    else
                        viewer.Clear();
                }
            }
        }

        void UserReporter_BodyEntered(object sender, PlayersControllerEventArgs e)
        {
            // A new user has entered the scene.
            Console.WriteLine("Neuer Body in der Szene");
            viewer.Clear();
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

            switch (mi.Header)
            {
                case "Maus":
                    Properties.Settings.Default.Programm = 1;
                    break;
                case "PowerPoint":
                    Properties.Settings.Default.Programm = 2;
                    break;
                case "Zeichnen":
                    Properties.Settings.Default.Programm = 3;
                    break;
                case "MediaPlayer (Windows)":
                    Properties.Settings.Default.Programm = 4;
                    break;
                default:
                    Properties.Settings.Default.Programm = 1;
                    Console.WriteLine("MainWindows: Fehler im MenuItem_Click");
                    break;
            }

            Properties.Settings.Default.Save();
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
            setVisual();
            //Defaults
            Properties.Settings.Default.Steering_Active = false;
            Properties.Settings.Default.Programm = 1;
        }

        private void btn_activate_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.Steering_Active = !Properties.Settings.Default.Steering_Active;
        }

        private void btn_screenshot_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Button Screenshot gedrückt");

            string path = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures), "vitruvius-capture.jpg");

            // Displays a SaveFileDialog so the user can save the Image
            // assigned to Button2.
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "JPeg Image|*.jpg";
            saveFileDialog.Title = "Save an Image File";
            saveFileDialog.ShowDialog();

            // If the file name is not an empty string open it for saving.
            if (saveFileDialog.FileName != "")
            {
                

                
            }
            string sfdname = saveFileDialog.FileName;
           
                Path.GetFileName(saveFileDialog.FileName);
            //Read Full Path of a Objekt ,ER
            string fullPath = System.IO.Path.GetDirectoryName(sfdname);

            Console.WriteLine(sfdname);
            // Save Bitmap to the Path
            (viewer.Image as WriteableBitmap).Save(sfdname);

        }

        private void btn_camera_Click(object sender, RoutedEventArgs e)
        {
            switch (Properties.Settings.Default.ChoosenVisual)
            {
                case "Color":
                    Properties.Settings.Default.ChoosenVisual = "Depth";
                    break;
                case "Depth":
                    Properties.Settings.Default.ChoosenVisual = "Infrared";
                    break;
                case "Infrared":
                    Properties.Settings.Default.ChoosenVisual = "Color";
                    break;
                default:
                    Console.WriteLine("MainWindow: Fehler bei btnCamera");
                    break;
            }
            Properties.Settings.Default.Save();
            setVisual();
        }

        private void setVisual()
        {
            switch (Properties.Settings.Default.ChoosenVisual)
            {
                case "Color":
                    viewer.Visualization = Visualization.Color;
                    Visual.Text = "Color";
                    break;
                case "Depth":
                    viewer.Visualization = Visualization.Depth;
                    Visual.Text = "Depth";
                    break;
                case "Infrared":
                    viewer.Visualization = Visualization.Infrared;
                    Visual.Text = "Infrared";
                    break;
                default:
                    Console.WriteLine("MainWindow: Fehler bei setVisual");
                    break;
            }
        }
    }
}