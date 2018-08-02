﻿using System;
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
        bool stickDown = false, left = false, right = false, up = false, down = false;
        double distance = 0, angle = 0;

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
            rectangle_1.Opacity = 1;
            rectangle_2.Opacity = 1;
            rectangle_3.Opacity = 1;
            rectangle_4.Opacity = 1;
            rectangle_5.Opacity = 1;
            rectangle_6.Opacity = 1;
            rectangle_7.Opacity = 1;
            rectangle_8.Opacity = 1;
            rectangle_9.Opacity = 1;
            rectangle_0.Opacity = 1;
            rectangle_1.Fill = System.Windows.Media.Brushes.Transparent;
            rectangle_2.Fill = System.Windows.Media.Brushes.Transparent;
            rectangle_3.Fill = System.Windows.Media.Brushes.Transparent;
            rectangle_4.Fill = System.Windows.Media.Brushes.Transparent;
            rectangle_5.Fill = System.Windows.Media.Brushes.Transparent;
            rectangle_6.Fill = System.Windows.Media.Brushes.Transparent;
            rectangle_7.Fill = System.Windows.Media.Brushes.Transparent;
            rectangle_8.Fill = System.Windows.Media.Brushes.Transparent;
            rectangle_9.Fill = System.Windows.Media.Brushes.Transparent;
            rectangle_0.Fill = System.Windows.Media.Brushes.Transparent;
        }
        void setvis(bool touch)
        {
            if (touch)
            {
                rectangle_1.Visibility = Visibility.Visible;
                rectangle_2.Visibility = Visibility.Visible;
                rectangle_3.Visibility = Visibility.Visible;
                rectangle_4.Visibility = Visibility.Visible;
                rectangle_5.Visibility = Visibility.Visible;
                rectangle_6.Visibility = Visibility.Visible;
                rectangle_7.Visibility = Visibility.Visible;
                rectangle_8.Visibility = Visibility.Visible;
                rectangle_9.Visibility = Visibility.Visible;
                rectangle_0.Visibility = Visibility.Visible;
                rectangle_1.Opacity = 0.3;
                rectangle_2.Opacity = 0.3;
                rectangle_3.Opacity = 0.3;
                rectangle_4.Opacity = 0.3;
                rectangle_5.Opacity = 0.3;
                rectangle_6.Opacity = 0.3;
                rectangle_7.Opacity = 0.3;
                rectangle_8.Opacity = 0.3;
                rectangle_9.Opacity = 0.3;
                rectangle_0.Opacity = 0.3;
                rectangle_1.Fill = (SolidColorBrush)new BrushConverter().ConvertFromString("#FFF4F4F5");
                rectangle_2.Fill = (SolidColorBrush)new BrushConverter().ConvertFromString("#FFF4F4F5");
                rectangle_3.Fill = (SolidColorBrush)new BrushConverter().ConvertFromString("#FFF4F4F5");
                rectangle_4.Fill = (SolidColorBrush)new BrushConverter().ConvertFromString("#FFF4F4F5");
                rectangle_5.Fill = (SolidColorBrush)new BrushConverter().ConvertFromString("#FFF4F4F5");
                rectangle_6.Fill = (SolidColorBrush)new BrushConverter().ConvertFromString("#FFF4F4F5");
                rectangle_7.Fill = (SolidColorBrush)new BrushConverter().ConvertFromString("#FFF4F4F5");
                rectangle_8.Fill = (SolidColorBrush)new BrushConverter().ConvertFromString("#FFF4F4F5");
                rectangle_9.Fill = (SolidColorBrush)new BrushConverter().ConvertFromString("#FFF4F4F5");
                rectangle_0.Fill = (SolidColorBrush)new BrushConverter().ConvertFromString("#FFF4F4F5");
            }
        }
        /*      
        protected override void OnManipulationStarted(ManipulationStartedEventArgs e)
        { 
            StickStartPoint = e.ManipulationOrigin;
            StickCurrentPoint = e.ManipulationOrigin;
            stickDown = true;

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
            stickDown = up = down = left = right = false;
            StickDeltaPoint.X = 0;
            StickDeltaPoint.Y = 0;
        }
        */

        protected override void OnManipulationStarted(ManipulationStartedEventArgs e)
        {
            StickStartPoint = e.ManipulationOrigin;
            StickCurrentPoint = e.ManipulationOrigin;
            distance = 0;
            angle = 0;
            stickDown = true;
            setvis(true);
        }

        protected override void OnManipulationDelta(ManipulationDeltaEventArgs e)
        {
            StickCurrentPoint = e.ManipulationOrigin;
            distance = Point.Subtract(StickStartPoint, StickCurrentPoint).Length;
            angle = (Math.Atan2(StickCurrentPoint.Y - StickStartPoint.Y, StickCurrentPoint.X - StickStartPoint.X) * 180.0 / Math.PI) + 180;
            int quadrant = 0;
            if (angle < 22.5 || angle >= 337.5)
            {
                quadrant = 1;
            }
            if (angle >= 22.5 && angle < 67.5)
            {
                quadrant = 2;
            }
            if (angle >= 67.5 && angle < 112.5)
            {
                quadrant = 3;
            }
            if (angle >= 112.5 && angle < 157.5)
            {
                quadrant = 4;
            }
            if (angle >= 157.5 && angle < 202.5)
            {
                quadrant = 5;
            }
            if (angle >= 202.5 && angle < 247.5) {
                quadrant = 6;
            }
            if (angle >= 247.5 && angle < 292.5)
            {
                quadrant = 7;
            }
            if (angle >= 292.5 && angle < 337.5)
            {
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
            stickDown = false;
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

        private void one_TouchDown(object sender, EventArgs e)
        {
            KeyboardOutput.performKeyDown(49);
        }
        private void one_TouchUp(object sender, EventArgs e)
        {
            KeyboardOutput.performKeyRelease(49);
        }
        private void two_TouchDown(object sender, EventArgs e)
        {
            KeyboardOutput.performKeyDown(50);
        }
        private void two_TouchUp(object sender, EventArgs e)
        {
            KeyboardOutput.performKeyRelease(50);
        }
        private void three_TouchDown(object sender, EventArgs e)
        {
            KeyboardOutput.performKeyDown(51);
        }
        private void three_TouchUp(object sender, EventArgs e)
        {
            KeyboardOutput.performKeyRelease(51);
        }
        private void four_TouchDown(object sender, EventArgs e)
        {
            KeyboardOutput.performKeyDown(52);
        }
        private void four_TouchUp(object sender, EventArgs e)
        {
            KeyboardOutput.performKeyRelease(52);
        }
        private void five_TouchDown(object sender, EventArgs e)
        {
            KeyboardOutput.performKeyDown(53);
        }
        private void five_TouchUp(object sender, EventArgs e)
        {
            KeyboardOutput.performKeyRelease(53);
        }
        private void six_TouchDown(object sender, EventArgs e)
        {
            KeyboardOutput.performKeyDown(54);
        }
        private void six_TouchUp(object sender, EventArgs e)
        {
            KeyboardOutput.performKeyRelease(54);
        }
        private void seven_TouchDown(object sender, EventArgs e)
        {
            KeyboardOutput.performKeyDown(55);
        }
        private void seven_TouchUp(object sender, EventArgs e)
        {
            KeyboardOutput.performKeyRelease(55);
        }
        private void eight_TouchDown(object sender, EventArgs e)
        {
            KeyboardOutput.performKeyDown(56);
        }
        private void eight_TouchUp(object sender, EventArgs e)
        {
            KeyboardOutput.performKeyRelease(56);
        }
        private void nine_TouchDown(object sender, EventArgs e)
        {
            KeyboardOutput.performKeyDown(57);
        }
        private void nine_TouchUp(object sender, EventArgs e)
        {
            KeyboardOutput.performKeyRelease(57);
        }
        private void zero_TouchDown(object sender, EventArgs e)
        {
            KeyboardOutput.performKeyDown(48);
        }
        private void zero_TouchUp(object sender, EventArgs e)
        {
            KeyboardOutput.performKeyRelease(48);
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
