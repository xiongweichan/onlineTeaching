using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeacherClient.Contract.Request
{
    public class resetPwd : ParamBase
    {
        public string phone { get; set; }
        public string password { get; set; }
        public string code { get; set; }
    }
}
