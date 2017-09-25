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
        }

        public void RefreshData()
        {
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

        public string _copy;

        public string HeadPath
        {
            get { return (string)GetValue(HeadPathProperty); }
            set { SetValue(HeadPathProperty, value); }
        }

        // Using a DependencyProperty as the backing store for HeadPath.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HeadPathProperty =
            DependencyProperty.Register("HeadPath", typeof(string), typeof(TeacherInfo), new PropertyMetadata());



        public Request.changePhone ChangePhone
        {
            get { return (Request.changePhone)GetValue(ChangePhoneProperty); }
            set { SetValue(ChangePhoneProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ChangePhone.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ChangePhoneProperty =
            DependencyProperty.Register("ChangePhone", typeof(Request.changePhone), typeof(TeacherInfo), new PropertyMetadata());


        public Request.changeEmail ChangeEmail
        {
            get { return (Request.changeEmail)GetValue(ChangeEmailProperty); }
            set { SetValue(ChangeEmailProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ChangeEmail.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ChangeEmailProperty =
            DependencyProperty.Register("ChangeEmail", typeof(Request.changeEmail), typeof(TeacherInfo), new PropertyMetadata());




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

                ChangePhone = new Contract.Request.changePhone { phone = t.data.phone };
                ChangeEmail = new Contract.Request.changeEmail { email = t.data.email };
                _copy = t.data.ToJson();
            }
            else
            {
                MessageWindow.Alter("错误提示", t != null ? t.info : "获取讲师信息失败，请检查服务是否正常");
            }
            cb_province.ItemsSource = SystemInit.Instance.Regions;
            cb_lecturerType.ItemsSource = SystemInit.Instance.LecturerTypeList;
            cb_lecturerGoodAt.ItemsSource = SystemInit.Instance.LecturerGoodAtList;
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
        string GetBirthDay()
        {
            return string.Format("{0}{1}", months.SelectedItem, days.SelectedItem);
        }

        private void months_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var m = months.SelectedIndex + 1;
            var t = m == 2 ? 29 : ((m % 2 == 1 && m < 8) || (m >= 8 && m % 2 == 0)) ? 31 : 30;
            var list = new List<string>();
            for (int i = 1; i <= t; i++) list.Add(string.Format("{0}日", i));
            days.ItemsSource = list;
        }

        async void phoneNext_Click(object sender, RoutedEventArgs e)
        {
            if (rb_pcheck.IsChecked.HasValue && rb_pcheck.IsChecked.Value)
            {
                Request.checkCode l = new Request.checkCode() { lec_id = App.CurrentLogin.lec_id, token = App.CurrentLogin.token, phone = ChangePhone.phone, type = Config.phoneCode.resetPassword, code = ChangePhone.code };
                var t = await WebHelper.doPost<Request.checkCode>(Config.Interface_checkCode, l);
                if (t)
                    rb_pedit.IsChecked = true;
            }
            else if (rb_pedit.IsChecked.HasValue && rb_pedit.IsChecked.Value)
            {
                //提交电话号码修改
                if (await WebHelper.doPost<Request.changePhone>(Config.Interface_changePhone, ChangePhone))
                {
                    UserInfo.phone = ChangePhone.phone_new;
                    ChangePhone = new Contract.Request.changePhone() { phone = UserInfo.phone };
                    rb_pcompleted.IsChecked = true;
                    MainWindow.Current.Logout_Click(null, null);
                }
            }
            else if (rb_pcompleted.IsChecked.HasValue && rb_pcompleted.IsChecked.Value)
            {
                rb_pcheck.IsChecked = true;
                //MainWindow.Current.Logout_Click(null, null);
            }
        }

        async void emailNext_Click(object sender, RoutedEventArgs e)
        {
            if (rb_mcheck.IsChecked.HasValue && rb_mcheck.IsChecked.Value)
            {
                Request.checkEmailCode l = new Request.checkEmailCode() { lec_id = App.CurrentLogin.lec_id, token = App.CurrentLogin.token, email = ChangeEmail.email, type = Config.phoneCode.resetPassword, code = ChangeEmail.code };
                var t = await WebHelper.doPost<Request.checkEmailCode>(Config.Interface_checkEmailCode, l);
                if (t)
                    rb_medit.IsChecked = true;
            }
            else if (rb_medit.IsChecked.HasValue && rb_medit.IsChecked.Value)
            {
                //提交邮箱修改
                if (await WebHelper.doPost<Request.changeEmail>(Config.Interface_changeEmail, ChangeEmail))
                {
                    UserInfo.email = ChangeEmail.email_new;
                    ChangeEmail = new Contract.Request.changeEmail() { email = UserInfo.email };
                    rb_mcompleted.IsChecked = true;

                    MainWindow.Current.Logout_Click(null, null);
                }
            }
            else if (rb_mcompleted.IsChecked.HasValue && rb_mcompleted.IsChecked.Value)
            {
                rb_medit.IsChecked = true;
            }
        }

        async void BtnChangedImage_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.Current.IsBusy = true;
            var str = await UploadImageHelper.UploadImage();
            if (!string.IsNullOrEmpty(str) && str != UserInfo.head)
            {
                HeadPath = str;
                Request.userHeadSet l = new Contract.Request.userHeadSet() { lec_id = App.CurrentLogin.lec_id, token = App.CurrentLogin.token, head = HeadPath };
                if (!await WebHelper.doPost<Request.userHeadSet>(Config.Interface_userHeadSet, l))
                    HeadPath = UserInfo.head;
                else
                    UserInfo.head = HeadPath;
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

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.UserInfo = _copy.FromJson<Reponse.userInfo>();
            this.HeadPath = this.UserInfo.head;
        }

        async void btnSave_Click(object sender, RoutedEventArgs e)
        {
            UserInfo.birth = GetBirthDay();
            Request.userSet l = new Contract.Request.userSet();
            l.lec_id = App.CurrentLogin.lec_id;
            l.token = App.CurrentLogin.token;
            l.intro = this.UserInfo.intro;
            l.nickname = UserInfo.nickname;
            l.realname = UserInfo.realname;
            l.sex = UserInfo.sex;
            l.age = UserInfo.age;
            l.birth = UserInfo.birth;
            l.province = UserInfo.province;
            l.city = UserInfo.city;
            l.district = UserInfo.district;
            l.lecturer_type = UserInfo.lecturer_type;
            l.good_at = UserInfo.good_at;
            l.wechat = UserInfo.wechat;
            l.job = UserInfo.job;
            l.phone = UserInfo.phone;
            l.mail = UserInfo.email;
            if (await WebHelper.doPost<Request.userSet>(Config.Interface_userSet, l))
                MessageWindow.Alter("提示", "保存成功！");
        }
        int t1 = 60;
        async void btnGetCode_Click(object sender, RoutedEventArgs e)
        {
            Request.phoneCode l = new Contract.Request.phoneCode() { lec_id = App.CurrentLogin.lec_id, token = App.CurrentLogin.token, phone = ChangePhone.phone, type = Config.phoneCode.editOldPhone };
            bool b = await WebHelper.doPost<Request.phoneCode>(Config.Interface_phoneCode, l);
            if (b)
            {
                t1 = 60;
                TimerHelper.TimerEvent += TimerHelper_TimerEvent;
            }
        }

        private void TimerHelper_TimerEvent(object sender, EventArgs e)
        {
            t1--;
            if (t1 == 0)
            {
                tbl_oldcodeleave.Visibility = Visibility.Collapsed;
                btnOldPhone.Visibility = Visibility.Visible;
                TimerHelper.TimerEvent -= TimerHelper_TimerEvent;
            }
            else
            {
                tbl_oldcodeleave.Visibility = Visibility.Visible;
                btnOldPhone.Visibility = Visibility.Collapsed;
                tbl_oldcodeleave.Text = string.Format("{0}秒", t1);
            }
        }
        int t2 = 60;
        async void btnGetCode_Click2(object sender, RoutedEventArgs e)
        {
            Request.phoneCode l = new Contract.Request.phoneCode() { lec_id = App.CurrentLogin.lec_id, token = App.CurrentLogin.token, phone = ChangePhone.phone_new, type = Config.phoneCode.editNewPhone };
            var b = await WebHelper.doPost<Request.phoneCode>(Config.Interface_phoneCode, l);
            if (b)
            {
                t2 = 60;
                TimerHelper.TimerEvent += TimerHelper_TimerEvent2;
            }
        }

        private void TimerHelper_TimerEvent2(object sender, EventArgs e)
        {
            t2--;
            if (t2 == 0)
            {
                tbl_newcodeleave.Visibility = Visibility.Collapsed;
                btnNewPhone.Visibility = Visibility.Visible;
                TimerHelper.TimerEvent -= TimerHelper_TimerEvent2;
            }
            else
            {
                tbl_newcodeleave.Visibility = Visibility.Visible;
                btnNewPhone.Visibility = Visibility.Collapsed;
                tbl_newcodeleave.Text = string.Format("{0}秒", t2);
            }
        }
        async void btnGetOldEmailCode_Click(object sender, RoutedEventArgs e)
        {
            Request.emailCode l = new Contract.Request.emailCode() { lec_id = App.CurrentLogin.lec_id, token = App.CurrentLogin.token, email = ChangeEmail.email, type = Config.emailCode.editOldEmail };
            await WebHelper.doPost<Request.emailCode>(Config.Interface_emailCode, l);
        }

        async void btnGetNewEmailCode_Click(object sender, RoutedEventArgs e)
        {
            Request.emailCode l = new Contract.Request.emailCode() { lec_id = App.CurrentLogin.lec_id, token = App.CurrentLogin.token, email = ChangeEmail.email_new, type = Config.emailCode.editNewEmail };
            await WebHelper.doPost<Request.emailCode>(Config.Interface_emailCode, l);
        }

        private void rb_Binding_Checked(object sender, RoutedEventArgs e)
        {
            rb_pcheck.IsChecked = true;
            rb_mcheck.IsChecked = true;
        }
    }
}
