using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeacherClient.Contract.Reponse
{
    public class region
    {
        public int id { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public List<region> data { get; set; }
    }
}
