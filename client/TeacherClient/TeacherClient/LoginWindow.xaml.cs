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
using Request = TeacherClient.Contract.Request;
using Reponse = TeacherClient.Contract.Reponse;
using TeacherClient.Core;
using Newtonsoft.Json;
using System.Security.Cryptography;
using Telerik.Windows.Controls;

namespace TeacherClient
{
    /// <summary>
    /// LoginWindow.xaml 的交互逻辑
    /// </summary>
    public partial class LoginWindow : WindowBase
    {

        LoginModel _model = new LoginModel();

        public LoginWindow()
            : this(false)
        { }
        public LoginWindow(bool focus)
        {
            InitializeComponent();
            this.DataContext = _model;
            bool b;
            _model.AutoLogin = bool.TryParse(ConfigManagerHelper.GetConfigByName(Config.IsAutoLogin), out b) && b;
            _model.RememblePwd = bool.TryParse(ConfigManagerHelper.GetConfigByName(Config.IsRememberPwd), out b) && b;
            if (_model.RememblePwd)
            {
                _model.UserAccount = ConfigManagerHelper.GetConfigByName(Config.UserAccount);
                pwd.Pwd = ConfigManagerHelper.GetConfigByName(Config.Password);
            }

            this.IsBusy = false;
            if (_model.AutoLogin && !focus)
            {
                Login_Click(null, null);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Canvas_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }

        private void ForgetPassword_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            FindPasswordWindow win = new FindPasswordWindow();
            win.ShowDialog();
            // 在此处添加事件处理程序实现。
        }

        private void RegisterAccount_Click(object sender, RoutedEventArgs e)
        {
            RegisterWindow win = new RegisterWindow();
            win.ShowDialog();
        }

        private async void Login_Click(object sender, RoutedEventArgs e)
        {
            //15989010000 123456
            Request.Login l = new Request.Login() { phone = _model.UserAccount, password = pwd.Pwd, lec_id = string.Empty, token = string.Empty };
            var t = await IPCHandle.doPost<Reponse.ResponseParam<Reponse.Login>>(Config.Interface_login, l.ReturnRequestParam());
            if (t == null || t.status != Config.SuccessCode)
            {
                MessageWindow.Alter("错误提示", t != null ? t.info : "登录失败，请检查服务是否正常");
            }
            else
            {
                ConfigManagerHelper.SetConfigByName(Config.IsRememberPwd, _model.RememblePwd.ToString());
                ConfigManagerHelper.SetConfigByName(Config.UserAccount, _model.RememblePwd ? _model.UserAccount : string.Empty);
                ConfigManagerHelper.SetConfigByName(Config.Password, _model.RememblePwd ? pwd.Pwd : string.Empty);
                ConfigManagerHelper.SetConfigByName(Config.IsAutoLogin, _model.AutoLogin.ToString());

                App.CurrentLogin = t.data;
                MainWindow win = new MainWindow();
                App.Current.MainWindow = win;
                win.Show();
                this.Close();
            }
        }
    }
    public class LoginModel : ViewModelBase
    {
        string _UserAccount;
        public string UserAccount
        {
            get { return _UserAccount; }
            set
            {
                _UserAccount = value;
                this.OnPropertyChanged("UserAccount");
            }
        }

        bool _RememblePwd;
        public bool RememblePwd
        {
            get { return _RememblePwd; }
            set
            {
                _RememblePwd = value;
                this.OnPropertyChanged("RememblePwd");
            }
        }
        bool _AutoLogin;
        public bool AutoLogin
        {
            get { return _AutoLogin; }
            set
            {
                _AutoLogin = value;
                this.OnPropertyChanged("AutoLogin");
            }
        }
    }
}
