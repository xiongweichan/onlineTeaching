using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeacherClient.Contract
{

    public class lesson
    {
        public string id { get; set; }
        public string course_id { get; set; }
        public string lesson_number { get; set; }
        public string start_time { get; set; }
        public string end_time { get; set; }
        [JsonProperty(PropertyName = "long")]
        public string Long { get; set; }
        public string province { get; set; }
        public string city { get; set; }
        public string district { get; set; }
        public string school { get; set; }
        public string address { get; set; }
        public string school_intro { get; set; }
        public string school_image { get; set; }
        public string num { get; set; }
        public string tel { get; set; }
        public string wechat { get; set; }
    }
}
