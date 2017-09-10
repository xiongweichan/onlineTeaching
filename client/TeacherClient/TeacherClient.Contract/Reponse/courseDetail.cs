using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeacherClient.Contract.Reponse
{
    public class courseDetail
    {
        public string id { get; set; }
        public string lecturer_id { get; set; }
        public string title { get; set; }
        public string intro { get; set; }
        public string image { get; set; }
        public string cat_id { get; set; }
        public string cat_id_1 { get; set; }
        public string cat_id_2 { get; set; }
        public string price { get; set; }
        public string status { get; set; }
        public string auth_time { get; set; }
        public string add_time { get; set; }
        public string audience { get; set; }
        public string course_type { get; set; }
        public string cat_name { get; set; }
        public string cat_name1 { get; set; }
        public string cat_name2 { get; set; }
        public List<courseDetailLesson> lessonList { get; set; }
    }

    public class courseDetailLesson
    {
        public string id { get; set; }
        public string course_id { get; set; }
        public string price { get; set; }
        public string video { get; set; }
        public string video_is_upload { get; set; }
        public string video_upload_time { get; set; }
        public string video_file_size { get; set; }
        public string video_file_name { get; set; }
        public string courseware { get; set; }
        public string courseware_is_upload { get; set; }
        public string courseware_upload_time { get; set; }
        public string courseware_file_size { get; set; }
        public string courseware_file_name { get; set; }
    }
}
