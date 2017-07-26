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
    public class TeacherIndexViewModel : Conductor<IRendering>.Collection.OneActive, IRendering
    {
        public TeacherIndexViewModel()
        {
            this.ShowSummaryInfo = true;
        }


        void ChangeView()
        {
            if (this.ShowSummaryInfo)
                this.ActiveItem = IoC.Get<TeacherIndexSummaryInfoViewModel>();
            else if (this.ShowTeacherInfo)
                this.ActiveItem = IoC.Get<TeacherIndexTeacherInfoViewModel>();


        }
        #region
        bool _ShowSummaryInfo;
        public bool ShowSummaryInfo
        {
            get { return _ShowSummaryInfo; }
            set { this.Set(ref _ShowSummaryInfo, value); if (value) ChangeView(); }
        }
        bool _ShowTeacherInfo;
        public bool ShowTeacherInfo
        {
            get { return _ShowTeacherInfo; }
            set { this.Set(ref _ShowTeacherInfo, value); if (value) ChangeView(); }
        }
        bool _ShowCertification;
        public bool ShowCertification
        {
            get { return _ShowCertification; }
            set { this.Set(ref _ShowCertification, value); if (value) ChangeView(); }
        }
        #endregion
    }
}
