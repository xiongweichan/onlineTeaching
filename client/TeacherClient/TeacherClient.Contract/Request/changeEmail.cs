using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeacherClient.Contract.Request
{
    public class changeEmail : ParamBase
    {
        public string email { get; set; }
        public string code { get; set; }
        public string email_new { get; set; }
        public string code_new { get; set; }
    }
}
