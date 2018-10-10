using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Language_Swopper_App
{
    /// <summary>
    /// Interaction logic for LoadingScreen.xaml
    /// </summary>
    public partial class LoadingScreen : Window
    {

        public LoadingScreen()
        {
            InitializeComponent();
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            BackgroundWorker worker = new BackgroundWorker();
            worker.WorkerReportsProgress = true;
            worker.DoWork += worker_DoWork;
            worker.ProgressChanged += worker_ProgressChanged;
            worker.RunWorkerCompleted += worker_RunWorkerCompleted;
            worker.RunWorkerAsync();
        }

        private MainWindow main;
        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            Thread thread = new Thread(() =>
            {
                main = new MainWindow((sender as BackgroundWorker));
            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            while (thread.ThreadState != ThreadState.Stopped){}
            Thread.Sleep(1000);
        }

        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Hide();
            main.ShowDialog();
            this.Close();
        }
        void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            lsStatus.Value = e.ProgressPercentage;
        }

        private void lsStatus_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            lsStatus.ValueChanged -= lsStatus_ValueChanged;
            for (double i = e.OldValue; i < e.NewValue + 1; i++)
            {
                lsStatus.Value = i;
                Thread.Sleep(10);
            }
            lsStatus.ValueChanged += lsStatus_ValueChanged;
        }
    }

    public class PercentToDecimalConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((double)value < 46)
                return (double)0;
            else if((double)value > 54)
                return (double)1;
            else
            {
                return ((((double)value - 46) / 8) * 100) / 100;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
