using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeacherClient.Contract.Request
{
    public class changePhone : ParamBase
    {
        public string phone { get; set; }
        public string code { get; set; }
        public string phone_new { get; set; }
        public string code_new { get; set; }
    }
}
