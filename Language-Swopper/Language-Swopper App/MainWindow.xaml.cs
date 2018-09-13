using Microsoft.CSharp;
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
        CompositionTarget WindowCompositionTarget { get; set; }

        double CachedMinWidth { get; set; }

        double CachedMinHeight { get; set; }

        POINT CachedMinTrackSize { get; set; }

        IntPtr WindowProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            switch (msg)
            {
                case 0x0024:
                    MINMAXINFO mmi = (MINMAXINFO)Marshal.PtrToStructure(lParam, typeof(MINMAXINFO));
                    IntPtr monitor = MonitorFromWindow(hwnd, 0x00000002 /*MONITOR_DEFAULTTONEAREST*/);
                    if (monitor != IntPtr.Zero)
                    {
                        MONITORINFO monitorInfo = new MONITORINFO { };
                        GetMonitorInfo(monitor, monitorInfo);
                        RECT rcWorkArea = monitorInfo.rcWork;
                        RECT rcMonitorArea = monitorInfo.rcMonitor;
                        mmi.ptMaxPosition.x = Math.Abs(rcWorkArea.left - rcMonitorArea.left);
                        mmi.ptMaxPosition.y = Math.Abs(rcWorkArea.top - rcMonitorArea.top);
                        mmi.ptMaxSize.x = Math.Abs(rcWorkArea.right - rcWorkArea.left);
                        mmi.ptMaxSize.y = Math.Abs(rcWorkArea.bottom - rcWorkArea.top);
                        if (!CachedMinTrackSize.Equals(mmi.ptMinTrackSize) || CachedMinHeight != MinHeight && CachedMinWidth != MinWidth)
                        {
                            mmi.ptMinTrackSize.x = (int)((CachedMinWidth = MinWidth) * WindowCompositionTarget.TransformToDevice.M11);
                            mmi.ptMinTrackSize.y = (int)((CachedMinHeight = MinHeight) * WindowCompositionTarget.TransformToDevice.M22);
                            CachedMinTrackSize = mmi.ptMinTrackSize;
                        }
                    }
                    Marshal.StructureToPtr(mmi, lParam, true);
                    handled = true;
                    break;
            }
            return IntPtr.Zero;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int x;
            public int y;
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
            public RECT rcMonitor = new RECT { };
            public RECT rcWork = new RECT { };
            public int dwFlags = 0;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 0)]
        public struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }

        [DllImport("user32")]
        internal static extern bool GetMonitorInfo(IntPtr hMonitor, MONITORINFO lpmi);

        [DllImport("User32")]
        internal static extern IntPtr MonitorFromWindow(IntPtr handle, int flags);

        #endregion

        public struct ColorType
        {
            public Color Color;
            public Tables.Highlight.Types Types;
        }
        Dictionary<string, Dictionary<string, ColorType>> Dictionarys = new Dictionary<string, Dictionary<string, ColorType>>();

        public MainWindow()
        {
            InitializeComponent();
            string startlang = "";
            using (var _context = new FileContext())
            {
                foreach (var folder in _context.Folders)
                {
                    if (folder.Name != "Base" && startlang == "")
                    {
                        startlang = $"{folder.Name}Dictionary";
                    }
                    Dictionary<string, ColorType> temp = new Dictionary<string, ColorType>();
                    foreach (var color in _context.Highlights.Where(h => h.FolderId == folder.FolderID))
                    {
                        temp.Add(color.Text, new ColorType() { Color = Color.FromArgb(byte.Parse(color.Color.Split('|')[0]), byte.Parse(color.Color.Split('|')[1]), byte.Parse(color.Color.Split('|')[2]), byte.Parse(color.Color.Split('|')[3])), Types = color.Type});
                    }
                    Dictionarys.Add($"{folder.Name}Dictionary", temp);
                }
            }

            MinimizeButton.Click += (s, e) => WindowState = WindowState.Minimized;
            MaximizeButton.Click += (s, e) => WindowState = WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
            CloseButton.Click += (s, e) => Close();
            SourceInitialized += (s, e) =>
            {
                WindowCompositionTarget = PresentationSource.FromVisual(this).CompositionTarget;
                HwndSource.FromHwnd(new WindowInteropHelper(this).Handle).AddHook(WindowProc);
            };
            //MainTextControl.Dictionary = CSharpDictionary;
            MainMultiTabControl.AddTabButtonClicked += PlussClick;
            //MainMenuControl.LanguageUpdated += LanguageUpdated;
            MainMenuControl.MenuOpenClicked += MenuOpen;
            MainMenuControl.MenuSaveClicked += MenuSave;
            MainMenuControl.MenuRefreshLanguageClicked += MenuRefreshLanguage;
            MainMultiTabControl.SamName.GetTextControl.Dictionary = Dictionarys[startlang];
            //MainMenuControl.languageChangedClicked += LanguageChangedClicked;
            MainMenuControl.Split_Clicked += MenuSplit;
        }

        private void MenuSplit()
        {
            foreach (var item in MainMultiTabControl.TopPanel.Children.OfType<TabButtonControl>())
            {

                if (item.Open)
                {
                    item.IsSplit = !item.IsSplit;
                }
            }
        }



        #region Menu
        public void MenuOpen()
        {
            MainMultiTabControl.TabAdd(MainMenuControl.language, Dictionarys[$"{MainMenuControl.language}Dictionary"]);
            ///switch (MainMenuControl.language)
            ///{
            ///    case "C#":
            ///        MainMultiTabControl.TabAdd(MainMenuControl.language, CSharpDictionary);
            ///        break;
            ///    case "Visual Basic":
            ///        MainMultiTabControl.TabAdd(MainMenuControl.language, VisualBasicLDictionary);
            ///        break;
            ///    case "Python":
            ///        MainMultiTabControl.TabAdd(MainMenuControl.language, PythodDictionary);
            ///        break;
            ///    case "MySql":
            ///        MainMultiTabControl.TabAdd(MainMenuControl.language, MySQLDictionary);
            ///        break;
            ///    case "Sql":
            ///        break;
            ///    default:
            ///        break;
            ///}
        }
        public void MenuSave()
        {
            foreach (var item in MainMultiTabControl.TopPanel.Children.OfType<TabButtonControl>())
            {
                if (item.Open == true)
                {
                    File.WriteAllText(
                        item.Document,
                        new TextRange(
                        item.GetTextControl.MainRichTextBox.Document.ContentStart,
                        item.GetTextControl.MainRichTextBox.Document.ContentEnd).Text);
                }
            }
            //if (MainMenuControl.Document != null)
            //{
            //    //File.WriteAllText(
            //        //MainMenuControl.Document, 
            //        //new TextRange(
            //            //MainTextControl.MainRichTextBox.Document.ContentStart,
            //            //MainTextControl.MainRichTextBox.Document.ContentEnd).Text);
            //}
        }
        public void MenuRefreshLanguage()
        {
            UpdateDatabase();
        }
        private void PlussClick(object sender, EventArgs e)
        {
            MenuOpen();
        }

        public void LanguageUpdated()
        {
            //switch (MainMenuControl.GetLanguage)
            //{
            //    case "C#":
            //        //MainTextControl.Dictionary = CSharpDictionary;
            //        break;
            //    case "Visual Basic":
            //        break;
            //    case "Python":
            //        //MainTextControl.Dictionary = PythodDictionary;
            //        break;
            //    case "MySql":
            //        break;
            //    case "Sql":
            //        break;
            //    default:
            //        break;
            //}
        }
        #endregion

        private void TestButton_Click(object sender, RoutedEventArgs e)
        {
            using (Controler controler = new Controler())
            {
                string[] vs;
                MainMultiTabControl.MainSplitTextControl.Right.MainRichTextBox.Document.Blocks.Clear();
                foreach (var item in MainMultiTabControl.TopPanel.Children.OfType<TabButtonControl>())
                {
                    if (item.Open && item.IsSplit)
                    {
                        TextRange textRange = new TextRange(MainMultiTabControl.MainSplitTextControl.Left.MainRichTextBox.Document.ContentStart, MainMultiTabControl.MainSplitTextControl.Left.MainRichTextBox.Document.ContentEnd);
                        vs = textRange.Text.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (string singleline in vs)
                        {
                            MainMultiTabControl.MainSplitTextControl.Right.MainRichTextBox.AppendText(controler.line(singleline));
                        }
                    }
                }
            }
            ///DirectoryInfo d = new DirectoryInfo(@"...\...\Swopper\Python");
            ///List<string> dList = new List<string>();
            ///foreach (var file in d.GetFiles("*.cs"))
            ///{
            ///    dList.Add($@"{file.Directory.ToString()}\{file.Name}");
            ///}
            ///d = new DirectoryInfo(@"...\...\Swopper\Base");
            ///foreach (var file in d.GetFiles("*.cs"))
            ///{
            ///    dList.Add($@"{file.Directory.ToString()}\{file.Name}");
            ///}
            ///
            ///Dictionary<string, string> providerOptions = new Dictionary<string, string>
            ///{
            ///    {"CompilerVersion", "v3.5"}
            ///};
            ///CSharpCodeProvider provider = new CSharpCodeProvider(providerOptions);
            ///
            ///CompilerParameters compilerParams = new CompilerParameters
            ///{
            ///    GenerateInMemory = true,
            ///    GenerateExecutable = false
            ///};
            ///
            ///CompilerResults results = provider.CompileAssemblyFromSource(compilerParams, source);
            ///CompilerResults results = provider.CompileAssemblyFromFile(compilerParams, dList.ToArray());
            ///
            ///if (results.Errors.Count != 0)
            ///    throw new Exception("Mission failed!");
            ///
            ///object o = results.CompiledAssembly.CreateInstance("LswString.Equals");
            ///MethodInfo mc = o.GetType().GetMethod("Read");
            ///var returnValue = mc.Invoke(o, new object[] { "Name = 'some\"'" });
            ///var returnValue = mc.Invoke(o, new object[] {
            ///    new TextRange(
            ///        MainTextControl.MainRichTextBox.Document.ContentStart,
            ///        MainTextControl.MainRichTextBox.Document.ContentEnd).Text});
            ///mc = o.GetType().GetMethod("Print");
            ///returnValue = mc.Invoke(o, new object[] { returnValue });
            ///MainTextControl.MainRichTextBox.Document.Blocks.Clear();
            ///MainTextControl.MainRichTextBox.AppendText(returnValue.ToString());
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {

        }
    }
}
