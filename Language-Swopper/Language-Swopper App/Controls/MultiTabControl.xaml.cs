using Language_Swopper_App.Controls;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Language_Swopper_App
{
    /// <summary>
    /// Interaction logic for MultiTabControl.xaml
    /// </summary>
    public partial class MultiTabControl : UserControl
    {
        //{
        //    get { return (TextControl)GetValue(textControlProperty); }
        //    set { SetValue(textControlProperty, value); }
        //}
        //public static readonly DependencyProperty textControlProperty = DependencyProperty.Register("textControl", typeof(TextControl), typeof(MultiTabControl), new PropertyMetadata(new TextControl()));

        public MultiTabControl()
        {
            InitializeComponent();
            //SamSam.ControlTabButtonClicked += new EventHandler(this.TabButtonControl_ControlClicked);
            SamName.ControlTabButtonClicked += new EventHandler(this.TabButtonControl_ControlClicked);
            Pluss.ControlTabButtonClicked += new EventHandler(this.TabButtonControl_ControlClicked2);
            //SamSam.CloseTabButtonClicked += new EventHandler(this.TabButtonClose_ControlClicked);
            SamName.CloseTabButtonClicked += new EventHandler(this.TabButtonClose_ControlClicked);

            TabMovementTimer.Tick += TabMovementTimer_Tick;
            TabMovementTimer.Interval = new TimeSpan(0, 0, 1);
            //SamSam.IsSplit = true;
            //SamSam.splitChanged += new TabButtonControl.SplitChanged(this.SplitChanged);
            SamName.splitChanged += new TabButtonControl.SplitChanged(this.SplitChanged);

            //SamSam.GetTextControl.LsLanguage = "CSharp";
            //SamSam.GetSplitTextControl.Left.LsLanguage = "CSharp";
            //SamSam.GetSplitTextControl.Right.LsLanguage = "CSharp";
            using (var _context = new FileContext())
            {
                foreach (var folder in _context.Folders)
                {
                    if (folder.Name != "Base")
                    {
                        SamName.GetTextControl.LsLanguage = $"{folder.Name}";
                        SamName.GetSplitTextControl.Left.LsLanguage = $"{folder.Name}";
                        SamName.GetSplitTextControl.Right.LsLanguage = $"{folder.Name}";
                        MainTextControl.LsLanguage = $"{folder.Name}";
                        MainSplitTextControl.Left.LsLanguage = $"{folder.Name}";
                        MainSplitTextControl.Right.LsLanguage = $"{folder.Name}";
                        break;
                    }
                }
            }
        }

        public void SplitChanged(object sender)
        {
            if (((TabButtonControl)sender).Open)
            {
                switch (((TabButtonControl)sender).IsSplit)
                {
                    case true:
                        GroopGrid.Children[0].Visibility = Visibility.Collapsed;
                        GroopGrid.Children[1].Visibility = Visibility.Visible;
                        ((TabButtonControl)sender).GetTextControl = GroopGrid.Children.OfType<TextControl>().FirstOrDefault();
                        ((TabButtonControl)sender).GetSplitTextControl = GroopGrid.Children.OfType<SplitTextControl>().FirstOrDefault();
                        Copy.CopyProperties(((TabButtonControl)sender).GetTextControl.MainRichTextBox, ((TabButtonControl)sender).GetSplitTextControl.Left.MainRichTextBox);
                        break;
                    case false:
                    default:
                        GroopGrid.Children[0].Visibility = Visibility.Visible;
                        GroopGrid.Children[1].Visibility = Visibility.Collapsed;
                        ((TabButtonControl)sender).GetTextControl = GroopGrid.Children.OfType<TextControl>().FirstOrDefault();
                        ((TabButtonControl)sender).GetSplitTextControl = GroopGrid.Children.OfType<SplitTextControl>().FirstOrDefault();
                        Copy.CopyProperties(((TabButtonControl)sender).GetSplitTextControl.Left.MainRichTextBox, ((TabButtonControl)sender).GetTextControl.MainRichTextBox);
                        break;
                }
            }
        }

        private void TabButtonClose_ControlClicked(object sender, EventArgs e)
        {
            if (((TabButtonControl)sender).Open && TopPanel.Children.Count > 2)
            {
                int rmin = 1;
                if ((TopPanel.Children.IndexOf((TabButtonControl)sender) - 1) < 0)
                    rmin = -1;
                TabButtonControl_ControlClicked(TopPanel.Children[TopPanel.Children.IndexOf((TabButtonControl)sender) - rmin], new EventArgs());
                ((TabButtonControl)TopPanel.Children[TopPanel.Children.IndexOf((TabButtonControl)sender) - rmin]).Open = true;
            }
            if (TopPanel.Children.Count == 2)
            {
                GroopGrid.Children.Remove(GroopGrid.Children.OfType<TextControl>().FirstOrDefault());
                GroopGrid.Children.Remove(GroopGrid.Children.OfType<SplitTextControl>().FirstOrDefault());
            }
            TopPanel.Children.Remove((TabButtonControl)sender);
        }

        public event EventHandler AddTabButtonClicked;
        private void TabButtonControl_ControlClicked2(object sender, EventArgs e)
        {
            if (AddTabButtonClicked != null)
                AddTabButtonClicked(this, EventArgs.Empty);
        }
        private void TabButtonControl_ControlClicked(object sender, EventArgs e)
        {
            #region Try 1
            TabButtonControl tab = (TabButtonControl)sender;
            foreach (var item in TopPanel.Children.OfType<TabButtonControl>())
            {

                if (item.Open)
                {
                    item.GetTextControl = GroopGrid.Children.OfType<TextControl>().FirstOrDefault();
                    item.GetSplitTextControl = GroopGrid.Children.OfType<SplitTextControl>().FirstOrDefault();
                }
                item.Open = false;
            }
            tab = (TabButtonControl)sender;
            try { GroopGrid.Children.Remove(GroopGrid.Children.OfType<TextControl>().FirstOrDefault()); } catch { }
            try { GroopGrid.Children.Remove(GroopGrid.Children.OfType<SplitTextControl>().FirstOrDefault()); } catch { }
            TextControl textControl = tab.GetTextControl;
            textControl.SetValue(Grid.RowProperty, 0);
            textControl.Margin = new Thickness(0, 1, 0, 0);
            textControl.SetValue(Panel.ZIndexProperty, 0);
            GroopGrid.Children.Add(tab.GetTextControl);
            SplitTextControl splitTextControl = tab.GetSplitTextControl;
            splitTextControl.SetValue(Grid.RowProperty, 1);
            splitTextControl.Margin = new Thickness(0, 1, 0, 0);
            splitTextControl.SetValue(Panel.ZIndexProperty, 0);
            GroopGrid.Children.Add(tab.GetSplitTextControl);
            switch (tab.IsSplit)
            {
                case true:
                    GroopGrid.Children[0].Visibility = Visibility.Collapsed;
                    GroopGrid.Children[1].Visibility = Visibility.Visible;
                    break;
                case false:
                default:
                    GroopGrid.Children[0].Visibility = Visibility.Visible;
                    GroopGrid.Children[1].Visibility = Visibility.Collapsed;
                    break;
            }
            #endregion
        }

        internal void ChangeLanguage(string len, Dictionary<string, MainWindow.ColorType> dictionary)
        {
            foreach (var item in TopPanel.Children.OfType<TabButtonControl>())
            {

                if (item.Open)
                {
                    item.GetSplitTextControl.Right.LsLanguage = len;
                    MainSplitTextControl.Right.LsLanguage = len;
                    item.GetSplitTextControl.Right.Dictionary = dictionary;
                    MainSplitTextControl.Right.Dictionary = dictionary;
                }
            }
        }

        private bool _isDown;
        private bool _isDragging;
        private Point _startPoint;
        private UIElement _realDragSource;
        private UIElement _dummyDragSource = new UIElement();
        private UIElement _tickDragSource = new UIElement();

        private void sp_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.Source == this.TopPanel)
            {
            }
            else
            {
                _isDown = true;
                _startPoint = e.GetPosition(this.TopPanel);
                TabMovementTimer.Start();
            }
        }

        private void sp_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            _isDown = false;
            _isDragging = false;
            try { _realDragSource.ReleaseMouseCapture(); } catch { }
            TabMovementTimer.Stop();
        }

        private void sp_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (_isDown)
            {
                if ((_isDragging == false) && ((Math.Abs(e.GetPosition(this.TopPanel).X - _startPoint.X) > SystemParameters.MinimumHorizontalDragDistance) ||
                    (Math.Abs(e.GetPosition(this.TopPanel).Y - _startPoint.Y) > SystemParameters.MinimumVerticalDragDistance)))
                {
                    _realDragSource = e.Source as UIElement;
                    if (((TabButtonControl)_realDragSource).Title != "+")
                    {
                        _isDragging = true;
                        _realDragSource.CaptureMouse();
                        DragDrop.DoDragDrop(_dummyDragSource, new DataObject("UIElement", e.Source, true), DragDropEffects.Move);
                    }
                }
            }
        }

        private void sp_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("UIElement"))
            {
                e.Effects = DragDropEffects.Move;
            }
        }

        System.Windows.Threading.DispatcherTimer TabMovementTimer = new System.Windows.Threading.DispatcherTimer();
        
        private void TabMovementTimer_Tick(object sender, EventArgs e)
        {
            //int droptargetIndex = -1, i = 0;
            //foreach (UIElement element in this.TopPanel.Children)
            //{
            //    if (element.Equals(_tickDragSource))
            //    {
            //        droptargetIndex = i;
            //        break;
            //    }
            //    i++;
            //}
            //if (droptargetIndex != -1)
            //{
            //    this.TopPanel.Children.Remove(_realDragSource);
            //    if (droptargetIndex == TopPanel.Children.Count)
            //    {
            //        this.TopPanel.Children.Insert(droptargetIndex - 1, _realDragSource);
            //    }
            //    else
            //    {
            //        this.TopPanel.Children.Insert(droptargetIndex, _realDragSource);
            //    }
            //}
        }

        private void sp_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("UIElement"))
            {
                UIElement droptarget = e.Source as UIElement;
                int droptargetIndex = -1, i = 0;
                foreach (UIElement element in this.TopPanel.Children)
                {
                    if (element.Equals(droptarget))
                    {
                        droptargetIndex = i;
                        break;
                    }
                    i++;
                }
                if (droptargetIndex != -1)
                {
                    this.TopPanel.Children.Remove(_realDragSource);
                    if (droptargetIndex == TopPanel.Children.Count)
                    {
                        this.TopPanel.Children.Insert(droptargetIndex -1, _realDragSource);
                    }
                    else
                    {
                        this.TopPanel.Children.Insert(droptargetIndex, _realDragSource);
                    }
                }

                _isDown = false;
                _isDragging = false;
                _realDragSource.ReleaseMouseCapture();
            }
        }

        public void TabAdd(string langage, Dictionary<string, MainWindow.ColorType> keyValuePairs)
        {
            TabButtonControl buttonControl = new TabButtonControl();
            buttonControl.LsLanguage = langage;
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = buttonControl.languageFilter[buttonControl.GetLanguage];
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            if (openFileDialog.ShowDialog() == true)
            {
                buttonControl.splitChanged += new TabButtonControl.SplitChanged(this.SplitChanged);
                buttonControl.GetTextControl.Dictionary = keyValuePairs;
                buttonControl.GetSplitTextControl.Left.Dictionary = keyValuePairs;
                buttonControl.GetSplitTextControl.Right.Dictionary = keyValuePairs;
                buttonControl.GetTextControl.LsLanguage = langage;
                buttonControl.GetSplitTextControl.Left.LsLanguage = langage;
                buttonControl.GetSplitTextControl.Right.LsLanguage = langage;
                buttonControl.ControlTabButtonClicked += new EventHandler(this.TabButtonControl_ControlClicked);
                buttonControl.CloseTabButtonClicked += new EventHandler(this.TabButtonClose_ControlClicked);
                buttonControl.Open = true;
                TabButtonControl_ControlClicked(buttonControl, new EventArgs());
                foreach (var item in TopPanel.Children.OfType<TabButtonControl>())
                {
                    item.Open = false;
                }
                TopPanel.Children.Insert(TopPanel.Children.Count - 1, buttonControl);

                buttonControl.Document = openFileDialog.FileName;
                buttonControl.GetTextControl.MainRichTextBox.Document.Blocks.Clear();
                buttonControl.GetTextControl.MainRichTextBox.AppendText(File.ReadAllText(buttonControl.Document));
                List<FormattedWord> formattedWords = new List<FormattedWord>();
                foreach (var item7 in ((SplitTextControl)this.GroopGrid.Children[1]).Right.Dictionary)
                {
                    formattedWords.Add(new FormattedWord(item7.Key, item7.Value.Color, item7.Value.Types));
                }
                (new RTX() { words = formattedWords }).update(ref buttonControl.GetTextControl.MainRichTextBox);
                buttonControl.GetSplitTextControl.Left.MainRichTextBox.Document.Blocks.Clear();
                buttonControl.GetSplitTextControl.Left.MainRichTextBox.AppendText(File.ReadAllText(buttonControl.Document));
                buttonControl.Title = $"{buttonControl.DocumentName}{buttonControl.DocumentType}";
            }
        }

        {
            bool next = false;
            TabButtonControl past = new TabButtonControl();
            foreach (var item in TopPanel.Children.OfType<TabButtonControl>())
            {
                if (item.Open)
                {
                    foreach (var item2 in TopPanel.Children.OfType<TabButtonControl>())
                    {

                        if (item2.Open)
                        {
                            item2.GetTextControl = GroopGrid.Children.OfType<TextControl>().FirstOrDefault();
                            item2.GetSplitTextControl = GroopGrid.Children.OfType<SplitTextControl>().FirstOrDefault();
                        }
                        item2.Open = false;
                    }
                    past.Open = true;
                    try { GroopGrid.Children.Remove(GroopGrid.Children.OfType<TextControl>().FirstOrDefault()); } catch { }
                    try { GroopGrid.Children.Remove(GroopGrid.Children.OfType<SplitTextControl>().FirstOrDefault()); } catch { }
                    TextControl textControl = past.GetTextControl;
                    textControl.SetValue(Grid.RowProperty, 0);
                    textControl.Margin = new Thickness(0, 1, 0, 0);
                    textControl.SetValue(Panel.ZIndexProperty, 0);
                    GroopGrid.Children.Add(past.GetTextControl);
                    SplitTextControl splitTextControl = past.GetSplitTextControl;
                    splitTextControl.SetValue(Grid.RowProperty, 1);
                    splitTextControl.Margin = new Thickness(0, 1, 0, 0);
                    splitTextControl.SetValue(Panel.ZIndexProperty, 0);
                    GroopGrid.Children.Add(past.GetSplitTextControl);
                    switch (past.IsSplit)
                    {
                        case true:
                            GroopGrid.Children[0].Visibility = Visibility.Collapsed;
                            GroopGrid.Children[1].Visibility = Visibility.Visible;
                            break;
                        case false:
                        default:
                            GroopGrid.Children[0].Visibility = Visibility.Visible;
                            GroopGrid.Children[1].Visibility = Visibility.Collapsed;
                            break;
                    }
                    break;
                }
                next = item.Open;
                item.Open = false;
                past = item;
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            bool next = false;
            TabButtonControl past = new TabButtonControl();
            foreach (var item in TopPanel.Children.OfType<TabButtonControl>())
            {
                if (item.Title == "+")
                {
                    foreach (var item2 in TopPanel.Children.OfType<TabButtonControl>())
                    {

                        if (item2.Open)
                        {
                            item2.GetTextControl = GroopGrid.Children.OfType<TextControl>().FirstOrDefault();
                            item2.GetSplitTextControl = GroopGrid.Children.OfType<SplitTextControl>().FirstOrDefault();
                        }
                        item2.Open = false;
                    }
                    past.Open = true;
                    try { GroopGrid.Children.Remove(GroopGrid.Children.OfType<TextControl>().FirstOrDefault()); } catch { }
                    try { GroopGrid.Children.Remove(GroopGrid.Children.OfType<SplitTextControl>().FirstOrDefault()); } catch { }
                    TextControl textControl = past.GetTextControl;
                    textControl.SetValue(Grid.RowProperty, 0);
                    textControl.Margin = new Thickness(0, 1, 0, 0);
                    textControl.SetValue(Panel.ZIndexProperty, 0);
                    GroopGrid.Children.Add(past.GetTextControl);
                    SplitTextControl splitTextControl = past.GetSplitTextControl;
                    splitTextControl.SetValue(Grid.RowProperty, 1);
                    splitTextControl.Margin = new Thickness(0, 1, 0, 0);
                    splitTextControl.SetValue(Panel.ZIndexProperty, 0);
                    GroopGrid.Children.Add(past.GetSplitTextControl);
                    switch (past.IsSplit)
                    {
                        case true:
                            GroopGrid.Children[0].Visibility = Visibility.Collapsed;
                            GroopGrid.Children[1].Visibility = Visibility.Visible;
                            break;
                        case false:
                        default:
                            GroopGrid.Children[0].Visibility = Visibility.Visible;
                            GroopGrid.Children[1].Visibility = Visibility.Collapsed;
                            break;
                    }
                    break;
                }
                else if (next)
                {
                    foreach (var item2 in TopPanel.Children.OfType<TabButtonControl>())
                    {

                        if (item2.Open)
                        {
                            item2.GetTextControl = GroopGrid.Children.OfType<TextControl>().FirstOrDefault();
                            item2.GetSplitTextControl = GroopGrid.Children.OfType<SplitTextControl>().FirstOrDefault();
                        }
                        item2.Open = false;
                    }
                    item.Open = true;
                    try { GroopGrid.Children.Remove(GroopGrid.Children.OfType<TextControl>().FirstOrDefault()); } catch { }
                    try { GroopGrid.Children.Remove(GroopGrid.Children.OfType<SplitTextControl>().FirstOrDefault()); } catch { }
                    TextControl textControl = item.GetTextControl;
                    textControl.SetValue(Grid.RowProperty, 0);
                    textControl.Margin = new Thickness(0, 1, 0, 0);
                    textControl.SetValue(Panel.ZIndexProperty, 0);
                    GroopGrid.Children.Add(item.GetTextControl);
                    SplitTextControl splitTextControl = item.GetSplitTextControl;
                    splitTextControl.SetValue(Grid.RowProperty, 1);
                    splitTextControl.Margin = new Thickness(0, 1, 0, 0);
                    splitTextControl.SetValue(Panel.ZIndexProperty, 0);
                    GroopGrid.Children.Add(item.GetSplitTextControl);
                    switch (item.IsSplit)
                    {
                        case true:
                            GroopGrid.Children[0].Visibility = Visibility.Collapsed;
                            GroopGrid.Children[1].Visibility = Visibility.Visible;
                            break;
                        case false:
                        default:
                            GroopGrid.Children[0].Visibility = Visibility.Visible;
                            GroopGrid.Children[1].Visibility = Visibility.Collapsed;
                            break;
                    }
                    break;
                }
                next = item.Open;
                item.Open = false;
                past = item;
            }
        }
    }
}
