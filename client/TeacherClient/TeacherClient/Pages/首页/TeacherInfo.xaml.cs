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
using TeacherClient.Core;
using Reponse = TeacherClient.Contract.Reponse;
using Request = TeacherClient.Contract.Request;

namespace TeacherClient.Pages
{
    /// <summary>
    /// TeacherInfo.xaml 的交互逻辑
    /// </summary>
    public partial class TeacherInfo : UserControl
    {
        public TeacherInfo()
        {
            InitializeComponent();
            Init();
        }

        async void Init()
        {
            (App.Current.MainWindow as MainWindow).IsBusy = true;
            Request.ParamBase l = new Request.ParamBase() { lec_id = App.CurrentLogin.lec_id, token = App.CurrentLogin.token };
            var t = await IPCHandle.doPost<Reponse.ResponseParam<Reponse.userInfo>>(Config.Interface_userInfo, l.ReturnRequestParam());
            if (t != null && t.status == Config.SuccessCode)
            {
                SetBirthDay(t.data.birth);
                (App.Current.MainWindow as MainWindow).IsBusy = false;
            }
            else
            {
                MessageWindow.Alter("错误提示", t != null ? t.info : "获取讲师信息失败，请检查服务是否正常");
            }

        }

        void SetBirthDay(string birth)
        {
            birth = birth ?? string.Empty;
            var list = new List<string>();
            for (int i = 1; i <= 12; i++)
                list.Add(string.Format("{0}月", i));
            months.ItemsSource = list;
            string[] arr = birth.Split(new string[] { "月", "日" }, StringSplitOptions.RemoveEmptyEntries);
            if (arr.Length == 2)
            {
                months.SelectedItem = arr[0] + "月";
                days.SelectedItem = arr[1] + "日";
            }
            else
            {
                months.SelectedIndex = 0;
                days.SelectedIndex = 0;
            }
        }

        private void months_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var m = months.SelectedIndex + 1;
            var t = m == 2 ? 29 : ((m % 2 == 1 && m < 8) || (m >= 8 && m % 2 == 0)) ? 31 : 30;
            var list = new List<string>();
            for (int i = 1; i <= t; i++) list.Add(string.Format("{0}日", i));
            days.ItemsSource = list;
        }

        private void phoneNext_Click(object sender, RoutedEventArgs e)
        {
            if(rb_pcheck.IsChecked.HasValue && rb_pcheck.IsChecked.Value)
            {
                rb_pedit.IsChecked = true;
            }
            else if(rb_pedit.IsChecked.HasValue && rb_pedit.IsChecked.Value)
            {
                rb_pcompleted.IsChecked = true;
            }
            else if(rb_pcompleted.IsChecked.HasValue && rb_pcompleted.IsChecked.Value)
            {
                rb_pcheck.IsChecked = true;
            }
        }

        private void emailNext_Click(object sender, RoutedEventArgs e)
        {
            if (rb_mcheck.IsChecked.HasValue && rb_mcheck.IsChecked.Value)
            {
                rb_medit.IsChecked = true;
            }
            else if (rb_medit.IsChecked.HasValue && rb_medit.IsChecked.Value)
            {
                rb_mcompleted.IsChecked = true;
            }
            else if (rb_mcompleted.IsChecked.HasValue && rb_mcompleted.IsChecked.Value)
            {
                rb_medit.IsChecked = true;
            }
        }
    }
}
