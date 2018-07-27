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

        //public const int KEYEVENTF_EXTENDEDKEY = 0x0001; //Key down flag
        //public const int KEYEVENTF_KEYUP = 0x0002; //Key up flag

        /// <summary>
        /// http://msdn.microsoft.com/en-us/library/windows/desktop/ms646270(v=vs.85).aspx
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        internal struct INPUT
        {
            public uint Type;
            public MOUSEKEYBDHARDWAREINPUT Data;
        }
        internal const uint INPUT_MOUSE = 0, INPUT_KEYBOARD = 1, INPUT_HARDWARE = 2;
        /// <summary>
        /// http://social.msdn.microsoft.com/Forums/en/csharplanguage/thread/f0e82d6e-4999-4d22-b3d3-32b25f61fb2a
        /// </summary>
        [StructLayout(LayoutKind.Explicit)]
        internal struct MOUSEKEYBDHARDWAREINPUT
        {
            [FieldOffset(0)]
            public HARDWAREINPUT Hardware;
            [FieldOffset(0)]
            public KEYBDINPUT Keyboard;
            [FieldOffset(0)]
            public MOUSEINPUT Mouse;
        }

        /// <summary>
        /// http://msdn.microsoft.com/en-us/library/windows/desktop/ms646310(v=vs.85).aspx
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        internal struct HARDWAREINPUT
        {
            public uint Msg;
            public ushort ParamL;
            public ushort ParamH;
        }

        /// <summary>
        /// http://msdn.microsoft.com/en-us/library/windows/desktop/ms646310(v=vs.85).aspx
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        internal struct KEYBDINPUT
        {
            public ushort Vk;
            public ushort Scan;
            public uint Flags;
            public uint Time;
            public IntPtr ExtraInfo;
        }

        /// <summary>
        /// http://social.msdn.microsoft.com/forums/en-US/netfxbcl/thread/2abc6be8-c593-4686-93d2-89785232dacd
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        internal struct MOUSEINPUT
        {
            public int X;
            public int Y;
            public uint MouseData;
            public uint Flags;
            public uint Time;
            public IntPtr ExtraInfo;
        }

        internal const uint KEYEVENTF_EXTENDEDKEY = 1, KEYEVENTF_KEYUP = 2, MOUSEEVENTF_WHEEL = 0x0800,
            MOUSEEVENTF_HWHEEL = 0x1000, MOUSEEVENTF_MIDDLEWDOWN = 0x0020, MOUSEEVENTF_MIDDLEWUP = 0x0040,
            KEYEVENTF_SCANCODE = 0x0008, MAPVK_VK_TO_VSC = 0, KEYEVENTF_UNICODE = 0x0004;

        internal const uint VK_PAUSE = 0x13, VK_LEFT = 0x25, VK_UP = 0x26, VK_RIGHT = 0x27, VK_DOWN = 0x28,
    VK_PRIOR = 0x21, VK_NEXT = 0x22, VK_END = 0x23, VK_HOME = 0x24, VK_INSERT = 0x2D, VK_DELETE = 0x2E, VK_APPS = 0x5D,
    VK_DIVIDE = 0x6F, VK_NUMLOCK = 0x90, VK_RCONTROL = 0xA3, VK_RMENU = 0xA5, VK_BROWSER_HOME = 0xAC,
    VK_VOLUME_MUTE = 0xAD, VK_VOLUME_DOWN = 0xAE, VK_VOLUME_UP = 0xAF,
    VK_MEDIA_NEXT_TRACK = 0xB0, VK_MEDIA_PREV_TRACK = 0xB1, VK_MEDIA_STOP = 0xB2, VK_MEDIA_PLAY_PAUSE = 0xB3,
    VK_LAUNCH_MAIL = 0xB4, VK_LAUNCH_MEDIA_SELECT = 0xB5, VK_LAUNCH_APP1 = 0xB6, VK_LAUNCH_APP2 = 0xB7, EXTENDED_FLAG = 0x100;

        private static INPUT[] sendInputs = new INPUT[2];

        [DllImport("user32.dll", SetLastError = true)]
        private static extern uint SendInput(uint numberOfInputs, INPUT[] inputs, int sizeOfInputs);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern ushort MapVirtualKey(uint uCode, uint uMapType);

        public static void performKeyPress(ushort key)
        {
            lock ("keypress")
            {
                ushort scancode = scancodeFromVK(key);
                bool extended = (scancode & 0x100) != 0;
                uint curflags = extended ? KEYEVENTF_EXTENDEDKEY : 0;

                sendInputs[0].Type = INPUT_KEYBOARD;
                sendInputs[0].Data.Keyboard.ExtraInfo = IntPtr.Zero;
                sendInputs[0].Data.Keyboard.Flags = curflags;
                sendInputs[0].Data.Keyboard.Scan = scancode;
                //sendInputs[0].Data.Keyboard.Flags = 1;
                //sendInputs[0].Data.Keyboard.Scan = 0;
                sendInputs[0].Data.Keyboard.Time = 0;
                sendInputs[0].Data.Keyboard.Vk = key;
                uint result = SendInput(1, sendInputs, Marshal.SizeOf(sendInputs[0]));
            }
        }

        public static void performKeyRelease(ushort key)
        {
            lock ("keypress")
            {
                ushort scancode = scancodeFromVK(key);
                bool extended = (scancode & 0x100) != 0;
                uint curflags = extended ? KEYEVENTF_EXTENDEDKEY : 0;

                sendInputs[0].Type = INPUT_KEYBOARD;
                sendInputs[0].Data.Keyboard.ExtraInfo = IntPtr.Zero;
                sendInputs[0].Data.Keyboard.Flags = curflags | KEYEVENTF_KEYUP;
                sendInputs[0].Data.Keyboard.Scan = scancode;
                //sendInputs[0].Data.Keyboard.Flags = KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP;
                //sendInputs[0].Data.Keyboard.Scan = 0;
                sendInputs[0].Data.Keyboard.Time = 0;
                sendInputs[0].Data.Keyboard.Vk = key;
                uint result = SendInput(1, sendInputs, Marshal.SizeOf(sendInputs[0]));
            }
        }

        private static ushort scancodeFromVK(uint vkey)
        {
            ushort scancode = 0;
            if (vkey == VK_PAUSE)
            {
                // MapVirtualKey does not work with VK_PAUSE
                scancode = 0x45;
            }
            else
            {
                scancode = MapVirtualKey(vkey, MAPVK_VK_TO_VSC);
            }

            switch (vkey)
            {
                case VK_LEFT:
                case VK_UP:
                case VK_RIGHT:
                case VK_DOWN:
                case VK_PRIOR:
                case VK_NEXT:
                case VK_END:
                case VK_HOME:
                case VK_INSERT:
                case VK_DELETE:
                case VK_DIVIDE:
                case VK_NUMLOCK:
                case VK_RCONTROL:
                case VK_RMENU:
                case VK_VOLUME_MUTE:
                case VK_VOLUME_DOWN:
                case VK_VOLUME_UP:
                case VK_MEDIA_NEXT_TRACK:
                case VK_MEDIA_PREV_TRACK:
                case VK_LAUNCH_MEDIA_SELECT:
                case VK_BROWSER_HOME:
                case VK_LAUNCH_MAIL:
                case VK_LAUNCH_APP1:
                case VK_LAUNCH_APP2:
                case VK_APPS:
                    {
                        scancode |= (ushort)EXTENDED_FLAG; // set extended bit
                        break;
                    }
            }

            return scancode;
        }


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
                        performKeyPress(49);
                        Thread.Sleep(20);
                        performKeyRelease(49);
                        break;
                    case 2:
                        performKeyPress(50);
                        Thread.Sleep(20);
                        performKeyRelease(50);
                        break;
                    case 3:
                        performKeyPress(51);
                        Thread.Sleep(20);
                        performKeyRelease(51);
                        break;
                    case 4:
                        performKeyPress(52);
                        Thread.Sleep(20);
                        performKeyRelease(52);
                        break;
                    case 5:
                        performKeyPress(53);
                        Thread.Sleep(20);
                        performKeyRelease(53);
                        break;
                    case 6:
                        performKeyPress(54);
                        Thread.Sleep(20);
                        performKeyRelease(54);
                        break;
                    case 7:
                        performKeyPress(55);
                        Thread.Sleep(20);
                        performKeyRelease(55);
                        break;
                    case 8:
                        performKeyPress(56);
                        Thread.Sleep(20);
                        performKeyRelease(56);
                        break;
                    case 9:
                        performKeyPress(57);
                        Thread.Sleep(20);
                        performKeyRelease(57);
                        break;
                    case 10:
                        performKeyPress(48);
                        Thread.Sleep(20);
                        performKeyRelease(48);
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
