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
        public bool IsSplit { get { return isSplit; } set { isSplit = value; try { splitChanged(this); } catch (Exception e) { /*MessageBox.Show(e.Message);*/ } } }
        private bool isSplit = false;
        public delegate void SplitChanged(object sender);
        public event SplitChanged splitChanged;

        public string Document;
        public string DocumentName { get { return Document.Split("\\".ToCharArray()[0]).Last().Split('.').First(); } }
        public string DocumentPath { get { string tempstring = ""; for (int i = 0; i < Document.Split("\\".ToCharArray()[0]).Length - 2; i++) { tempstring += Document.Split("\\".ToCharArray()[0])[i]; } return tempstring; } }
        public string DocumentType { get { return $".{Document.Split('.').Last()}"; } }

        public string LsLanguage { get { return language; } set { language = value; try { LanguageUpdated(); } catch { } } }
        public string GetLanguage { get { return language; } }
        private string language;
        public Dictionary<string, string> languageFilter = new Dictionary<string, string>()
        {
            { "Python", "Python files (*.py)|*.py|Text files (*.txt)|*.txt|All files (*.*)|*.*"},
            { "C#", "C# (*.cs)|*.py|Text files (*.txt)|*.txt|All files (*.*)|*.*"},
            { "Visual Basic", "Visual Basic (*.vb)|*.py|Text files (*.txt)|*.txt|All files (*.*)|*.*"},
            { "MySql", "MySQL files (*.sql)|*.sql|Text files (*.txt)|*.txt|All files (*.*)|*.*"}
        };
        //had to restart needed a change to save this can be deleted later
        public delegate void LanguageUpdate();
        public event LanguageUpdate LanguageUpdated;



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
            GetSplitTextControl = new SplitTextControl();
        }


        public TabButtonControl(TabButtonControl t)
        {
            InitializeComponent();
            this.Title = t.Title;
            this.ControlTabButtonClicked = t.ControlTabButtonClicked;
            this.CloseTabButtonClicked = t.CloseTabButtonClicked;
            this.GetTextControl = t.GetTextControl;
            this.GetSplitTextControl = t.GetSplitTextControl;
            Button_Click(BackButton, new RoutedEventArgs());
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



        public SplitTextControl GetSplitTextControlT()
        {
            return GetSplitTextControl;
        }
        public void Set(ref SplitTextControl mainSplitTextControl)
        {
            mainSplitTextControl = GetSplitTextControl;
        }
        public SplitTextControl GetSplitTextControl { get; set; }

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
