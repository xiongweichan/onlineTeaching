using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeacherClient.Contract.Reponse
{
    public class bankcardDetail
    {
        public int id { get; set; }
        public string lecturer_id { get; set; }
        public string bank_type { get; set; }
        public string bank_card { get; set; }
        public string real_name { get; set; }
        public string id_card { get; set; }
        public string mobile { get; set; }
        public string is_default { get; set; }
        public string province { get; set; }
        public string city { get; set; }
        public string district { get; set; }
        public string address { get; set; }
    }
}
