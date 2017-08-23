using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeacherClient.Contract.Request
{
    public class coursewareAdd : ParamBase
    {
        public string title { get; set; }
        public string intro { get; set; }
        public string cat_id { get; set; }
        public string cat_id_1 { get; set; }
        public string cat_id_2 { get; set; }
        public string price { get; set; }
        public string url { get; set; }
    }
}
