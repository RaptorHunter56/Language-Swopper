using System;
using System.Collections.Generic;
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

namespace Language_Swopper_App
{
    /// <summary>
    /// Interaction logic for TabButtonControl.xaml
    /// </summary>
    public partial class TabButtonControl : UserControl
    {
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }
        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register("Title", typeof(string), typeof(TabButtonControl), new UIPropertyMetadata(""));
        public bool Open
        {
            get { return (bool)GetValue(OpenProperty); }
            set { SetValue(OpenProperty, value); if (Open) ExitButton.Visibility = Visibility.Visible; else ExitButton.Visibility = Visibility.Hidden; }
        }
        public static readonly DependencyProperty OpenProperty = DependencyProperty.Register("Open", typeof(bool), typeof(TabButtonControl), new PropertyMetadata(false));
        public int TabFontSize
        {
            get { return (int)GetValue(TabFontSizeProperty); }
            set { SetValue(TabFontSizeProperty, value); }
        }
        public static readonly DependencyProperty TabFontSizeProperty = DependencyProperty.Register("TabFontSize", typeof(int), typeof(TabButtonControl), new PropertyMetadata(14));
        public Thickness TabPadding
        {
            get { return (Thickness)GetValue(TabPaddingProperty); }
            set { SetValue(TabPaddingProperty, value); }
        }
        public static readonly DependencyProperty TabPaddingProperty = DependencyProperty.Register("TabPadding", typeof(Thickness), typeof(TabButtonControl), new PropertyMetadata(new Thickness(5, -1, 5, 1)));

        public TabButtonControl()
        {
            InitializeComponent();
            GetTextControl = new TextControl();
        }

        public event EventHandler ControlTabButtonClicked;
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (ControlTabButtonClicked != null)
                ControlTabButtonClicked(this, EventArgs.Empty);
            if (Title != "+")
                Open = true;
        }

        public TextControl GetTextControlT()
        {
            return GetTextControl;
        }
        public void Set(ref TextControl mainTextControl)
        {
            mainTextControl = GetTextControl;
        }
        public TextControl GetTextControl { get; set; }

        public event EventHandler CloseTabButtonClicked;
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (CloseTabButtonClicked != null)
                CloseTabButtonClicked(this, EventArgs.Empty);
        }

        private void Button_MouseEnter(object sender, MouseEventArgs e)
        {
            ExitButton.Visibility = Visibility.Visible;
        }

        private void Button_MouseLeave(object sender, MouseEventArgs e)
        {
            if (!Open)
             ExitButton.Visibility = Visibility.Hidden;
        }

        private void ExitButton_MouseEnter(object sender, MouseEventArgs e)
        {
            ExitButton.FontWeight = FontWeights.DemiBold;
        }

        private void ExitButton_MouseLeave(object sender, MouseEventArgs e)
        {
            ExitButton.FontWeight = FontWeights.Normal;
        }
        //{
        //    get { return (TextControl)GetValue(GetTextControlProperty); }
        //    set { SetValue(GetTextControlProperty, value); }
        //}
        //public static readonly DependencyProperty GetTextControlProperty = DependencyProperty.Register("GetTextControl", typeof(TextControl), typeof(TabButtonControl), new PropertyMetadata(new TextControl()));
    }
}
