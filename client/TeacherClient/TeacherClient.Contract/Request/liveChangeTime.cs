using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeacherClient.Contract.Request
{
    public class liveChangeTime : ParamBase
    {
        public string id { get; set; }
        public string start_time { get; set; }
        public string end_time { get; set; }
    }
}
