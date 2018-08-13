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
    /// Interaction logic for MenuControl.xaml
    /// </summary>
    public partial class MenuControl : UserControl
    {
        private string document;
        public string DocumentName { get { return document.Split("\\".ToCharArray()[0]).Last().Split('.').First(); } }
        public string DocumentPath { get { string tempstring = ""; for (int i = 0; i < document.Split("\\".ToCharArray()[0]).Length - 2; i++) { tempstring += document.Split("\\".ToCharArray()[0])[i]; } return tempstring; } }
        public string DocumentType { get { return $".{document.Split('.').Last()}"; } }

        private string Language { get { return language; } set { language = value; try { LanguageUpdated(); } catch { } } }
        public string GetLanguage { get { return language; } }
        private string language;

        public delegate void LanguageUpdate();
        public event LanguageUpdate LanguageUpdated;

        public MenuControl()
        {
            InitializeComponent();
        }

        private void MenuItem_Checked(object sender, RoutedEventArgs e)
        {
            if (((MenuItem)sender).IsChecked)
            {
                Language = ((MenuItem)sender).Header.ToString();
                foreach (MenuItem item in LanguageMenu.Items.OfType<MenuItem>())
                {
                    if (item != sender)
                        item.IsChecked = false;
                }
            }
        }
    }
}
