using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeacherClient.Contract.Request
{
    public class checkEmailCode:ParamBase
    {
        public string email { get; set; }
        public string code { get; set; }
        public string type { get; set; }
    }
}
