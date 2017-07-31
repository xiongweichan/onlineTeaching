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

namespace TeacherClient
{
    /// <summary>
    /// LoginWindow.xaml 的交互逻辑
    /// </summary>
    public partial class LoginWindow : WindowBase
    {
        /// <summary>
        /// 强制不允许自动登陆，退出登陆时可能用到
        /// </summary>
        public bool FocusNotAutoLogin { get; set; }

        public LoginWindow()
        {
            InitializeComponent();
            this.IsBusy = false;
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

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            Request.RequestParam r = new Request.RequestParam();
            Request.Login l = new Request.Login() { phone = "15989010000", password = "123456", lec_id="0", token= "登录返回的token令牌" };
            r.data = JsonConvert.SerializeObject(l);

            r.apisign = "c0b3519a7295596f9a815074bc0dcf5f"; 
            this.DoWork<Reponse.ResponseParam<Reponse.Login>>(() =>
                {
                    return IPCHandle.doPost<Reponse.ResponseParam<Reponse.Login>, Request.RequestParam>("Live/LecLogin/login", r);
                }, (t) =>
                {
                    if (t == null || t.status != Config.SuccessCode)
                    {
                        MessageWindow win = new MessageWindow();
                        win.Title = "登录失败";
                        win.Message = t != null ? t.info : "登录失败，请检查服务是否正常";
                        win.ShowDialog();
                    }
                    else
                    {
                        MainWindow win = new MainWindow();
                        win.Show();
                        this.Close();
                        App.Current.MainWindow = win;
                    }
                });
            MyLogin();
        }

        async void MyLogin()
        {


        }
    }
}
