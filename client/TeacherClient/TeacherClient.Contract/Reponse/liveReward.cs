using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeacherClient.Contract.Reponse
{
    public class liveReward
    {
        public string reward_money { get; set; }
        public string gift_num { get; set; }
        public string user_num { get; set; }
        public List<commentmodel> commentList { get; set; }
    }
    public class commentmodel
    {
        public string id { get; set; }
        public string user_id { get; set; }
        public string nickname { get; set; }
        public string live_id { get; set; }
        public string comment { get; set; }
        public string add_time { get; set; }
    }
}
