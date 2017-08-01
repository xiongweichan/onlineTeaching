using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace TeacherClient
{
    public static class WebHelper
    {
        public static IEnumerable<KeyValuePair<string, string>> ReturnRequestParam<T>(this T request)
        {
            var data = JsonConvert.SerializeObject(request);
            var s = "dbcf94929378b8a98bf7efb2d44d5c8a" + data;
            MD5 md5 = MD5.Create();
            var ss = md5.ComputeHash(Encoding.UTF8.GetBytes(s));
            var apisign = ToHexString(md5.Hash);

            return new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("data", data),
                new KeyValuePair<string, string>("apisign", apisign),
            };
        }

        static string ToHexString(byte[] bys)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in bys)
            {
                sb.Append(item.ToString("x2"));
            }
            return sb.ToString();
        }
    }
}
