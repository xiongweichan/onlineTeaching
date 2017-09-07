using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeacherClient.Contract
{
    public class courselesson
    {
        public string id { get; set; }
        public string lesson_number { get; set; }
        public string price { get; set; }
        public string video_file_name { get; set; }
        public string courseware_file_name { get; set; }
    }
}
