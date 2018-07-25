using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
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
using System.Threading;

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
        //InputSimulator sim = new InputSimulator();
        [DllImport("user32.dll", SetLastError = true)]
        static extern void keybd_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);
        public const int KEYEVENTF_EXTENDEDKEY = 0x0001; //Key down flag
        public const int KEYEVENTF_KEYUP = 0x0002; //Key up flag
        public const int VK_0 = 0x30;
        public const int VK_1 = 0x31;
        public const int VK_2 = 0x32;
        public const int VK_3 = 0x33;
        public const int VK_4 = 0x34;
        public const int VK_5 = 0x35;
        public const int VK_6 = 0x36;
        public const int VK_7 = 0x37;
        public const int VK_8 = 0x38;
        public const int VK_9 = 0x39;
        

        int place = 1;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // install system-wide keyboard hook
            _hook = new KeyboardHook();
            _hook.KeyDown += new KeyboardHook.HookEventHandler(OnHookKeyDown);
            setvis();

        }
        // keyboard hook handler
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
                        keybd_event(VK_1, 0, KEYEVENTF_EXTENDEDKEY, 0);
                        Thread.Sleep(50);
                        keybd_event(VK_1, 0, KEYEVENTF_KEYUP, 0);
                        break;
                    case 2:
                        keybd_event(VK_2, 0, KEYEVENTF_EXTENDEDKEY, 0);
                        Thread.Sleep(50);
                        keybd_event(VK_2, 0, KEYEVENTF_KEYUP, 0);
                        break;
                    case 3:
                        keybd_event(VK_3, 0, KEYEVENTF_EXTENDEDKEY, 0);
                        Thread.Sleep(50);
                        keybd_event(VK_3, 0, KEYEVENTF_KEYUP, 0);
                        break;
                    case 4:
                        keybd_event(VK_4, 0, KEYEVENTF_EXTENDEDKEY, 0);
                        Thread.Sleep(50);
                        keybd_event(VK_4, 0, KEYEVENTF_KEYUP, 0);
                        break;
                    case 5:
                        keybd_event(VK_5, 0, KEYEVENTF_EXTENDEDKEY, 0);
                        Thread.Sleep(50);
                        keybd_event(VK_5, 0, KEYEVENTF_KEYUP, 0);
                        break;
                    case 6:
                        keybd_event(VK_6, 0, KEYEVENTF_EXTENDEDKEY, 0);
                        Thread.Sleep(50);
                        keybd_event(VK_6, 0, KEYEVENTF_KEYUP, 0);
                        break;
                    case 7:
                        keybd_event(VK_7, 0, KEYEVENTF_EXTENDEDKEY, 0);
                        Thread.Sleep(50);
                        keybd_event(VK_7, 0, KEYEVENTF_KEYUP, 0);
                        break;
                    case 8:
                        keybd_event(VK_8, 0, KEYEVENTF_EXTENDEDKEY, 0);
                        Thread.Sleep(50);
                        keybd_event(VK_8, 0, KEYEVENTF_KEYUP, 0);
                        break;
                    case 9:
                        keybd_event(VK_9, 0, KEYEVENTF_EXTENDEDKEY, 0);
                        Thread.Sleep(50);
                        keybd_event(VK_9, 0, KEYEVENTF_KEYUP, 0);
                        break;
                    case 10:
                        keybd_event(VK_0, 0, KEYEVENTF_EXTENDEDKEY, 0);
                        Thread.Sleep(50);
                        keybd_event(VK_0, 0, KEYEVENTF_KEYUP, 0);
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

        private void buttonKeyboard_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(@"C:\Program Files\Common Files\microsoft shared\ink\TabTip.exe");
        }

        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }
    }

    
}
