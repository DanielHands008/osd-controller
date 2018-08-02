using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
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
        public MainWindow()
        {
            InitializeComponent();
        }

        private KeyboardHook _hook;
        
        int place = 1;

        Point StickStartPoint = new Point(0, 0);
        Point StickCurrentPoint = new Point(0, 0);
        Point StickDeltaPoint = new Point(0, 0);
        bool StickDown = false, left = false, right = false, up = false, down = false;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // install system-wide keyboard hook
            _hook = new KeyboardHook();
            _hook.KeyDown += new KeyboardHook.HookEventHandler(OnHookKeyDown);
            setvis();

        }

        void OnHookKeyDown(object sender, HookEventArgs e)
        {
            
            if(e.Key == Key.F18 && place < 10)
            {
                place++;
                setvis();
            }
            if (e.Key == Key.F20 && place > 1)
            {
                place--;
                setvis();
            }
            if (e.Key == Key.F17)
            {
                switch(place)
                {
                    case 1:
                        KeyboardOutput.performKeyPress(49);
                        break;
                    case 2:
                        KeyboardOutput.performKeyPress(50);
                        break;
                    case 3:
                        KeyboardOutput.performKeyPress(51);
                        break;
                    case 4:
                        KeyboardOutput.performKeyPress(52);
                        break;
                    case 5:
                        KeyboardOutput.performKeyPress(53);
                        break;
                    case 6:
                        KeyboardOutput.performKeyPress(54);
                        break;
                    case 7:
                        KeyboardOutput.performKeyPress(55);
                        break;
                    case 8:
                        KeyboardOutput.performKeyPress(56);
                        break;
                    case 9:
                        KeyboardOutput.performKeyPress(57);
                        break;
                    case 10:
                        KeyboardOutput.performKeyPress(48);
                        break;
                    default:
                        break;
                }
            }
        }
        void setvis()
        {
            if (place == 1) rectangle_1.Visibility = Visibility.Visible; else rectangle_1.Visibility = Visibility.Hidden;
            if (place == 2) rectangle_2.Visibility = Visibility.Visible; else rectangle_2.Visibility = Visibility.Hidden;
            if (place == 3) rectangle_3.Visibility = Visibility.Visible; else rectangle_3.Visibility = Visibility.Hidden;
            if (place == 4) rectangle_4.Visibility = Visibility.Visible; else rectangle_4.Visibility = Visibility.Hidden;
            if (place == 5) rectangle_5.Visibility = Visibility.Visible; else rectangle_5.Visibility = Visibility.Hidden;
            if (place == 6) rectangle_6.Visibility = Visibility.Visible; else rectangle_6.Visibility = Visibility.Hidden;
            if (place == 7) rectangle_7.Visibility = Visibility.Visible; else rectangle_7.Visibility = Visibility.Hidden;
            if (place == 8) rectangle_8.Visibility = Visibility.Visible; else rectangle_8.Visibility = Visibility.Hidden;
            if (place == 9) rectangle_9.Visibility = Visibility.Visible; else rectangle_9.Visibility = Visibility.Hidden;
            if (place == 10) rectangle_0.Visibility = Visibility.Visible; else rectangle_0.Visibility = Visibility.Hidden;
        }


        protected override void OnManipulationStarted(ManipulationStartedEventArgs e)
        { 
            StickStartPoint = e.ManipulationOrigin;
            StickCurrentPoint = e.ManipulationOrigin;
            StickDown = true;

        }

        protected override void OnManipulationDelta(ManipulationDeltaEventArgs e)
        {
            StickCurrentPoint = e.ManipulationOrigin;
            StickDeltaPoint.X = StickCurrentPoint.X - StickStartPoint.X;
            StickDeltaPoint.Y = StickCurrentPoint.Y - StickStartPoint.Y;
            if (StickDeltaPoint.X > 50)
            {
                right = true;
                KeyboardOutput.performKeyDown(68);
            }
            else
            {
                right = false;
                KeyboardOutput.performKeyRelease(68);
            }
            if (StickDeltaPoint.X < -50)
            {
                left = true;
                KeyboardOutput.performKeyDown(65);
            }
            else
            {
                left = false;
                KeyboardOutput.performKeyRelease(65);
            }
            if (StickDeltaPoint.Y > 50)
            {
                down = true;
                KeyboardOutput.performKeyDown(83);
            }
            else
            {
                down = false;
                KeyboardOutput.performKeyRelease(83);
            }
            if (StickDeltaPoint.Y < -50)
            {
                up = true;
                KeyboardOutput.performKeyDown(87);
            }
            else
            {
                up = false;
                KeyboardOutput.performKeyRelease(87);
            }
        }

        protected override void OnManipulationCompleted(ManipulationCompletedEventArgs e)
        {
            if (right)
            {
                KeyboardOutput.performKeyRelease(68);
            }
            if (left)
            {
                KeyboardOutput.performKeyRelease(65);
            }
            if (up)
            {
                KeyboardOutput.performKeyRelease(87);
            }
            if (down) {
                KeyboardOutput.performKeyRelease(83);
            }
            StickDown = up = down = left = right = false;
            StickDeltaPoint.X = 0;
            StickDeltaPoint.Y = 0;
        }


        private void up_TouchDown(object sender, EventArgs e)
        {
            KeyboardOutput.performKeyDown(38);
        }
        private void up_TouchUp(object sender, EventArgs e)
        {
            KeyboardOutput.performKeyRelease(38);
        }
        private void down_TouchDown(object sender, EventArgs e)
        {
            KeyboardOutput.performKeyDown(40);
        }
        private void down_TouchUp(object sender, EventArgs e)
        {
            KeyboardOutput.performKeyRelease(40);
        }
        private void left_TouchDown(object sender, EventArgs e)
        {
            KeyboardOutput.performKeyDown(37);
        }
        private void left_TouchUp(object sender, EventArgs e)
        {
            KeyboardOutput.performKeyRelease(37);
        }
        private void right_TouchDown(object sender, EventArgs e)
        {
            KeyboardOutput.performKeyDown(39);
        }
        private void right_TouchUp(object sender, EventArgs e)
        {
            KeyboardOutput.performKeyRelease(39);
        }

        private void esc_TouchDown(object sender, EventArgs e)
        {
            KeyboardOutput.performKeyDown(27);
        }
        private void esc_TouchUp(object sender, EventArgs e)
        {
            KeyboardOutput.performKeyRelease(27);
        }
        private void F12_TouchDown(object sender, EventArgs e)
        {
            KeyboardOutput.performKeyDown(123);
        }
        private void F12_TouchUp(object sender, EventArgs e)
        {
            KeyboardOutput.performKeyRelease(123);
        }
        private void enter_TouchDown(object sender, EventArgs e)
        {
            KeyboardOutput.performKeyDown(13);
        }
        private void enter_TouchUp(object sender, EventArgs e)
        {
            KeyboardOutput.performKeyRelease(13);
        }
        private void end_TouchDown(object sender, EventArgs e)
        {
            KeyboardOutput.performKeyDown(35);
        }
        private void end_TouchUp(object sender, EventArgs e)
        {
            KeyboardOutput.performKeyRelease(35);
        }
        private void ctrl_TouchDown(object sender, EventArgs e)
        {
            KeyboardOutput.performKeyDown(162);
        }
        private void ctrl_TouchUp(object sender, EventArgs e)
        {
            KeyboardOutput.performKeyRelease(162);
        }


        private void buttonKeyboard_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(@"C:\Program Files\Common Files\microsoft shared\ink\TabTip.exe");
        }

        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
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
