using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeacherClient.Contract.Request
{
    public class liveAdd : ParamBase
    {
        public string title { get; set; }
        public string image { get; set; }
        public string cat_id { get; set; }
        public string cat_id_1 { get; set; }
        public string cat_id_2 { get; set; }
        public string is_first { get; set; }
        public string intro { get; set; }
        public string syllabus { get; set; }
        public string price { get; set; }
        public string relate_live_id { get; set; }
        public string start_time { get; set; }
        public string end_time { get; set; }
        public string courseware { get; set; }
    }
}
