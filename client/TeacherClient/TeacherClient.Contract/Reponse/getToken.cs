using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeacherClient.Contract.Reponse
{
    public class getToken
    {
        public string domain { get; set; }
        public string token { get; set; }
        public string key { get; set; }
        public Aliyun aliyun { get; set; }
    }
    public class Aliyun
    {
        public string accessid { get; set; }
        public string host { get; set; }
        public string policy { get; set; }
        public string signature { get; set; }
        public string expire { get; set; }
        public string callback { get; set; }
        public string dir { get; set; }
    }
}
