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
using Telerik.Windows.Controls;
using Reponse = TeacherClient.Contract.Reponse;
using Request = TeacherClient.Contract.Request;

namespace TeacherClient
{
    /// <summary>
    /// FindPasswordWindow.xaml 的交互逻辑
    /// </summary>
    public partial class FindPasswordWindow : WindowBase
    {
        FindPasswordModel _model = new FindPasswordModel();
        public FindPasswordWindow()
        {
            InitializeComponent();
            this.DataContext = _model;
            this.IsBusy = false;
        }

        private void ToNewPassword_Click(object sender, RoutedEventArgs e)
        {
            CheckOldPhone();
        }
        async void CheckOldPhone()
        {
            this.IsBusy = true;
            Request.checkCode l = new Request.checkCode() { lec_id = App.CurrentLogin.lec_id, token = App.CurrentLogin.token, phone = textBox.Text, type = Config.phoneCode.resetPassword, code = recvcode.Text };
            var t = await WebHelper.doPost<string, Request.checkCode>(Config.Interface_checkCode, l);
            if (t != null)
                _model.ShowSecondPage = true;
            this.IsBusy = false;
        }

        private async void BtnOK_Click(object sender, RoutedEventArgs e)
        {
            if (pwd_new.Pwd != pwd_newRepeat.Pwd)
            {
                MessageWindow.Alter("提示", "两次密码输入不一致");
                return;
            }
            this.IsBusy = true;
            Request.resetPwd l = new Request.resetPwd() { lec_id = App.CurrentLogin.lec_id, token = App.CurrentLogin.token, phone = textBox.Text, password = pwd_new.Pwd, code = recvcode.Text };
            var t = await WebHelper.doPost<Request.resetPwd>(Config.Interface_resetPwd, l);
            if (t)
                _model.ShowThirdPage = true;
            this.IsBusy = false;            
        }

        private void BtnSure_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private async void btn_GetPhoneCode(object sender, RoutedEventArgs e)
        {
            this.IsBusy = true;
            Request.phoneCode l = new Request.phoneCode() { lec_id = App.CurrentLogin.lec_id, token = App.CurrentLogin.token, phone = textBox.Text, type = Config.phoneCode.resetPassword };
            var t = await WebHelper.doPost<object, Request.phoneCode>(Config.Interface_phoneCode, l);
            this.IsBusy = false;

        }
    }
    public class FindPasswordModel : ViewModelBase
    {

        public FindPasswordModel()
        {
            ShowFirstPage = true;
        }

        bool _ShowFirstPage;
        /// <summary>
        /// 页面1
        /// </summary>
        public bool ShowFirstPage
        {
            get { return _ShowFirstPage; }
            set
            {
                if (value != _ShowFirstPage)
                {
                    _ShowFirstPage = value;
                    OnPropertyChanged("ShowFirstPage");
                }
            }
        }

        bool _ShowSecondPage;
        /// <summary>
        /// 页面2
        /// </summary>
        public bool ShowSecondPage
        {
            get { return _ShowSecondPage; }
            set
            {
                if (value != _ShowSecondPage)
                {
                    _ShowSecondPage = value;
                    OnPropertyChanged("ShowSecondPage");
                }
            }
        }

        bool _ShowThirdPage;
        /// <summary>
        /// 页面3
        /// </summary>
        public bool ShowThirdPage
        {
            get { return _ShowThirdPage; }
            set
            {
                if (value != _ShowThirdPage)
                {
                    _ShowThirdPage = value;
                    OnPropertyChanged("ShowThirdPage");
                }
            }
        }


    }
}
