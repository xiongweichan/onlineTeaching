using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeacherClient.Contract.Request
{
    public class messageDel : RequestID
    {
        /// <summary>
        /// (必须)(1:学员消息,2:学员留言)
        /// </summary>
        public string type { get; set; }
    }
}
