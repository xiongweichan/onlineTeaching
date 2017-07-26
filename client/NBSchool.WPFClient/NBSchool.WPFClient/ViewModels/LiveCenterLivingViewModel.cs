using Caliburn.Micro;
using NBSchool.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBSchool.WPFClient.ViewModels
{
    public class LiveCenterLivingViewModel : Conductor<IRendering>.Collection.OneActive, IRendering
    {
    }
}
