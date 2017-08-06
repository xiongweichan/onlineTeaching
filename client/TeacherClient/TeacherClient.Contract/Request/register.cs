using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeacherClient.Contract.Request
{
    public class register : ParamBase
    {
        public string phone { get; set; }
        public string password { get; set; }
        public string code { get; set; }
        public string email { get; set; }
        public string id_card_front { get; set; }
        public string id_card_back { get; set; }
        public string body_photo { get; set; }
        public string qualification_cert { get; set; }
    }
}
