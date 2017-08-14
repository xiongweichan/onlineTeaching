using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeacherClient.Contract.Request
{
    public class requestlivelist : PageParam
    {
        public string is_first { get; set; }
        public string list_type { get; set; }
    }
}
