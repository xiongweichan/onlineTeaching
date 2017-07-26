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
    public class RegisterCompletedViewModel: Conductor<IRegister>.Collection.OneActive, IRendering
    {
        public void BackToLogin()
        {
            IoC.Get<IRegister>().ToNext();
        }
    }
}
