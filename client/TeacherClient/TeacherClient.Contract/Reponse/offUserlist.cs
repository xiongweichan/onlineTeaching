using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeacherClient.Contract.Reponse
{
    public class offUserlist
    {
        public int id { get; set; }
        public string course_id { get; set; }
        public string lesson_number { get; set; }
        public string user_id { get; set; }
        public string name { get; set; }
        public string phone { get; set; }
        public string desc { get; set; }
        public string status { get; set; }
        public string add_time { get; set; }
    }
}
