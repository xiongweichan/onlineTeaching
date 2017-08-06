using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeacherClient.Contract.Reponse
{
    public class withdrawList
    {
        public int id { get; set; }
        public string lecturer_id { get; set; }
        public string money { get; set; }
        public string add_time { get; set; }
        public string bank { get; set; }
        public string bank_card { get; set; }
        public string is_send { get; set; }
        public string transaction_sn { get; set; }
        public string real_name { get; set; }
        public string mobile { get; set; }
        public string remark { get; set; }
    }
}
