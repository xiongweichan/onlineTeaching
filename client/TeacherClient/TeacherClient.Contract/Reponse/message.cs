using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeacherClient.Contract.Reponse
{
    public class message
    {
        public string id { get; set; }
        public string type { get; set; }
        public string title { get; set; }
        public string summary { get; set; }
        public string content { get; set; }
        public string add_time { get; set; }
        public string lecturer_id { get; set; }
        public string status { get; set; }
    }
}
