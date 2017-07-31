using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeacherClient.Contract.Reponse
{
    public class ResponseParam<T>
    {
        public int status { get; set; }
        public string info { get; set; }
        public T data { get; set; }
        public long time { get; set; }
    }
}
