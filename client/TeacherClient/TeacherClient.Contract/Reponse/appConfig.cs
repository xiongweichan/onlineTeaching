using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeacherClient.Contract.Reponse
{
    public class appConfig
    {
        /// <summary>
        /// 邻居学院用户保密协议
        /// </summary>
        public string LEC_REGISTER_PRIVATE { get; set; }
        /// <summary>
        /// 邻居学院讲师注册协议
        /// </summary>
        public string LEC_REGISTER_SERVICE { get; set; }
        /// <summary>
        /// 讲师端帮助(个人界面)
        /// </summary>
        public string LEC_HELP { get; set; }
    }
}
