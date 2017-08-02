using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeacherClient.Contract.Reponse
{
    public class Login
    {
        public string lec_id { get; set; }
        public string token { get; set; }
        public userInfo user { get; set; }
    }
}
