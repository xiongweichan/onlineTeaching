using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeacherClient.Contract.Request
{
    public class AliyunUploadCompleted : ParamBase
    {
        public string id { get; set; }
        public string key { get; set; }
    }
    public class AliyunUploadCompleted_Course : AliyunUploadCompleted
    {
        public string type { get; set; }
    }
}
