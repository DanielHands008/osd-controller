﻿using ScpDriverInterface;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace osd_buttons
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Mutex m;
        public MainWindow()
        {
            bool isnew;
            m = new Mutex(true, "HXG6qA7Tx#e2@IBSvpa4AySyD7KZj1D4", out isnew);
            if (!isnew)
            {
                Environment.Exit(0);
            }
            InitializeComponent();
        }

        Point StickStartPoint = new Point(0, 0);
        Point StickCurrentPoint = new Point(0, 0);
        Point StickDeltaPoint = new Point(0, 0);
        bool left = false, right = false, up = false, down = false;
        double distance = 0, angle = 0;

        ScpBus scpBus = new ScpBus();
        X360Controller controller = new X360Controller();

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show(X360Buttons.A.GetType().ToString());
            loadCustomUi();
        }

        protected override void OnManipulationStarted(ManipulationStartedEventArgs e)
        {
            StickStartPoint = e.ManipulationOrigin;
            StickCurrentPoint = e.ManipulationOrigin;
            distance = 0;
            angle = 0;
        }

        protected override void OnManipulationDelta(ManipulationDeltaEventArgs e)
        {
            StickCurrentPoint = e.ManipulationOrigin;
            distance = Point.Subtract(StickStartPoint, StickCurrentPoint).Length;
            angle = (Math.Atan2(StickCurrentPoint.Y - StickStartPoint.Y, StickCurrentPoint.X - StickStartPoint.X) * 180.0 / Math.PI) + 180;
            int quadrant = 0;
            if (distance > 30)
            {
                if (angle < 22.5 || angle >= 337.5)
                    quadrant = 1;
                if (angle >= 22.5 && angle < 67.5)
                    quadrant = 2;
                if (angle >= 67.5 && angle < 112.5)
                    quadrant = 3;
                if (angle >= 112.5 && angle < 157.5)
                    quadrant = 4;
                if (angle >= 157.5 && angle < 202.5)
                    quadrant = 5;
                if (angle >= 202.5 && angle < 247.5)
                    quadrant = 6;
                if (angle >= 247.5 && angle < 292.5)
                    quadrant = 7;
                if (angle >= 292.5 && angle < 337.5)
                    quadrant = 8;
            }
            switch (quadrant)
            {
                case 1:
                    wasdKeys(false, true, false, false);
                    break;
                case 2:
                    wasdKeys(true, true, false, false);
                    break;
                case 3:
                    wasdKeys(true, false, false, false);
                    break;
                case 4:
                    wasdKeys(true, false, false, true);
                    break;
                case 5:
                    wasdKeys(false, false, false, true);
                    break;
                case 6:
                    wasdKeys(false, false, true, true);
                    break;
                case 7:
                    wasdKeys(false, false, true, false);
                    break;
                case 8:
                    wasdKeys(false, true, true, false);
                    break;
                default:
                    wasdKeys(false, false, false, false);
                    break;
            }

        }

        protected override void OnManipulationCompleted(ManipulationCompletedEventArgs e)
        {
            wasdKeys(false, false, false, false);
        }

        void wasdKeys(bool iup, bool ileft, bool idown, bool iright)
        {
            if (iup)
            {
                if (!up)
                {
                    up = true;
                    KeyboardOutput.performKeyDown(87);
                }
            }
            else
            {
                if (up)
                {
                    up = false;
                    KeyboardOutput.performKeyRelease(87);
                }
            }
            if (ileft)
            {
                if (!left)
                {
                    left = true;
                    KeyboardOutput.performKeyDown(65);
                }
            }
            else
            {
                if (left)
                {
                    left = false;
                    KeyboardOutput.performKeyRelease(65);
                }
            }
            if (idown)
            {
                if (!down)
                {
                    down = true;
                    KeyboardOutput.performKeyDown(83);
                }
            }
            else
            {
                if (down)
                {
                    down = false;
                    KeyboardOutput.performKeyRelease(83);
                }
            }
            if (iright)
            {
                if (!right)
                {
                    right = true;
                    KeyboardOutput.performKeyDown(68);
                }
            }
            else
            {
                if (right)
                {
                    right = false;
                    KeyboardOutput.performKeyRelease(68);
                }
            }
        }



        private void button_TouchDown(object sender, EventArgs e)
        {
            string tag = "";
            if (sender.GetType().ToString() == "System.Windows.Shapes.Rectangle")
                tag = ((Rectangle)sender).Tag.ToString();
            if (sender.GetType().ToString() == "System.Windows.Shapes.Ellipse")
                tag = ((Ellipse)sender).Tag.ToString();
            if (sender.GetType().ToString() == "System.Windows.Controls.Image")
                tag = ((Image)sender).Tag.ToString();
            if (tag.StartsWith("keycode:"))
            {
                UInt16 keyCode = UInt16.Parse(tag.Substring(8));
                KeyboardOutput.performKeyDown(keyCode);
            }
            if (tag.StartsWith("xinput:"))
            {
                string button = tag.Substring(7);
                if (button.ToUpper() == "RT")
                {
                    controller.RightTrigger = 255;
                }
                else if (button.ToUpper() == "LT")
                {
                    controller.LeftTrigger = 255;
                }
                else
                {
                    controller.Buttons |= StringToX360(button);
                }
                
                scpBus.Report(1, controller.GetReport());
            }
        }
        private void button_TouchUp(object sender, EventArgs e)
        {
            string tag = "";
            if (sender.GetType().ToString() == "System.Windows.Shapes.Rectangle")
                tag = ((Rectangle)sender).Tag.ToString();
            if (sender.GetType().ToString() == "System.Windows.Shapes.Ellipse")
                tag = ((Ellipse)sender).Tag.ToString();
            if (sender.GetType().ToString() == "System.Windows.Controls.Image")
                tag = ((Image)sender).Tag.ToString();
            if (tag.StartsWith("keycode:"))
            {
                UInt16 keyCode = UInt16.Parse(tag.Substring(8));
                KeyboardOutput.performKeyRelease(keyCode);
            }
            if (tag.StartsWith("xinput:"))
            {
                string button = tag.Substring(7);
                if (button.ToUpper() == "RT")
                {
                    controller.RightTrigger = 0;
                }
                else if (button.ToUpper() == "LT")
                {
                    controller.LeftTrigger = 0;
                }
                else
                {
                    controller.Buttons &= ~StringToX360(button);
                }

                scpBus.Report(1, controller.GetReport());
            }
        }


        private void keyboard_Open(object sender, RoutedEventArgs e)
        {
            Process.Start(@"C:\Program Files\Common Files\microsoft shared\ink\TabTip.exe");
        }

        private void show_close_dialog(object sender, RoutedEventArgs e)
        {
            close_dialog.Visibility = Visibility.Visible;
        }
        private void hide_close_dialog(object sender, RoutedEventArgs e)
        {
            close_dialog.Visibility = Visibility.Hidden;
        }

        private void close_App(object sender, RoutedEventArgs e)
        {
            scpBus.UnplugAll();
            System.Windows.Application.Current.Shutdown();
        }

        private void loadCustomUi()
        {
            try
            {
                bool xinputControls = false;
                string content = File.ReadAllText("layout.xaml");
                Grid grd = new Grid();
                var grdEncoding = new ASCIIEncoding();
                var grdBytes = grdEncoding.GetBytes(content);
                grd = (Grid)XamlReader.Load(new MemoryStream(grdBytes));
                Grid.SetColumn(grd, 0);
                Grid.SetRow(grd, 0);
                custom_ui.Children.Add(grd);
                Grid grid = (Grid)custom_ui.Children[0];
                for(int i = 0; i < grid.Children.Count; i++)
                {
                    if (grid.Children[i].GetType().ToString() == "System.Windows.Shapes.Rectangle")
                    {
                        grid.Children[i].AddHandler(Rectangle.TouchEnterEvent, new RoutedEventHandler(button_TouchDown));
                        grid.Children[i].AddHandler(Rectangle.TouchLeaveEvent, new RoutedEventHandler(button_TouchUp));
                        if (((Rectangle)grid.Children[i]).Tag.ToString().StartsWith("xinput:"))
                            xinputControls = true;
                    }
                    if (grid.Children[i].GetType().ToString() == "System.Windows.Shapes.Ellipse")
                    {
                        grid.Children[i].AddHandler(Ellipse.TouchEnterEvent, new RoutedEventHandler(button_TouchDown));
                        grid.Children[i].AddHandler(Ellipse.TouchLeaveEvent, new RoutedEventHandler(button_TouchUp));
                        if (((Ellipse)grid.Children[i]).Tag.ToString().StartsWith("xinput:"))
                            xinputControls = true;
                    }
                    if (grid.Children[i].GetType().ToString() == "System.Windows.Controls.Image")
                    {
                        grid.Children[i].AddHandler(Image.TouchEnterEvent, new RoutedEventHandler(button_TouchDown));
                        grid.Children[i].AddHandler(Image.TouchLeaveEvent, new RoutedEventHandler(button_TouchUp));
                        if (((Image)grid.Children[i]).Tag.ToString().StartsWith("xinput:"))
                            xinputControls = true;
                    }
                }
                if (xinputControls)
                    scpBus.PlugIn(1);
            }
            catch (FileNotFoundException ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        static X360Buttons StringToX360(string button)
        {
            switch(button.ToUpper())
            {
                case "A":
                    return X360Buttons.A;
                case "B":
                    return X360Buttons.B;
                case "X":
                    return X360Buttons.X;
                case "Y":
                    return X360Buttons.Y;
                case "START":
                    return X360Buttons.Start;
                case "BACK":
                    return X360Buttons.Back;
                case "LB":
                    return X360Buttons.LeftBumper;
                case "RB":
                    return X360Buttons.RightBumper;
                case "LS":
                    return X360Buttons.LeftStick;
                case "RS":
                    return X360Buttons.RightStick;
                case "UP":
                    return X360Buttons.Up;
                case "DOWN":
                    return X360Buttons.Down;
                case "LEFT":
                    return X360Buttons.Left;
                case "RIGHT":
                    return X360Buttons.Right;
                case "LOGO":
                    return X360Buttons.Logo;
                default:
                    return X360Buttons.None;
            }
        }

        #region Prevent Focus

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);

            //Set the window style to noactivate.
            WindowInteropHelper helper = new WindowInteropHelper(this);
            SetWindowLong(helper.Handle, GWL_EXSTYLE,
                GetWindowLong(helper.Handle, GWL_EXSTYLE) | WS_EX_NOACTIVATE);
        }

        private const int GWL_EXSTYLE = -20;
        private const int WS_EX_NOACTIVATE = 0x08000000;

        [DllImport("user32.dll")]
        public static extern IntPtr SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll")]
        public static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        #endregion


    }


}
