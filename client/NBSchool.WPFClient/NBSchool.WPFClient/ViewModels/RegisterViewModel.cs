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
    [Export(typeof(IRegister))]
    public class RegisterViewModel : Conductor<IRendering>.Collection.OneActive, IRegister
    {
        RegisterInfoViewModel _registerInfoVM;
        CertificationViewModel _certificationVM;
        RegisterCompletedViewModel _registerCompletedVM;

        public void ToNext()
        {
            if(this.ActiveItem == null)
                ActiveItem = _registerInfoVM;
            else if (this.ActiveItem is RegisterInfoViewModel)
                this.ActiveItem = _certificationVM;
            else if (this.ActiveItem is CertificationViewModel)
                this.ActiveItem = _registerCompletedVM;
            else
            {
                this.TryClose(true);
            }
        }

        public RegisterViewModel()
        {
        }

        protected override void OnActivate()
        {
            base.OnActivate();
            _registerInfoVM = IoC.Get<RegisterInfoViewModel>();
            _certificationVM = IoC.Get<CertificationViewModel>();
            _registerCompletedVM = IoC.Get<RegisterCompletedViewModel>();
            ActiveItem = null;
            ToNext();
        }
        
    }
}
