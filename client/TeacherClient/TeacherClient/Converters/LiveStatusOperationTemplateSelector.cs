﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace TeacherClient
{
    public class LiveStatusOperationTemplateSelector : DataTemplateSelector
    {
        public DataTemplate WaitCheck { get; set; }
        public DataTemplate CheckFailed { get; set; }
        public DataTemplate CheckSuccess { get; set; }
        public DataTemplate LiveEnd { get; set; }
        public DataTemplate WaitLive { get; set; }
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var live = item as Contract.Reponse.live;
            if (live != null)
                switch (live.status.ObjToString())
                {
                    case "0":
                        return WaitCheck;
                    case "2":
                        return CheckFailed;
                    case "1":
                        switch (live.time_status.ObjToString())
                        {
                            case "0":
                                return WaitLive;
                            case "1":
                                return WaitLive;
                            case "2":
                                return LiveEnd;
                        }
                        return CheckSuccess;
                }
            return base.SelectTemplate(item, container);
        }
    }
}
