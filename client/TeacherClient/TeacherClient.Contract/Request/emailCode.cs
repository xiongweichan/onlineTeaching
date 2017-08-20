using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeacherClient.Contract.Request
{
    public class emailCode : ParamBase
    {
        public string email { get; set; }
        public string type { get; set; }
    }
}
