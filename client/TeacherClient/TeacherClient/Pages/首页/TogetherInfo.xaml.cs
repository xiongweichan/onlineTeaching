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
using Telerik.Windows.Controls;
using TeacherClient.Core;
using Reponse = TeacherClient.Contract.Reponse;
using Request = TeacherClient.Contract.Request;

namespace TeacherClient.Pages
{
    /// <summary>
    /// TogetherInfo.xaml 的交互逻辑
    /// </summary>
    public partial class TogetherInfo : UserControl
    {
        public TogetherInfo()
        {
            InitializeComponent();
            //Init();

        }
        public void RefreshData()
        {
            Init();
        }
        async void Init()
        {
            MainWindow.Current.IsBusy = true;
            Request.ParamBase l = new Request.ParamBase() { lec_id = App.CurrentLogin.lec_id, token = App.CurrentLogin.token };
            var t = await WebHelper.doPost<Reponse.MainIndex, Request.ParamBase>(Config.Interface_mainIndex, l);
            App.CurrentLogin.user.lecturer_code = t.userInfo.lecturer_code;
            this.DataContext = t;
            MainWindow.Current.IsBusy = false;
        }

        private void StackPanel_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var id = (sender as StackPanel).Tag.ObjToString();
            MainWindow.Current.NavigateToPage(2, 2, id);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainIndex.Current.NavigateToPage(1, null);
        }

        private void SPWaitLive_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            MainWindow.Current.NavigateToPage(1, 0, 1);

        }

        private void SPWaitCheck_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            MainWindow.Current.NavigateToPage(1, 0, 3);

        }

        private void SPWaitReward_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            MainWindow.Current.NavigateToPage(4, 0, null);

        }
    }
}
