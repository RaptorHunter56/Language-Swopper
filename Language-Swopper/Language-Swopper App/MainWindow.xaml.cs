using Microsoft.CSharp;
using Microsoft.Win32;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
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
        #region KeyBindings
        private void OpenCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MenuOpen();//Implementation of open file
        }
        private void SaveCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MenuSave();//Implementation of saveAs
        }
        #endregion

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

        private static int count = 0;
        public static int TimeProgress(int amount = 1)
        {
            if (amount == 0) amount = 1;
            count += ((100 / 12) / amount);
            if (count > 100) count = 100;
            return count;
        }

        public MainWindow(BackgroundWorker sender)
        {
            InitializeComponent();
            sender.ReportProgress(TimeProgress());
            string startlang = "";
            sender.ReportProgress(TimeProgress());
            using (var _context = new FileContext())
            {
                if (_context.Folders.Count() <= 0) { UpdateDatabase(); }
                sender.ReportProgress(TimeProgress());
                foreach (var folder in _context.Folders)
                {
                    sender.ReportProgress(TimeProgress(_context.Folders.Count()));
                    if (folder.Name != "Base" && startlang == "")
                    {
                        startlang = $"{folder.Name}Dictionary";
                    }
                    sender.ReportProgress(TimeProgress(_context.Folders.Count()));
                    Dictionary<string, ColorType> temp = new Dictionary<string, ColorType>();
                    sender.ReportProgress(TimeProgress(_context.Folders.Count()));
                    foreach (var color in _context.Highlights.Where(h => h.FolderId == folder.FolderID))
                    {
                        if (!temp.Keys.Contains(color.Text))
                        {
                            temp.Add(color.Text, new ColorType() { Color = Color.FromArgb(byte.Parse(color.Color.Split('|')[0]), byte.Parse(color.Color.Split('|')[1]), byte.Parse(color.Color.Split('|')[2]), byte.Parse(color.Color.Split('|')[3])), Types = color.Type});
                        }
                    }
                    Dictionarys.Add($"{folder.Name}Dictionary", temp);
                    sender.ReportProgress(TimeProgress(_context.Folders.Count()));
                }
            }
            sender.ReportProgress(TimeProgress());

            MinimizeButton.Click += (s, e) => WindowState = WindowState.Minimized;
            sender.ReportProgress(TimeProgress());
            MaximizeButton.Click += (s, e) => WindowState = WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
            CloseButton.Click += (s, e) => Close();
            sender.ReportProgress(TimeProgress());
            SourceInitialized += (s, e) =>
            {
                WindowCompositionTarget = PresentationSource.FromVisual(this).CompositionTarget;
                HwndSource.FromHwnd(new WindowInteropHelper(this).Handle).AddHook(WindowProc);
            };
            sender.ReportProgress(TimeProgress());
            //MainTextControl.Dictionary = CSharpDictionary;
            MainMultiTabControl.AddTabButtonClicked += PlussClick;
            //MainMenuControl.LanguageUpdated += LanguageUpdated;
            MainMenuControl.MenuOpenClicked += MenuOpen;
            sender.ReportProgress(TimeProgress());
            MainMenuControl.MenuSaveClicked += MenuSave;
            MainMenuControl.MenuRefreshLanguageClicked += MenuRefreshLanguage;
            sender.ReportProgress(TimeProgress());
            MainMultiTabControl.SamName.GetTextControl.Dictionary = Dictionarys[startlang];
            MainMultiTabControl.SamName.GetTextControl.LsLanguage = startlang;
            sender.ReportProgress(TimeProgress());
            MainMenuControl.languageChangedClicked += LanguageChangedClicked;
            MainMenuControl.Split_Clicked += MenuSplit;
            sender.ReportProgress(100);
        }

        public void LanguageChangedClicked(string len)
        {
            MainMultiTabControl.ChangeLanguage(len);
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
            testbutton.IsEnabled = false;
            testbutton.Background = new SolidColorBrush(new Color() { R = 100, G = 100, B = 100});//221
            using (Controler controler = new Controler())
            {
                controler._In = MainMultiTabControl.MainSplitTextControl.Left.LsLanguage.TrimEnd(']').TrimStart('[');
                controler._Out = MainMultiTabControl.MainSplitTextControl.Right.LsLanguage.TrimEnd(']').TrimStart('[');
                MainMultiTabControl.MainSplitTextControl.Right.MainRichTextBox.Document.Blocks.Clear();
                foreach (var item in MainMultiTabControl.TopPanel.Children.OfType<TabButtonControl>())
                {
                    if (item.Open && item.IsSplit)
                    {
                        //Fix Tabs
                        //DOM FIX ME
                        //MainMultiTabControl.MainSplitTextControl.Left.TestingBackground = new SolidColorBrush(Colors.Blue);
                        TextRange textRange = new TextRange(((SplitTextControl)MainMultiTabControl.GroopGrid.Children[1]).Left.MainRichTextBox.Document.ContentStart, ((SplitTextControl)MainMultiTabControl.GroopGrid.Children[1]).Left.MainRichTextBox.Document.ContentEnd);
                        string[] vs = textRange.Text.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                        ((SplitTextControl)MainMultiTabControl.GroopGrid.Children[1]).Right.MainRichTextBox.AppendText(controler.Lines(vs));
                        ((SplitTextControl)MainMultiTabControl.GroopGrid.Children[1]).Left.MainRichTextBox.AppendText("7777");
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
            ///
            testbutton.Background = new SolidColorBrush(new Color() { R = 221, G = 221, B = 221 });
            testbutton.IsEnabled = true;
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {

        }
    }
}
