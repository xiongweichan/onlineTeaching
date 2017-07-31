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

namespace TeacherClient
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : WindowBase
    {
        public MainWindow()
        {
            InitializeComponent();
            Model = new UserModel();
            Model.UserAccount = "test";
            Model.MessageCount = 10;
            this.DataContext = this;
            this.IsBusy = false;
        }
        public UserModel Model { get; set; }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow win = new LoginWindow();
            win.FocusNotAutoLogin = true;
            win.Show();
            App.Current.MainWindow = win;
            this.Close();
        }

        private void StackPanel_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            popup_Logout.IsOpen = true;
        }

        private void OpenSetting_Click(object sender, RoutedEventArgs e)
        {
            SettingWIndow win = new SettingWIndow();
            win.ShowDialog();
        }
    }

    public class UserModel : ViewModelBase
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
        int _MessageCount;
        public int MessageCount
        {
            get { return _MessageCount; }
            set
            {
                _MessageCount = value;
                this.OnPropertyChanged("MessageCount");
            }
        }
    }
}
