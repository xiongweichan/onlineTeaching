using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeacherClient.Contract.Reponse
{
    public class category
    {
        public string id { get; set; }
        public string p_id { get; set; }
        public string name { get; set; }
        public string image { get; set; }
        public List<category> data { get; set; }
    }
}
