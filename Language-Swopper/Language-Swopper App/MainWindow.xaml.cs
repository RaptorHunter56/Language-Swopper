﻿using Microsoft.CSharp;
using Microsoft.Win32;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Language_Swopper_App
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Thing
        private static IntPtr WindowProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            switch (msg)
            {
                case 0x0024:
                    WmGetMinMaxInfo(hwnd, lParam);
                    handled = true;
                    break;
            }
            return (IntPtr)0;
        }

        private static void WmGetMinMaxInfo(IntPtr hwnd, IntPtr lParam)
        {
            MINMAXINFO mmi = (MINMAXINFO)Marshal.PtrToStructure(lParam, typeof(MINMAXINFO));
            int MONITOR_DEFAULTTONEAREST = 0x00000002;
            IntPtr monitor = MonitorFromWindow(hwnd, MONITOR_DEFAULTTONEAREST);
            if (monitor != IntPtr.Zero)
            {
                MONITORINFO monitorInfo = new MONITORINFO();
                GetMonitorInfo(monitor, monitorInfo);
                RECT rcWorkArea = monitorInfo.rcWork;
                RECT rcMonitorArea = monitorInfo.rcMonitor;
                mmi.ptMaxPosition.x = Math.Abs(rcWorkArea.left - rcMonitorArea.left);
                mmi.ptMaxPosition.y = Math.Abs(rcWorkArea.top - rcMonitorArea.top);
                mmi.ptMaxSize.x = Math.Abs(rcWorkArea.right - rcWorkArea.left);
                mmi.ptMaxSize.y = Math.Abs(rcWorkArea.bottom - rcWorkArea.top);
            }
            Marshal.StructureToPtr(mmi, lParam, true);
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            /// <summary>x coordinate of point.</summary>
            public int x;
            /// <summary>y coordinate of point.</summary>
            public int y;
            /// <summary>Construct a point of coordinates (x,y).</summary>
            public POINT(int x, int y)
            {
                this.x = x;
                this.y = y;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MINMAXINFO
        {
            public POINT ptReserved;
            public POINT ptMaxSize;
            public POINT ptMaxPosition;
            public POINT ptMinTrackSize;
            public POINT ptMaxTrackSize;
        };

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public class MONITORINFO
        {
            public int cbSize = Marshal.SizeOf(typeof(MONITORINFO));
            public RECT rcMonitor = new RECT();
            public RECT rcWork = new RECT();
            public int dwFlags = 0;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 0)]
        public struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
            public static readonly RECT Empty = new RECT();
            public int Width { get { return Math.Abs(right - left); } }
            public int Height { get { return bottom - top; } }
            public RECT(int left, int top, int right, int bottom)
            {
                this.left = left;
                this.top = top;
                this.right = right;
                this.bottom = bottom;
            }
            public RECT(RECT rcSrc)
            {
                left = rcSrc.left;
                top = rcSrc.top;
                right = rcSrc.right;
                bottom = rcSrc.bottom;
            }
            public bool IsEmpty { get { return left >= right || top >= bottom; } }
            public override string ToString()
            {
                if (this == Empty) { return "RECT {Empty}"; }
                return "RECT { left : " + left + " / top : " + top + " / right : " + right + " / bottom : " + bottom + " }";
            }
            public override bool Equals(object obj)
            {
                if (!(obj is Rect)) { return false; }
                return (this == (RECT)obj);
            }
            /// <summary>Return the HashCode for this struct (not garanteed to be unique)</summary>
            public override int GetHashCode() => left.GetHashCode() + top.GetHashCode() + right.GetHashCode() + bottom.GetHashCode();
            /// <summary> Determine if 2 RECT are equal (deep compare)</summary>
            public static bool operator ==(RECT rect1, RECT rect2) { return (rect1.left == rect2.left && rect1.top == rect2.top && rect1.right == rect2.right && rect1.bottom == rect2.bottom); }
            /// <summary> Determine if 2 RECT are different(deep compare)</summary>
            public static bool operator !=(RECT rect1, RECT rect2) { return !(rect1 == rect2); }
        }

        [DllImport("user32")]
        internal static extern bool GetMonitorInfo(IntPtr hMonitor, MONITORINFO lpmi);

        [DllImport("User32")]
        internal static extern IntPtr MonitorFromWindow(IntPtr handle, int flags);

        #endregion

        public MainWindow()
        {
            InitializeComponent();
            SourceInitialized += (s, e) =>
            {
                IntPtr handle = (new WindowInteropHelper(this)).Handle;
                HwndSource.FromHwnd(handle).AddHook(new HwndSourceHook(WindowProc));
            };
            MinimizeButton.Click += (s, c) => WindowState = WindowState.Minimized;
            MaximizeButton.Click += (s, c) => WindowState = WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
            CloseButton.Click += (s, c) => Close();
            MainTextControl.Dictionary = CSharpDictionary;
            MainMenuControl.LanguageUpdated += LanguageUpdated;
            MainMenuControl.MenuOpenClicked += MenuOpen;
            MainMenuControl.MenuSaveClicked += MenuSave;
            MainMenuControl.MenuRefreshLanguageClicked += MenuRefreshLanguage;
        }

        public Dictionary<string, Color> CSharpDictionary = new Dictionary<string, Color>()
        {
            { "using", new Color() { A = 255, R = 255, G = 255, B = 0 } },
            { "open", new Color() { A = 255, R = 255, G = 255, B = 0 } },
            { "push", new Color() { A = 255, R = 0, G = 255, B = 255 } },
            { "reload", new Color(){ A = 255, R = 255, G = 0, B = 255 } }
        };
        public Dictionary<string, Color> PythodDictionary = new Dictionary<string, Color>()
        {
            { "for", new Color() { A = 255, R = 255, G = 0, B = 0 } },
            { "open", new Color() { A = 255, R = 255, G = 0, B = 0 } },
            { "push", new Color() { A = 255, R = 0, G = 255, B = 0 } },
            { "reload", new Color(){ A = 255, R = 0, G = 0, B = 255 } }
        };


        #region Menu
        public void MenuOpen()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = MainMenuControl.languageFilter[MainMenuControl.GetLanguage];
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            if (openFileDialog.ShowDialog() == true)
            {
                MainMenuControl.Document = openFileDialog.FileName;
                MainTextControl.MainRichTextBox.Document.Blocks.Clear();
                MainTextControl.MainRichTextBox.AppendText(File.ReadAllText(openFileDialog.FileName));
            }
        }
        public void MenuSave()
        {
            if (MainMenuControl.Document != null)
            {
                File.WriteAllText(
                    MainMenuControl.Document, 
                    new TextRange(
                        MainTextControl.MainRichTextBox.Document.ContentStart,
                        MainTextControl.MainRichTextBox.Document.ContentEnd).Text);
            }
        }
        public void MenuRefreshLanguage()
        {
            UpdateDatabase();
        }
        public void LanguageUpdated()
        {
            switch (MainMenuControl.GetLanguage)
            {
                case "C#":
                    MainTextControl.Dictionary = CSharpDictionary;
                    break;
                case "Visual Basic":
                    break;
                case "Python":
                    MainTextControl.Dictionary = PythodDictionary;
                    break;
                case "MySql":
                    break;
                case "Sql":
                    break;
                default:
                    break;
            }
        }
        #endregion

        private void TestButton_Click(object sender, RoutedEventArgs e)
        {
            DirectoryInfo d = new DirectoryInfo(@"...\...\Swopper\Python");
            List<string> dList = new List<string>();
            foreach (var file in d.GetFiles("*.cs"))
            {
                dList.Add($@"{file.Directory.ToString()}\{file.Name}");
            }
            d = new DirectoryInfo(@"...\...\Swopper\Base");
            foreach (var file in d.GetFiles("*.cs"))
            {
                dList.Add($@"{file.Directory.ToString()}\{file.Name}");
            }

            Dictionary<string, string> providerOptions = new Dictionary<string, string>
            {
                {"CompilerVersion", "v3.5"}
            };
            CSharpCodeProvider provider = new CSharpCodeProvider(providerOptions);

            CompilerParameters compilerParams = new CompilerParameters
            {
                GenerateInMemory = true,
                GenerateExecutable = false
            };

            //CompilerResults results = provider.CompileAssemblyFromSource(compilerParams, source);
            CompilerResults results = provider.CompileAssemblyFromFile(compilerParams, dList.ToArray());

            if (results.Errors.Count != 0)
                throw new Exception("Mission failed!");

            object o = results.CompiledAssembly.CreateInstance("LswString.Equals");
            MethodInfo mc = o.GetType().GetMethod("Read");
            //var returnValue = mc.Invoke(o, new object[] { "Name = 'some\"'" });
            var returnValue = mc.Invoke(o, new object[] {
                new TextRange(
                    MainTextControl.MainRichTextBox.Document.ContentStart,
                    MainTextControl.MainRichTextBox.Document.ContentEnd).Text});
            mc = o.GetType().GetMethod("Print");
            returnValue = mc.Invoke(o, new object[] { returnValue });
            MainTextControl.MainRichTextBox.Document.Blocks.Clear();
            MainTextControl.MainRichTextBox.AppendText(returnValue.ToString());
        }
    }
}
