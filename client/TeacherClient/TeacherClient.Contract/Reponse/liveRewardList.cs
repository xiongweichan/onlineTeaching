using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeacherClient.Contract.Reponse
{
    public class liveRewardList
    {
        public int id { get; set; }
        public string lecturer_id { get; set; }
        public string live_id { get; set; }
        public string time { get; set; }
        public string title { get; set; }
        public string total_money { get; set; }
        public string total_num { get; set; }
        public string user_count { get; set; }
    }
}
