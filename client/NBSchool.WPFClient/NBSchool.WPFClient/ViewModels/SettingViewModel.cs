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
    [Export(typeof(ISetting))]
    class SettingViewModel : Conductor<IRendering>.Collection.OneActive, ISetting
    {
        bool _ShowAccountSetting;
        public bool ShowAccountSetting
        {
            get { return _ShowAccountSetting; }
            set { this.Set(ref _ShowAccountSetting, value); }
        }
        bool _ShowCommonSetting;
        public bool ShowCommonSetting
        {
            get { return _ShowCommonSetting; }
            set { this.Set(ref _ShowCommonSetting, value); }
        }
        bool _ShowAboutSetting;
        public bool ShowAboutSetting
        {
            get { return _ShowAboutSetting; }
            set { this.Set(ref _ShowAboutSetting, value); }
        }
        string _UserName;
        public string UserName
        {
            get { return _UserName; }
            set { this.Set(ref _UserName, value); }
        }
        int _CacheCount;
        public int CacheCount
        {
            get { return _CacheCount; }
            set { this.Set(ref _CacheCount, value); }
        }

        string _Version;
        public string Version
        {
            get { return _Version; }
            set { this.Set(ref _Version, value); }
        }
    }
}
