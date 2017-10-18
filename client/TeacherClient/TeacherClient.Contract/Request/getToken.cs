using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeacherClient.Contract.Request
{
    public class getToken : ParamBase
    {
        public string file_name { get; set; }
        public string id { get; set; }
        public string aliyun { get; set; }
    }
}
