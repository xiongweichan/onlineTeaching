using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TeacherClient.Pages
{
    /// <summary>
    /// RequestLive.xaml 的交互逻辑
    /// </summary>
    public partial class RequestLive : UserControl
    {
        public RequestLive()
        {
            InitializeComponent();
        }

        private void UploadCourse_Click(object sender, RoutedEventArgs e)
        {

        }
        CancellationTokenSource _token;
        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            if (firstpage.IsChecked == true)
                secondpage.IsChecked = true;
            else if (secondpage.IsChecked == true)
                thirdpage.IsChecked = true;
            else if (thirdpage.IsChecked == true)
            {
                fourthpage.IsChecked = true;
                _token = new CancellationTokenSource();
                Task.Factory.StartNew(new Action(() =>
                {
                    for (int i = 0; i < 5; i++)
                    {
                        _token.Token.ThrowIfCancellationRequested();
                        Thread.Sleep(1000);
                    }
                    _token.Token.ThrowIfCancellationRequested();
                    this.Dispatcher.Invoke(new Action(() =>
                    {
                        LiveCenter.Current.Type = 0;
                    }));
                }), _token.Token);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _token.Cancel();
            LiveCenter.Current.Type = 0;
        }
    }
}
