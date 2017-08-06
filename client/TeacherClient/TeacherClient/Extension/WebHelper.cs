using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TeacherClient.Core;
using Reponse = TeacherClient.Contract.Reponse;
using Request = TeacherClient.Contract.Request;

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

        public async static Task<T1> doPost<T1, T2>(string address, T2 param)
        {
            var t = await IPCHandle.doPost<Reponse.ResponseParam<T1>>(address, param.ReturnRequestParam());
            if (t == null || t.status != Config.SuccessCode)
            {
                Telerik.Windows.Controls.ViewModelBase.InvokeOnUIThread(() =>
                {
                    MessageWindow win = new MessageWindow();
                    win.Title = "错误";
                    win.Message = t == null ? "服务器连接失败！" : t.info;
                    win.ShowDialog();
                });
                return default(T1);
            }
            else return t.data;
        }
    }
}
