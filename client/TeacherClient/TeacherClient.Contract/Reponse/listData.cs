using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeacherClient.Contract.Reponse
{
    public class listData<T>
    {
        public int totalCount { get; set; }
        public List<T> list { get; set; }
    }
}
