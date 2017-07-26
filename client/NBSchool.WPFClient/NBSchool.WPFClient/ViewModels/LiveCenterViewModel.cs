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
    public class LiveCenterViewModel : Conductor<IRendering>.Collection.OneActive, IRendering
    {
        public LiveCenterViewModel()
        {
            ShowMyLive = true;
        }


        bool _ShowMyLive;
        public bool ShowMyLive
        {
            get { return _ShowMyLive; }
            set
            {
                this.Set(ref _ShowMyLive, value);
                if (value)
                    this.ActiveItem = IoC.Get<LiveCenterMyLiveViewModel>();
            }
        }

        bool _ShowRequestLive;
        public bool ShowRequestLive
        {
            get { return _ShowRequestLive; }
            set
            {
                this.Set(ref _ShowRequestLive, value);
                if (value)
                    this.ActiveItem = IoC.Get<LiveCenterRequestViewModel>();
            }
        }
    }
}
