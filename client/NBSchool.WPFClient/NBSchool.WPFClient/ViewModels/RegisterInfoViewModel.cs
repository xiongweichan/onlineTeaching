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
    [PartCreationPolicy(CreationPolicy.NonShared)]
    [Export()]
    public class RegisterInfoViewModel:Conductor<IRegister>.Collection.OneActive, IRendering
    {
        protected override void OnActivate()
        {
            base.OnActivate();
            TelPhone = string.Empty;
            Certification = string.Empty;
            EMail = string.Empty;
            Password = string.Empty;
            RepeatPassword = string.Empty;
        }

        string _TelPhone;
        public string TelPhone
        {
            get { return _TelPhone; }
            set { this.Set(ref _TelPhone, value); }
        }

        string _Certification;
        public string Certification
        {
            get { return _Certification; }
            set { this.Set(ref _Certification, value); }
        }
        string _EMail;
        public string EMail
        {
            get { return _EMail; }
            set { this.Set(ref _EMail, value); }
        }
        string _Password;
        public string Password
        {
            get { return _Password; }
            set { this.Set(ref _Password, value); }
        }
        string _RepeatPassword;
        public string RepeatPassword
        {
            get { return _RepeatPassword; }
            set { this.Set(ref _RepeatPassword, value); }
        }

        public void btnNextStep()
        {
            IoC.Get<IRegister>().ToNext();
        }
    }
}
