using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeacherClient.Contract.Request
{
    public class ParamBase
    {
        public string lec_id { get; set; }
        public string token { get; set; }
    }
    public class PageParam : ParamBase
    {
        public int page { get; set; }
        public int pageSize { get; set; }
    }
}
