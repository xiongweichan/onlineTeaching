using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeacherClient.Contract.Request
{
    public class RequestListByID : PageParam
    {
        public string id { get; set; }
    }
}
