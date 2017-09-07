using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeacherClient.Contract.Request
{
    public class courseAdd : ParamBase
    {
        public string title { get; set; }
        public string intro { get; set; }
        public string image { get; set; }
        public string cat_id { get; set; }
        public string cat_id_1 { get; set; }
        public string cat_id_2 { get; set; }
        public string course_type { get; set; }
        public List<courselesson> lessonList { get; set; }
    }
}
