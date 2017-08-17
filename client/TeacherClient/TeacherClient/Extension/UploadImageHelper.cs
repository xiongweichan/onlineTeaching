using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reponse = TeacherClient.Contract.Reponse;
using Request = TeacherClient.Contract.Request;

namespace TeacherClient
{
    public class UploadImageHelper
    {

        public static async Task<string> UploadImage()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Multiselect = false;
            dialog.Filter = "图片文件|*.png;*.bmp;*.jpg";
            if (dialog.ShowDialog() == true)
            {
                var path = dialog.FileName;
                var s = dialog.OpenFile();
                if (s.Length > 50 * 1024)
                {
                    MessageWindow.Alter("提示", "图片过大");
                    return string.Empty;
                }
                byte[] bys = new byte[s.Length];
                s.Read(bys, 0, (int)s.Length);
                var str = Convert.ToBase64String(bys);
                Request.imageUpLoad l = new Request.imageUpLoad() { lec_id = App.CurrentLogin.lec_id, token = App.CurrentLogin.token, upload = str };
                var t = await WebHelper.doPost<Reponse.imageUpLoad, Request.imageUpLoad>(Config.Interface_imageUpLoad, l);
                return t.url;
            }
            return string.Empty;
        }
    }
}
