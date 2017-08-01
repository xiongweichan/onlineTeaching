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

namespace TeacherClient
{
    /// <summary>
    /// SettingWIndow.xaml 的交互逻辑
    /// </summary>
    public partial class SettingWIndow : WindowBase
    {
        public SettingModel Model { get; set; }
        public SettingWIndow()
        {
            InitializeComponent();
            Model = new SettingModel();
            Model.UserName = App.CurrentLogin.user.nickname;
            UpdateModel();
            this.DataContext = this;
            this.IsBusy = false;
        }

        private void UpdateModel()
        {
            Model.IsAutoStart = App.IsAutoStartup;
            Model.CachePath = CacheHelper.CacheFilePath;
            Model.CacheCount = CacheHelper.GetCacheCount();
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            var wins = App.Current.Windows;
            LoginWindow win = new LoginWindow();
            win.FocusNotAutoLogin = true;
            win.Show();
            App.Current.MainWindow = win;
            foreach (Window item in wins)
                item.Close();
        }

        private void ApplySetting_Click(object sender, RoutedEventArgs e)
        {
            App.IsAutoStartup = Model.IsAutoStart;
            CacheHelper.CacheFilePath = Model.CachePath;
            Model.CacheCount = CacheHelper.GetCacheCount();
        }

        private void CancelSetting_Click(object sender, RoutedEventArgs e)
        {
            UpdateModel();
        }

        private void CheckNewVersion_Click(object sender, RoutedEventArgs e)
        {
            //TODO:检查更新
        }
    }
    public class SettingModel : ViewModelBase
    {
        string _UserName;
        public string UserName
        {
            get { return _UserName; }
            set
            {
                _UserName = value;
                this.OnPropertyChanged("UserName");
            }
        }

        bool _IsAutoStart;
        public bool IsAutoStart
        {
            get { return _IsAutoStart; }
            set
            {
                _IsAutoStart = value;
                this.OnPropertyChanged("IsAutoStart");
            }
        }
        string _CachePath;
        public string CachePath
        {
            get { return _CachePath; }
            set
            {
                _CachePath = value;
                this.OnPropertyChanged("CachePath");
            }
        }
        double _CacheCount;
        public double CacheCount
        {
            get { return _CacheCount; }
            set
            {
                _CacheCount = value;
                this.OnPropertyChanged("CacheCount");
            }
        }
    }
}
