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
            Model.UserName = App.CurrentLogin.user.realname;
            Model.Image = App.CurrentLogin.user.head;
            UpdateModel();
            this.DataContext = this;
            this.IsBusy = false;
        }

        private void UpdateModel()
        {
            Model.IsAutoStart = App.IsAutoStartup;
            Model.CachePath = CacheHelper.CacheFilePath;
            Model.CacheCount = CacheHelper.GetCacheCount().ToString("f1") + "M";
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            if (MessageWindow.Alter("询问", "是否确定退出？") != true) return;
            var wins = App.Current.Windows;
            LoginWindow win = new LoginWindow(true);
            win.Show();
            App.Current.MainWindow = win;
            foreach (Window item in wins)
                item.Close();
        }

        private void ApplySetting_Click(object sender, RoutedEventArgs e)
        {
            App.IsAutoStartup = Model.IsAutoStart;
            CacheHelper.CacheFilePath = Model.CachePath;
            Model.CacheCount = CacheHelper.GetCacheCount().ToString("f1") + "M";
        }

        private void CancelSetting_Click(object sender, RoutedEventArgs e)
        {
            UpdateModel();
        }

        private void CheckNewVersion_Click(object sender, RoutedEventArgs e)
        {
            //TODO:检查更新
        }

        private void Btn_ClearCacheClick(object sender, RoutedEventArgs e)
        {
            CacheHelper.ClearCache();
            var d = CacheHelper.GetCacheCount();
            Model.CacheCount = d.ToString("f1") + "M";
            if (d > 0)
                MessageWindow.Alter("提示", "清空完成，部分缓存正在使用，无法清除。");
            else
                MessageWindow.Alter("提示", "清空完成。");
        }

        private void ChangedDir_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog();
            if(dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Model.CachePath = dialog.SelectedPath;
                Model.CacheCount = CacheHelper.GetCacheCount().ToString("f1") + "M";
            }
        }
    }
    public class SettingModel : ViewModelBase
    {
        string _Image;
        public string Image
        {
            get { return _Image; }
            set
            {
                _Image = value;
                this.OnPropertyChanged("Image");
            }
        }

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
        string _CacheCount;
        public string CacheCount
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
