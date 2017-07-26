using Caliburn.Micro;
using NBSchool.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBSchool.WPFClient.ViewModels
{
    [Export(typeof(IMain))]
    public class MainViewModel : Conductor<IRendering>.Collection.OneActive, IMain
    {
        string _UserAccount;
        public string UserAccount
        {
            get { return _UserAccount; }
            set { this.Set(ref _UserAccount, value); }
        }
        int _MessageCount;
        public int MessageCount
        {
            get { return _MessageCount; }
            set { this.Set(ref _MessageCount, value); }
        }

        public MainViewModel()
        {
            UserAccount = "18602043866";
            MessageCount = 5;

            ShowTeacherIndex = true;
        }

        public void OpenSetting()
        {
            IoC.Get<IWindowManager>().ShowDialog(IoC.Get<ISetting>());
        }

        void ChangeView()
        {
            if (this.ShowTeacherIndex)
                this.ActiveItem = IoC.Get<TeacherIndexViewModel>();
            else if (this.ShowLiveCenter)
                this.ActiveItem = IoC.Get<LiveCenterViewModel>();

            else
                this.ActiveItem = null;
        }

        #region 页面切换
        bool _ShowTeacherIndex;
        public bool ShowTeacherIndex
        {
            get { return _ShowTeacherIndex; }
            set { this.Set(ref _ShowTeacherIndex, value); ChangeView(); }
        }
        bool _ShowLiveCenter;
        public bool ShowLiveCenter
        {
            get { return _ShowLiveCenter; }
            set { this.Set(ref _ShowLiveCenter, value); ChangeView(); }
        }
        bool _ShowClassCenter;
        public bool ShowClassCenter
        {
            get { return _ShowClassCenter; }
            set { this.Set(ref _ShowClassCenter, value); ChangeView(); }
        }
        bool _ShowOfflineClass;
        public bool ShowOfflineClass
        {
            get { return _ShowOfflineClass; }
            set { this.Set(ref _ShowOfflineClass, value); ChangeView(); }
        }
        bool _ShowRewardCenter;
        public bool ShowRewardCenter
        {
            get { return _ShowRewardCenter; }
            set { this.Set(ref _ShowRewardCenter, value); ChangeView(); }
        }
        #endregion
    }
}
