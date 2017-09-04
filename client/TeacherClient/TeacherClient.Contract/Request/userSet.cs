using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeacherClient.Contract.Request
{
    public class userSet : ParamBase
    {
        public string intro { get; set; }
        public string nickname { get; set; }
        public string realname { get; set; }
        public string sex { get; set; }
        public string age { get; set; }
        public string birth { get; set; }
        public string province { get; set; }
        public string city { get; set; }
        public string district { get; set; }
        public string lecturer_type { get; set; }
        public string good_at { get; set; }
        public string wechat { get; set; }
        public string job { get; set; }
        public string phone { get; set; }
        public string mail { get; set; }
    }
}
