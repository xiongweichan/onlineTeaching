using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeacherClient.Contract.Request
{
    public class withdrawAdd : ParamBase
    {
        public string money { get; set; }
        public string remark { get; set; }
    }
}
