using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeacherClient.Contract.Reponse
{
    public class liveGiftIndex
    {
        public List<gift> gift_list { get; set; }
        public string total_money { get; set; }
    }
    public class gift
    {
        public string gift_id { get; set; }
        public string gift_title { get; set; }
        public string gift_price { get; set; }
        public string gift_discount { get; set; }
        public string gift_image { get; set; }
        public string gift_num { get; set; }
    }
}
