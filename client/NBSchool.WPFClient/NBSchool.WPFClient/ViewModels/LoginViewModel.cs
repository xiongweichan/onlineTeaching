using Caliburn.Micro;
using MahApps.Metro.Controls;
using NBSchool.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MahApps.Metro.Controls.Dialogs;
using NBSchool.WPFClient.Views;

namespace NBSchool.WPFClient.ViewModels
{
    [Export(typeof(ILogin))]
    public class LoginViewModel : Conductor<IRendering>.Collection.OneActive, ILogin
    {
        string _UserAccount;
        public string UserAccount
        {
            get { return _UserAccount; }
            set { this.Set(ref _UserAccount, value); }
        }
        string _Password;
        public string Password
        {
            get { return _Password; }
            set { this.Set(ref _Password, value); }
        }
        MetroWindow _win;

        public LoginViewModel()
        {
            base.DisplayName = "邻居学院 登录窗口";
        }


        protected override void OnViewAttached(object view, object context)
        {
            base.OnViewAttached(view, context);
            _win = (MetroWindow)MetroWindow.GetWindow(view as DependencyObject);
        }

        public async void Login()
        {
            if (string.IsNullOrWhiteSpace(this.UserAccount) || string.IsNullOrWhiteSpace(this.Password))
            {
                await _win.ShowMessageAsync("提示", "用户名、密码不能为空！");
                return;
            }
            var controller = await _win.ShowProgressAsync("提示", "正在登录系统……");
            controller.SetIndeterminate();
            //TODO:登陆
            var main = IoC.Get<IMain>() as MainViewModel;
            IoC.Get<IWindowManager>().ShowWindow(main);
            var shellView = main.GetView() as MainView;
            if (shellView == null) Environment.Exit(0);
            shellView.Focus();
            shellView.Activate();
            await controller.CloseAsync();
            this.TryClose();
        }

        public void ForgetPassword()
        {
            //TODO:忘记密码
        }

        public void RegisterAccount()
        {
            object obj = IoC.Get<IRegister>();
            IoC.Get<IWindowManager>().ShowDialog(obj);
        }

    }
}
