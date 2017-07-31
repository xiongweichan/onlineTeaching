using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeacherClient.Core
{
    public class HttpNetUtils
    {
        public static string MapToUrl(Dictionary<string, object> map)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in map)
            {
                string keyvalue = string.Format("&{0}={1}", item.Key, item.Value.ToString());
                sb.Append(keyvalue);
            }
            return sb.ToString();
        }
    }
}
