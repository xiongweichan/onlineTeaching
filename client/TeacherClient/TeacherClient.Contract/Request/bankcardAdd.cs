using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeacherClient.Contract.Request
{
    public class bankcardAdd : ParamBase
    {
        public string bank_type { get; set; }
        public string bank_card { get; set; }
        public string real_name { get; set; }
        public string id_card { get; set; }
        public string mobile { get; set; }
        public string province { get; set; }
        public string city { get; set; }
        public string bank_site { get; set; }
        public string code { get; set; }
    }
}
