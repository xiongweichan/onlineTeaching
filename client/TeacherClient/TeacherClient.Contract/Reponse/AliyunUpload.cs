using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeacherClient.Contract.Reponse
{
    public class AliyunUpload
    {
        public string access_key_id { get; set; }
        public string access_key_secret { get; set; }
        public string oss_end_point { get; set; }
        public string oss_bucket { get; set; }
        public string oss_domain { get; set; }
        public string key { get; set; }
    }
}
