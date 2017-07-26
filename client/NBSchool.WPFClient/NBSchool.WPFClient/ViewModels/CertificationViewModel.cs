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
    public class CertificationViewModel : Conductor<IRegister>.Collection.OneActive, IRendering
    {

        public void CompletedRegister()
        {
            IoC.Get<IRegister>().ToNext();
        }
    }
}
