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
            RePopluateMenu();
            //try { languageChangedClicked(language); } catch { }
        }
        public string language;
        private bool First = true;

        public delegate void languageChanged(string Lan);
        public event languageChanged languageChangedClicked;
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
                try { languageChangedClicked(language); } catch { }
            }
        }

        public delegate void MenuOpen();
        public event MenuOpen MenuOpenClicked;
        private void OpenMenuItem_Click(object sender, ExecutedRoutedEventArgs e)
        {
            try { MenuOpenClicked(); } catch { }
        }

        public delegate void MenuSave();
        public event MenuOpen MenuSaveClicked;
        private void SaveMenuItem_Click(object sender, ExecutedRoutedEventArgs e)
        {
            try { MenuSaveClicked(); } catch { }
        }

        public delegate void MenuRefreshLanguage();
        public event MenuOpen MenuRefreshLanguageClicked;
        private void RefreshLanguageMenuItem_Click(object sender, RoutedEventArgs e)
        {
            try { MenuRefreshLanguageClicked(); RePopluateMenu(); } catch { }
        }

        public void RePopluateMenu()
        {
            LanguageMenu.Items.Clear();
            using (var _context = new FileContext())
            {
                foreach (var folder in _context.Folders)
                {
                    if (folder.Name != "Base")
                    {
                        //< MenuItem x: Name = "VisualBasicLanguageMenu" Header = "Visual Basic" IsCheckable = "True" Checked = "MenuItem_Checked" />
                        MenuItem temp = new MenuItem();
                        //temp.Name = $"{folder.Name}LanguageMenu";
                        temp.Header = $"{folder.Name}";
                        temp.IsCheckable = true;
                        temp.Checked += MenuItem_Checked;
                        LanguageMenu.Items.Add(temp);
                        if (First) language = folder.Name;
                        First = false;
                    }
                }
            }
            int count = 1;
            foreach (MenuItem item in LanguageMenu.Items.OfType<MenuItem>())
            {
                if (count == 1)
                    item.IsChecked = true;
                count++;
                break;
            }
        }

        public delegate void Split();
        public event Split Split_Clicked;
        private void Split_Click(object sender, RoutedEventArgs e)
        {
            try { Split_Clicked(); } catch { }
        }
    }
}
