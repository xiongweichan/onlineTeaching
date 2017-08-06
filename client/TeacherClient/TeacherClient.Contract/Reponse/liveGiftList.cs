using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeacherClient.Contract.Reponse
{
    public class liveGiftList
    {
        public int id { get; set; }
        public string lecturer_id { get; set; }
        public string live_id { get; set; }
        public string time { get; set; }
        public string title { get; set; }
        public string total_num { get; set; }
        public List<gift> gift_list { get; set; }
        public string total_money { get; set; }
        public string end_long { get; set; }
    }
}
