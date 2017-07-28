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
    [Export(typeof(IForgetPassword))]
    public class ForgetPasswordViewModel : Conductor<IRendering>.Collection.OneActive, IForgetPassword
    {
        public ForgetPasswordViewModel()
        {
        }
        bool _ShowFirstPage;
        public bool ShowFirstPage
        {
            get { return _ShowFirstPage; }
            set { this.Set(ref _ShowFirstPage, value); }
        }
        bool _ShowSecondPage;
        public bool ShowSecondPage
        {
            get { return _ShowSecondPage; }
            set { this.Set(ref _ShowSecondPage, value); }
        }
        bool _ShowThirdPage;
        public bool ShowThirdPage
        {
            get { return _ShowThirdPage; }
            set { this.Set(ref _ShowThirdPage, value); }
        }

        protected override void OnActivate()
        {
            ShowFirstPage = true;
            base.OnActivate();
        }
        public void ToNewPassword()
        {
            ShowSecondPage = true;
        }
        public void BtnOK()
        {
            ShowThirdPage = true;
        }
        public void BtnSure()
        {
           
        }
    }
}
