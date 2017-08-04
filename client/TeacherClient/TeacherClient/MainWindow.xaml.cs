﻿using System;
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
using TeacherClient.Pages;
using Telerik.Windows.Controls;
using Reponse = TeacherClient.Contract.Reponse;
using Request = TeacherClient.Contract.Request;

namespace TeacherClient
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : WindowBase
    {
        public MainWindow()
        {
            Current = this;

            InitializeComponent();
            Init();
        }
        async void Init()
        {
            Model = new UserModel();
            Request.ParamBase l = new Request.ParamBase() { lec_id = App.CurrentLogin.lec_id, token = App.CurrentLogin.token };
            var t = await IPCHandle.doPost<Reponse.ResponseParam<Reponse.MainIndex>>(Config.Interface_mainIndex, l.ReturnRequestParam());


            Model.UserAccount = App.CurrentLogin.user.phone;
            Model.MessageCount = t.data.unreadCount;
            Model.Version = App.CurrentVersion;

            this.DataContext = this;
            this.IsBusy = false;
        }

        public UserModel Model { get; set; }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow win = new LoginWindow(true);
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
        MainIndex _mainIndex;
        LiveCenter _liveCenter;
        CourseCenter _courseCenter;
        OfflineCourse _offlineCourse;
        RewardCenter _rewardCenter;
        private void StackPanel_Checked(object sender, RoutedEventArgs e)
        {
            switch (Model.MenuIndex)
            {
                case 0:
                    if (_mainIndex == null)
                        _mainIndex = new MainIndex();
                    frame.Content = _mainIndex;
                    break;
                case 1:
                    if (_liveCenter == null)
                        _liveCenter = new LiveCenter();
                    frame.Content = _liveCenter;
                    break;
                case 2:
                    if (_courseCenter == null)
                        _courseCenter = new CourseCenter();
                    frame.Content = _courseCenter;
                    break;
                case 3:
                    if (_offlineCourse == null)
                        _offlineCourse = new OfflineCourse();
                    frame.Content = _offlineCourse;
                    break;
                case 4:
                    if (_rewardCenter == null)
                        _rewardCenter = new RewardCenter();
                    frame.Content = _rewardCenter;
                    break;
            }
        }
        public static MainWindow Current { get; private set; }
        public void NavigateToPage(int findex, int sindex, object data)
        {
            Model.MenuIndex = findex;
            (frame.Content as INavigation).NavigateToPage(sindex, data);
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
        string _MessageCount;
        public string MessageCount
        {
            get { return _MessageCount; }
            set
            {
                _MessageCount = value;
                this.OnPropertyChanged("MessageCount");
            }
        }
        string _Version;
        public string Version
        {
            get { return _Version; }
            set
            {
                _Version = value;
                this.OnPropertyChanged("Version");
            }
        }

        int _MenuIndex;
        public int MenuIndex
        {
            get { return _MenuIndex; }
            set
            {
                _MenuIndex = value;
                this.OnPropertyChanged("MenuIndex");
            }
        }
    }
}
