using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeacherClient.Contract.Request
{
    public class Login : ParamBase
    {
        public string phone { get; set; }
        public string password { get; set; }
    }
}
