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
        public MenuControl()
        {
            InitializeComponent();
        }
        public string language;
        public delegate void languageChanged();
        public event MenuOpen languageChangedClicked;
        private void MenuItem_Checked(object sender, RoutedEventArgs e)
        {
            //To Do
            if (((MenuItem)sender).IsChecked)
            {
                language = ((MenuItem)sender).Header.ToString();
                foreach (MenuItem item in LanguageMenu.Items.OfType<MenuItem>())
                {
                    if (item != sender)
                        item.IsChecked = false;
                }
                try { languageChangedClicked(); } catch { }
            }
        }

        public delegate void MenuOpen();
        public event MenuOpen MenuOpenClicked;
        private void OpenMenuItem_Click(object sender, RoutedEventArgs e)
        {
            try { MenuOpenClicked(); } catch { }
        }

        public delegate void MenuSave();
        public event MenuOpen MenuSaveClicked;
        private void SaveMenuItem_Click(object sender, RoutedEventArgs e)
        {
            try { MenuSaveClicked(); } catch { }
        }

        public delegate void MenuRefreshLanguage();
        public event MenuOpen MenuRefreshLanguageClicked;
        private void RefreshLanguageMenuItem_Click(object sender, RoutedEventArgs e)
        {
            try { MenuRefreshLanguageClicked(); } catch { }
        }


        public delegate void Split();
        public event Split Split_Clicked;
        private void Split_Click(object sender, RoutedEventArgs e)
        {
            try { Split_Clicked(); } catch { }
        }
    }
}
