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


        public Reponse.userInfo UserInfo
        {
            get { return (Reponse.userInfo)GetValue(UserInfoProperty); }
            set { SetValue(UserInfoProperty, value); }
        }

        // Using a DependencyProperty as the backing store for UserInfo.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty UserInfoProperty =
            DependencyProperty.Register("UserInfo", typeof(Reponse.userInfo), typeof(TeacherInfo), new PropertyMetadata());



        public string HeadPath
        {
            get { return (string)GetValue(HeadPathProperty); }
            set { SetValue(HeadPathProperty, value); }
        }

        // Using a DependencyProperty as the backing store for HeadPath.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HeadPathProperty =
            DependencyProperty.Register("HeadPath", typeof(string), typeof(TeacherInfo), new PropertyMetadata());

        async void Init()
        {
            (App.Current.MainWindow as MainWindow).IsBusy = true;
            Request.ParamBase l = new Request.ParamBase() { lec_id = App.CurrentLogin.lec_id, token = App.CurrentLogin.token };
            var t = await IPCHandle.doPost<Reponse.ResponseParam<Reponse.userInfo>>(Config.Interface_userInfo, l.ReturnRequestParam());

            (App.Current.MainWindow as MainWindow).IsBusy = false;
            if (t != null && t.status == Config.SuccessCode)
            {
                SetBirthDay(t.data.birth);
                UserInfo = t.data;
                HeadPath = t.data.head;
            }
            else
            {
                MessageWindow.Alter("错误提示", t != null ? t.info : "获取讲师信息失败，请检查服务是否正常");
            }
            cb_province.ItemsSource = SystemInit.Instance.Regions;
            this.DataContext = this;
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

        async void BtnChangedImage_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.Current.IsBusy = true;
            HeadPath = await UploadImageHelper.UploadImage();
            if(!string.IsNullOrEmpty(HeadPath) && HeadPath != UserInfo.head)
            {
                Request.userHeadSet l = new Contract.Request.userHeadSet() { lec_id = App.CurrentLogin.lec_id, token = App.CurrentLogin.token, head = HeadPath };
                if (!await WebHelper.doPost<Request.userHeadSet>(Config.Interface_userHeadSet, l))
                    HeadPath = UserInfo.head;
            }
            MainWindow.Current.IsBusy = false;
        }

        private void cb_province_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cb_province.SelectedItem is Reponse.region)
            {
                cb_city.ItemsSource = (cb_province.SelectedItem as Reponse.region).data;
                //cb_city.SelectedIndex = 0;
            }
        }

        private void cb_city_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cb_city.SelectedItem is Reponse.region)
            {
                cb_district.ItemsSource = (cb_city.SelectedItem as Reponse.region).data;
                //cb_district.SelectedIndex = 0;
            }
        }
    }
}
