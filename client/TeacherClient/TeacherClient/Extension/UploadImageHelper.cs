using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeacherClient.Core;
using Reponse = TeacherClient.Contract.Reponse;
using Request = TeacherClient.Contract.Request;

namespace TeacherClient
{
    public class UploadImageHelper
    {

        public static async Task<string> UploadImage()
        {
            try
            {
                var path = FileSelectHelper.FileSelecte("图片文件|*.png;*.bmp;*.jpg", 2);
                if (!string.IsNullOrWhiteSpace(path))
                {
                    byte[] bys = File.ReadAllBytes(path);
                    var str = Convert.ToBase64String(bys);
                    Request.imageUpLoad l = new Request.imageUpLoad() { lec_id = App.CurrentLogin.lec_id, token = App.CurrentLogin.token, upload = str };
                    var t = await WebHelper.doPost<Reponse.imageUpLoad, Request.imageUpLoad>(Config.Interface_imageUpLoad, l);
                    return t.url;
                }
            }
            catch (InvalidOperationException)
            {
                MessageWindow.Alter("提示", "未选中文件");
            }
            catch (IOException)
            {
                MessageWindow.Alter("提示", "文件正由另一进程使用，因此该进程无法访问此文件。");
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
            
            return string.Empty;
        }
    }

    public class FileSelectHelper
    {
        public static string FileSelecte(string filter, int max)
        {
            try
            {
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.Multiselect = false;
                dialog.Filter = filter;
                if (dialog.ShowDialog() == true)
                {
                    var path = dialog.FileName;
                    var s = dialog.OpenFile();
                    if (s.Length == 0)
                    {
                        MessageWindow.Alter("提示", "文件大小不能为0");
                        return string.Empty;
                    }
                    else if (s.Length > max * 1024 * 1024)
                    {
                        MessageWindow.Alter("提示", "文件大小超过允许的范围");
                        return string.Empty;
                    }
                    return path;
                }
            }
            catch (InvalidOperationException)
            {
                MessageWindow.Alter("提示", "未选中文件");
            }
            catch (IOException)
            {
                MessageWindow.Alter("提示", "文件正由另一进程使用，因此该进程无法访问此文件。");
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
            
            return string.Empty;
        }
    }
}
