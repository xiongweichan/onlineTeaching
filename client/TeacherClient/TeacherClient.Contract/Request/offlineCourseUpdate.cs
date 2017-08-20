using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeacherClient.Contract.Request
{
    public class offlineCourseUpdate : ParamBase
    {
        public string id { get; set; }
        public string title { get; set; }
        public string intro { get; set; }
        public string course_type { get; set; }
        public string image { get; set; }
        public string content { get; set; }
        public string lessonList { get; set; }
    }
}
