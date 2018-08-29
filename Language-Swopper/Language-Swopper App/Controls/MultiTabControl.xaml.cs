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
        }
        private void TabButtonClose_ControlClicked(object sender, EventArgs e)
        {
            if (((TabButtonControl)sender).Open && TopPanel.Children.Count > 2)
            {
                int rmin = 1;
                if ((TopPanel.Children.IndexOf((TabButtonControl)sender) - 1) < 0)
                    rmin = 0;
                TabButtonControl_ControlClicked(TopPanel.Children[TopPanel.Children.IndexOf((TabButtonControl)sender) - rmin], new EventArgs());
                ((TabButtonControl)TopPanel.Children[TopPanel.Children.IndexOf((TabButtonControl)sender) - rmin]).Open = true;
            }
            if (TopPanel.Children.Count == 2)
            {
                GroopGrid.Children.Remove(GroopGrid.Children.OfType<TextControl>().FirstOrDefault());
            }
            TopPanel.Children.Remove((TabButtonControl)sender);
        }
        private void TabButtonControl_ControlClicked2(object sender, EventArgs e)
        {
            TabButtonControl buttonControl = new TabButtonControl();
            buttonControl.Title = "NewDoc";
            buttonControl.Name = "NewDoc";
            buttonControl.ControlTabButtonClicked += new EventHandler(this.TabButtonControl_ControlClicked);
            buttonControl.CloseTabButtonClicked += new EventHandler(this.TabButtonClose_ControlClicked);
            buttonControl.Open = true;
            TabButtonControl_ControlClicked(buttonControl, new EventArgs());
            foreach (var item in TopPanel.Children.OfType<TabButtonControl>())
            {
                item.Open = false;
            }
            TopPanel.Children.Insert(TopPanel.Children.Count - 1, buttonControl);
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
    }
}
