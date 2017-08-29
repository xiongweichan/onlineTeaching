using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeacherClient.Contract.Request
{
    public class coursewareChangeTitle : RequestID
    {
        public string title { get; set; }
    }
}
