using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace NBSchool.WPFClient
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
            switch (item.ToString())
            {
                case "等待审核":
                    return WaitCheck;
                case "审核失败":
                    return CheckFailed;
                case "审核成功":
                    return CheckSuccess;
                case "直播结束":
                    return LiveEnd;
                case "等待直播":
                    return WaitLive;
            }
            return base.SelectTemplate(item, container);
        }
    }
}
