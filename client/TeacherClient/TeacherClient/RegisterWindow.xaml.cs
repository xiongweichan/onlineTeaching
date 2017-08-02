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
using System.Windows.Shapes;
using System.Windows.Threading;
using Telerik.Windows.Controls;

namespace TeacherClient
{
    /// <summary>
    /// RegisterWindow.xaml 的交互逻辑
    /// </summary>
    public partial class RegisterWindow : WindowBase
    {
        public RegisterWindow()
        {
            InitializeComponent();
            this.DataContext = this;
            this.IsBusy = false;
            _timer = new DispatcherTimer(new TimeSpan(0, 0, 1), DispatcherPriority.Normal, Time_Tick, this.Dispatcher);
        }

        private void Time_Tick(object sender, EventArgs e)
        {
            Count--;
            run_time.Text = Count.ToString();
            if (Count == 0)
            {
                BtnSure_Click(null, null);
                _timer.Stop();
            }
        }

        private void BtnSure_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Run_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        DispatcherTimer _timer;
        public int Count { get; set; }

        private void btnNextStep_Click(object sender, RoutedEventArgs e)
        {
            if (firstpage.IsChecked.HasValue && firstpage.IsChecked.Value)
            {
                secondpage.IsChecked = true;
            }
            else if (secondpage.IsChecked.HasValue && secondpage.IsChecked.Value)
            {
                thirdpage.IsChecked = true;
                Count = 5;
                run_time.Text = Count.ToString();
                _timer.Start();
            }

        }
    }
}
