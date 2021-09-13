using ScpDriverInterface;
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
            GetLayouts();
        }

        private void GetLayouts()
        {
            string[] layouts = Directory.GetDirectories("layouts", "*", SearchOption.TopDirectoryOnly);
            LayoutList.Rows = layouts.Length;
            for (int i = 0; i < layouts.Length; ++i)
            {
                string[] layoutPathParts = layouts[i].Split('\\');
                Button button = new Button()
                {
                    Width = 200,
                    Content = string.Format(layoutPathParts[layoutPathParts.Length - 1]),
                    Tag = layouts[i] + "\\"
                };
                button.Click += new RoutedEventHandler(LayoutClicked);
                LayoutList.Children.Add(button);
            }
        }

        private void LayoutClicked(object sender, RoutedEventArgs e)
        {
            LayoutListUI.Visibility = Visibility.Collapsed;
            loadCustomUi((sender as Button).Tag.ToString());
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
            string tag = ((Ellipse)e.ManipulationContainer).Tag.ToString().ToUpper();
            StickCurrentPoint = e.ManipulationOrigin;
            if (tag == "STICK:WASD")
            {
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
            if (tag.StartsWith("STICK:X360")) {
                string side = "";
                if (tag.Substring(10) == ".RIGHT")
                    side = "RIGHT";
                if (tag.Substring(10) == ".LEFT")
                    side = "LEFT";
                double xDelta = StickCurrentPoint.X - StickStartPoint.X;
                if (xDelta > 50)
                    xDelta = 50;
                if (xDelta < -50)
                    xDelta = -50;
                double yDelta = StickCurrentPoint.Y - StickStartPoint.Y;
                if (yDelta > 50)
                    yDelta = 50;
                if (yDelta < -50)
                    yDelta = -50;
                if (xDelta != 0)
                    xDelta = xDelta / 50;
                if (yDelta != 0)
                    yDelta = yDelta / 50;
                if (side == "LEFT")
                {
                    controller.LeftStickX = Convert.ToInt16(xDelta * 32767);
                    controller.LeftStickY = Convert.ToInt16(-(yDelta * 32767));
                    scpBus.Report(1, controller.GetReport());
                }
                if (side == "RIGHT")
                {
                    controller.RightStickX = Convert.ToInt16(xDelta * 32767);
                    controller.RightStickY = Convert.ToInt16(-(yDelta * 32767));
                    scpBus.Report(1, controller.GetReport());
                }
            }

        }

        protected override void OnManipulationCompleted(ManipulationCompletedEventArgs e)
        {
            string tag = ((Ellipse)e.ManipulationContainer).Tag.ToString().ToUpper();
            if (tag == "STICK:WASD")
                wasdKeys(false, false, false, false);
            if (tag == "STICK:X360.LEFT")
            {
                controller.LeftStickX = 0;
                controller.LeftStickY = 0;
                scpBus.Report(1, controller.GetReport());
            }
            if (tag == "STICK:X360.RIGHT")
            {
                controller.RightStickX = 0;
                controller.RightStickY = 0;
                scpBus.Report(1, controller.GetReport());
            }
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
                tag = ((Rectangle)sender).Tag.ToString().ToUpper();
            if (sender.GetType().ToString() == "System.Windows.Shapes.Ellipse")
                tag = ((Ellipse)sender).Tag.ToString().ToUpper();
            if (sender.GetType().ToString() == "System.Windows.Controls.Image")
                tag = ((Image)sender).Tag.ToString().ToUpper();
            if (tag.StartsWith("KECODE:"))
            {
                UInt16 keyCode = UInt16.Parse(tag.Substring(8));
                KeyboardOutput.performKeyDown(keyCode);
            }
            if (tag.StartsWith("XINPUT:"))
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
                tag = ((Rectangle)sender).Tag.ToString().ToUpper();
            if (sender.GetType().ToString() == "System.Windows.Shapes.Ellipse")
                tag = ((Ellipse)sender).Tag.ToString().ToUpper();
            if (sender.GetType().ToString() == "System.Windows.Controls.Image")
                tag = ((Image)sender).Tag.ToString().ToUpper();
            if (tag.StartsWith("KEYCODE:"))
            {
                UInt16 keyCode = UInt16.Parse(tag.Substring(8));
                KeyboardOutput.performKeyRelease(keyCode);
            }
            if (tag.StartsWith("XINPUT:"))
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

        private void loadCustomUi(string layoutFile)
        {
            try
            {
                bool xinputControls = false;
                string content = File.ReadAllText(layoutFile + "layout.xaml");
                string path = AppDomain.CurrentDomain.BaseDirectory + layoutFile;
                content = content.Replace("$layoutdir/", path);
                Grid grd = new Grid();
                var grdEncoding = new ASCIIEncoding();
                var grdBytes = grdEncoding.GetBytes(content);
                grd = (Grid)XamlReader.Load(new MemoryStream(grdBytes));
                Grid.SetColumn(grd, 0);
                Grid.SetRow(grd, 0);
                custom_ui.Children.Clear();
                custom_ui.Children.Add(grd);
                grd = (Grid)custom_ui.Children[0];
                for(int i = 0; i < grd.Children.Count; i++)
                {
                    if (grd.Children[i].GetType().ToString() == "System.Windows.Shapes.Rectangle")
                    {
                        grd.Children[i].AddHandler(Rectangle.TouchEnterEvent, new RoutedEventHandler(button_TouchDown));
                        grd.Children[i].AddHandler(Rectangle.TouchLeaveEvent, new RoutedEventHandler(button_TouchUp));
                        if (((Rectangle)grd.Children[i]).Tag.ToString().StartsWith("xinput:"))
                            xinputControls = true;
                    }
                    if (grd.Children[i].GetType().ToString() == "System.Windows.Shapes.Ellipse")
                    {
                        grd.Children[i].AddHandler(Ellipse.TouchEnterEvent, new RoutedEventHandler(button_TouchDown));
                        grd.Children[i].AddHandler(Ellipse.TouchLeaveEvent, new RoutedEventHandler(button_TouchUp));
                        if (((Ellipse)grd.Children[i]).Tag.ToString().StartsWith("xinput:"))
                            xinputControls = true;
                    }
                    if (grd.Children[i].GetType().ToString() == "System.Windows.Controls.Image")
                    {
                        grd.Children[i].AddHandler(Image.TouchEnterEvent, new RoutedEventHandler(button_TouchDown));
                        grd.Children[i].AddHandler(Image.TouchLeaveEvent, new RoutedEventHandler(button_TouchUp));
                        if (((Image)grd.Children[i]).Tag.ToString().StartsWith("xinput:"))
                            xinputControls = true;
                    }
                    if (grd.Children[i].GetType().ToString() == "System.Windows.Controls.UserControl")
                    {
                        UserControl controlField = (UserControl)grd.Children[i];
                        Ellipse controlContent = (Ellipse)controlField.Content;
                        if (controlContent.Tag.ToString().ToUpper().StartsWith("STICK:X360"))
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
            switch(button)
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
