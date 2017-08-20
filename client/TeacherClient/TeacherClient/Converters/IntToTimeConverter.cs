using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace TeacherClient
{
    public class IntToTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                return value.ObjToString().GetTime().ToString(parameter.ObjToString());
            }
            catch (Exception ex)
            {
                TeacherClient.Core.Log.Error("解析时间失败", ex);
                return "";
            }

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                DateTime time;
                return DateTime.TryParse(value.ObjToString(), out time) ? time.ConvertDateTimeInt() : 0;
            }
            catch (Exception ex)
            {
                TeacherClient.Core.Log.Error("解析时间失败", ex);
                return value;
            }
        }


    }
}
