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
            SamSam.ControlTabButtonClicked += new EventHandler(this.TabButtonControl_ControlClicked);
            SamName.ControlTabButtonClicked += new EventHandler(this.TabButtonControl_ControlClicked);
            Pluss.ControlTabButtonClicked += new EventHandler(this.TabButtonControl_ControlClicked2);
            SamSam.CloseTabButtonClicked += new EventHandler(this.TabButtonClose_ControlClicked);
            SamName.CloseTabButtonClicked += new EventHandler(this.TabButtonClose_ControlClicked);

            TabMovementTimer.Tick += TabMovementTimer_Tick;
            TabMovementTimer.Interval = new TimeSpan(0, 0, 1);
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
                }
                item.Open = false;
            }
            tab = (TabButtonControl)sender;
            try { GroopGrid.Children.Remove(GroopGrid.Children.OfType<TextControl>().FirstOrDefault()); } catch { }
            TextControl textControl = tab.GetTextControl;
            textControl.SetValue(Grid.RowProperty, 2);
            textControl.Margin = new Thickness(0, 1, 0, 0);
            textControl.SetValue(Panel.ZIndexProperty, 0);
            GroopGrid.Children.Add(tab.GetTextControl);
            #endregion
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

        public void TabAdd(string langage, Dictionary<string, Color> keyValuePairs)
        {
            TabButtonControl buttonControl = new TabButtonControl();
            buttonControl.LsLanguage = langage;
            buttonControl.GetTextControl.Dictionary = keyValuePairs;
            buttonControl.ControlTabButtonClicked += new EventHandler(this.TabButtonControl_ControlClicked);
            buttonControl.CloseTabButtonClicked += new EventHandler(this.TabButtonClose_ControlClicked);
            buttonControl.Open = true;
            TabButtonControl_ControlClicked(buttonControl, new EventArgs());
            foreach (var item in TopPanel.Children.OfType<TabButtonControl>())
            {
                item.Open = false;
            }
            TopPanel.Children.Insert(TopPanel.Children.Count - 1, buttonControl);
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = buttonControl.languageFilter[buttonControl.GetLanguage];
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            if (openFileDialog.ShowDialog() == true)
            {
                buttonControl.Document = openFileDialog.FileName;
                buttonControl.GetTextControl.MainRichTextBox.Document.Blocks.Clear();
                buttonControl.GetTextControl.MainRichTextBox.AppendText(File.ReadAllText(buttonControl.Document));
            }
            buttonControl.Title = $"{buttonControl.DocumentName}{buttonControl.DocumentType}";
        }
    }
}
