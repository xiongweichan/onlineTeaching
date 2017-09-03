using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace TeacherClient
{
    public class lecturerTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (SystemInit.Instance.LecturerTypeList == null) return value;
            var v = SystemInit.Instance.LecturerTypeList.FirstOrDefault(T => T.id == value.ObjToString());
            if (v != null) return v.lecturer_type_name;
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
