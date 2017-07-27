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
    [Export()]
    public class ClassCenterViewModel : Conductor<IRendering>.Collection.OneActive, IRendering
    {
        public ClassCenterViewModel()
        {
            ShowClassManager = true;
        }

        bool _ShowClassManager;
        public bool ShowClassManager
        {
            get { return _ShowClassManager; }
            set { this.Set(ref _ShowClassManager, value); }
        }
        bool _ShowCoursewareManager;
        public bool ShowCoursewareManager
        {
            get { return _ShowCoursewareManager; }
            set { this.Set(ref _ShowCoursewareManager, value); }
        }
        bool _ShowMessageCenter;
        public bool ShowMessageCenter
        {
            get { return _ShowMessageCenter; }
            set { this.Set(ref _ShowMessageCenter, value); }
        }
    }
}
