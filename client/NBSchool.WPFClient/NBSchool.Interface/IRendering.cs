using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBSchool.Interface
{
    public interface IRendering
    {
        /// <summary>
        /// 页面被切换隐藏后的通知事件
        /// </summary>
        /// <param name="activateViewModel">要切换到的页面</param>
        /// <param name="deActivateViewModel">被切换出的页面</param>
        //void DeActivateView(object activateViewModel, object deActivateViewModel);
    }
}
