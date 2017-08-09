using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeacherClient.Contract.Reponse
{
    public class lessonList
    {
        public int id { get; set; }
        public string course_id { get; set; }
        public string lesson_number { get; set; }
        public string start_time { get; set; }
        public string end_time { get; set; }
        [JsonProperty(PropertyName = "long")]
        public string Long { get; set; }
        public string num { get; set; }
        public string user_num { get; set; }
        public string add_time { get; set; }
        public string lecturer_id { get; set; }
        public string title { get; set; }
        public string course_type { get; set; }
        public string time_status { get; set; }
    }
}
