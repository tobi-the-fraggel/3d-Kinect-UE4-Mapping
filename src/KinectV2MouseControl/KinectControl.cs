using System;
using System.Windows;
using System.Windows.Threading;
using System.Windows.Media;
using Microsoft.Kinect;
using LightBuzz.Vitruvius;
using System.ComponentModel;
using System.Diagnostics;

namespace Mousenect
{
    public class KinectControl
    {
        /// <summary>
        /// Active Kinect sensor
        /// </summary>
        public KinectSensor sensor;
        /// <summary>
        /// Timer für Gesten-Pausen
        /// </summary>
        DispatcherTimer timer = new DispatcherTimer();
        /// <summary>
        /// Vitruvius erzeugter GestureController
        /// </summary>
        GestureController gestureController;
        /// <summary>
        /// Vitruvius Erzeugter MultiSourceFrameReader
        /// </summary>
        MultiSourceFrameReader _reader;
        /// <summary>
        /// Screen width and height for determining the exact mouse sensitivity
        /// </summary>
        int screenWidth, screenHeight;
        /// <summary>
        /// How far the cursor move according to your hand's movement
        /// </summary>
        public float mouseSensitivity = MOUSE_SENSITIVITY;
        /// <summary>
        /// Decide if the user need to do clicks or only move the cursor
        /// </summary>
        public bool doClick = DO_CLICK;
        /// <summary>
        /// Value 0 - 0.95f, the larger it is, the smoother the cursor would move
        /// </summary>
        public float cursorSmoothing = CURSOR_SMOOTHING;

        // Default values
        public const float MOUSE_SENSITIVITY = 3.5f;
        public const bool DO_CLICK = true;
        public const float CURSOR_SMOOTHING = 0.2f;

        /// <summary>
        /// For storing last cursor position
        /// </summary>
        Point lastCurPos = new Point(0, 0);
        /// <summary>
        /// If true, user did a left hand Grip gesture
        /// </summary>
        bool wasLeftGrip = false;
        /// <summary>
        /// Verzögerung zwischen den Gesten
        /// </summary>
        bool wasGesture = false;
        bool wasDoubleLasso = false;
        int GestureCount = 0;
        int DoubleLassoCount = 0;
        int GripCount = 0;

        /// <summary>
        /// Variable zum aktivieren/deaktivieren der Steuerung
        /// </summary>
        bool Steering_Active = false;

        double angle = 0;

        /// <summary>
        /// Variable zur Steuerung verschiedener Programme
        /// Maus = 1
        /// Powerpoint = 2
        /// Zeichnen = 3
        /// </summary>
        public byte Programm = 1;
        DrawingWindow drawing;

        public KinectControl()
        {
            // get Active Kinect Sensor
            sensor = KinectSensor.GetDefault();

            // get screen with and height
            screenWidth = (int)SystemParameters.PrimaryScreenWidth;
            screenHeight = (int)SystemParameters.PrimaryScreenHeight;

            //PropertyChanged Event verknüpfen
            Properties.Settings.Default.PropertyChanged += KinectControl_PropertyChanged;

            #region Timer-Setup
            timer.Interval = new TimeSpan(0, 0, 0, 0, 1000);
            timer.Tick += new EventHandler(Timer_Tick);
            #endregion

            //Default für Programm (Maus)
            Programm = Properties.Settings.Default.Programm;

            // open the sensor
            sensor.Open();

            //Vitruvius
            _reader = sensor.OpenMultiSourceFrameReader(FrameSourceTypes.Body);
            _reader.MultiSourceFrameArrived += Reader_MultiSourceFrameArrived;
            gestureController = new GestureController();
            gestureController.GestureRecognized += GestureController_GestureRecognized;
        }

        private void KinectControl_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "CursorSmoothing")
            {
                this.cursorSmoothing = Properties.Settings.Default.CursorSmoothing;
                Console.WriteLine("KinectControl: CursorSmoothing geändert auf " + this.cursorSmoothing);
            }
            else if (e.PropertyName == "MouseSensitivity")
            {
                this.mouseSensitivity = Properties.Settings.Default.MouseSensitivity;
                Console.WriteLine("KinectControl: MouseSensitivity geändert auf " + this.mouseSensitivity);
            }
            else if (e.PropertyName == "DoClick")
            {
                this.doClick = Properties.Settings.Default.DoClick;
                Console.WriteLine("KinectControl: DoClick geändert auf " + this.doClick);
            }
            else if (e.PropertyName == "Programm")
            {
                this.Programm = Properties.Settings.Default.Programm;
                Console.WriteLine("KinectControl: Programm geändert auf " + this.Programm);

                if (this.Programm == 3)
                {
                    drawing = new DrawingWindow(this);
                    drawing.Show();
                    Console.WriteLine("DrawingWindow geöffnet");
                }
                else if(this.Programm == 4)
                {
                    Process.Start("wmplayer.exe");
                    Console.WriteLine("Mediaplayer geöffnet");
                }
            }
            else if (e.PropertyName == "Steering_Active")
            {
                this.Steering_Active = Properties.Settings.Default.Steering_Active;
                Console.WriteLine("KinectControl: SteeringActive geändert auf " + this.Steering_Active);
            }
        }

        #region Timer-Tick's
        /// <summary>
        /// Nach einer Sekunde ausgelöst um mehrere Gesten-Auslöser zu vermeiden
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Timer_Tick(object sender, EventArgs e)
        {
            Console.WriteLine("Gesten-Pause vorbei");
            wasGesture = false;
            wasDoubleLasso = false;
            timer.Stop();
        }
        #endregion

        /// <summary>
        /// Event wird bei erkannter Geste ausgelöst
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">enthält die erkannte Geste</param>
        void GestureController_GestureRecognized(object sender, GestureEventArgs e)
        {
            //Geste nur nach einer Sekunde zur vorherigen auswerten
            if (!wasGesture)
            {
                wasGesture = true;
                timer.Start();

                if(Programm == 4) //Media Player Gesten
                {
                    if (e.GestureType == GestureType.SwipeLeft)
                        InputControl.MediaPrev();
                    else if (e.GestureType == GestureType.SwipeRight)
                        InputControl.MediaNext();
                    else if (e.GestureType == GestureType.JoinedHands)
                        InputControl.MediaPlayPause();
                }

                //Debugging Konsolen-Ausgabe
                Console.WriteLine("Geste wurde erkannt: " + e.GestureType.ToString());
            }
        }

        /// <summary>
        /// Event wird bei jedem eingehenden Frame ausgelöst
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Reader_MultiSourceFrameArrived(object sender, MultiSourceFrameArrivedEventArgs e)
        {
            //Steuerung aktiv
            if (Steering_Active)
            {
                var reference = e.FrameReference.AcquireFrame();
                // Body
                using (var frame = reference.BodyFrameReference.AcquireFrame())
                {
                    if (frame != null)
                    {
                        //Den Closest Body wählen
                        Body body = frame.Bodies().Closest();

                        // get closest tracked body only, notice there's a break below.
                        if (body != null)
                        {
                            //Gesture-Controller das neuste Frame liefern
                            gestureController.Update(body);
                            //Winkel Update
                            Joint shoulder = body.Joints[JointType.ShoulderLeft];
                            Joint elbow = body.Joints[JointType.ElbowLeft];
                            Joint wrist = body.Joints[JointType.WristLeft];
                            angle = elbow.Angle(shoulder, wrist);

                            //2x Lasso zum beenden der Anwendung muss 30 Frames gehalten werden
                            if (body.HandLeftState == HandState.Lasso && body.HandRightState == HandState.Lasso)
                            {
                                if (!wasDoubleLasso)
                                {
                                    wasDoubleLasso = true;
                                    DoubleLassoCount = 0;
                                }
                                else
                                {
                                    DoubleLassoCount++;
                                    if (DoubleLassoCount > 30)
                                        System.Environment.Exit(0);
                                }
                            }
                            else
                            {
                                wasDoubleLasso = false;
                                DoubleLassoCount = 0;
                            }

                            // Positionen der linken, rechten Hand und Rücken auslesen
                            CameraSpacePoint handLeft = body.Joints[JointType.HandLeft].Position;
                            CameraSpacePoint handRight = body.Joints[JointType.HandRight].Position;
                            CameraSpacePoint spineBase = body.Joints[JointType.SpineBase].Position;

                            //MausSteuerung
                            if (Programm == 1)
                            {
                                //Wenn Hände erkannt -> Maus-Zeiger bewegen
                                if (body.HandRightState != HandState.Unknown && body.HandRightState != HandState.NotTracked) // Rechte Hand muss getrackt sein
                                {
                                    /* hand x calculated by this. we don't use shoulder right as a reference cause the shoulder right
                                     * is usually behind the lift right hand, and the position would be inferred and unstable.
                                     * because the spine base is on the left of right hand, we plus 0.05f to make it closer to the right. */
                                    float x = handRight.X - spineBase.X + 0.05f;
                                    /* hand y calculated by this. ss spine base is way lower than right hand, we plus 0.51f to make it
                                     * higher, the value 0.51f is worked out by testing for a several times, you can set it as another one you like. */
                                    float y = spineBase.Y - handRight.Y + 0.51f;
                                    // get current cursor position
                                    Point curPos = InputControl.GetCursorPosition();
                                    // smoothing for using should be 0 - 0.95f. The way we smooth the cusor is: oldPos + (newPos - oldPos) * smoothValue
                                    float smoothing = 1 - cursorSmoothing;
                                    // set cursor position
                                    InputControl.SetCursorPos((int)(curPos.X + (x * mouseSensitivity * screenWidth - curPos.X) * smoothing), (int)(curPos.Y + ((y + 0.25f) * mouseSensitivity * screenHeight - curPos.Y) * smoothing));

                                    // Grip gesture
                                    if (doClick)
                                    {
                                        if (body.HandLeftState == HandState.Closed)
                                        {
                                            if (!wasLeftGrip)
                                            {
                                                GripCount = 0;
                                            }
                                            else
                                            {
                                                GripCount++;
                                                if (GripCount > 50)
                                                {
                                                    InputControl.MouseLeftDown();
                                                }
                                            }
                                            wasLeftGrip = true;
                                        }
                                        else if (body.HandLeftState == HandState.Open)
                                        {
                                            if (wasLeftGrip)
                                            {
                                                if (GripCount <= 50)
                                                {
                                                    InputControl.DoMouseClick();
                                                }
                                                GripCount = 0;
                                                InputControl.MouseLeftUp();
                                            }
                                            else
                                            {
                                                GripCount = 0;
                                            }
                                            wasLeftGrip = false;
                                        }
                                    }
                                } // Ende der Maus-Bewegung
                            }
                            //PowerPoint-Steuerung
                            else if (Programm == 2)
                            {
                                if (handLeft.Z - spineBase.Z < -0.15f) // if left hand lift forward
                                {
                                    if (body.HandLeftState == HandState.Open)
                                    {
                                        GestureCount++;
                                        if (GestureCount > 30)
                                        {
                                            InputControl.PressLeftArrowKey();
                                            Console.WriteLine("PowerPoint: Pfeiltaste LINKS");
                                            GestureCount = 0;
                                        }
                                    }
                                    else
                                    {
                                        GestureCount = 0;
                                    }
                                }
                                else if (handRight.Z - spineBase.Z < -0.15f) // if right hand lift forward
                                {
                                    if (body.HandRightState == HandState.Open)
                                    {
                                        GestureCount++;
                                        if (GestureCount > 30)
                                        {
                                            InputControl.PressRightArrowKey();
                                            Console.WriteLine("PowerPoint: Pfeiltaste RECHTS");
                                            GestureCount = 0;
                                        }
                                    }
                                    else
                                    {
                                        GestureCount = 0;
                                    }
                                }
                                else
                                {
                                    GestureCount = 0;
                                }

                            }
                        }
                    }
                }
            }
        }
    }
}
