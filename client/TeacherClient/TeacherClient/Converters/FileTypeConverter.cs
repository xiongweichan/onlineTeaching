using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace TeacherClient
{
    public class FileTypeConverter : IValueConverter
    {
        public static string GetFileType(string name)
        {
            switch (name.ToLower())
            {
                case "doc":
                case "docx":
                case "txt":
                case "xls":
                case "xlsx":
                case "ppt":
                case "pptx":
                    return "文档";
                case "png":
                case "jpg":
                case "bmp":
                    return "图片";
                case "mp3":
                case "mp4":
                case "avi":
                case "rmvb":
                    return "音视频";
            }
            return name;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return GetFileType(value.ObjToString());
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
