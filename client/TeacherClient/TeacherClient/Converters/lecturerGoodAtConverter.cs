using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace TeacherClient
{
    public class lecturerGoodAtConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (SystemInit.Instance.LecturerGoodAtList == null) return value;
            var v = SystemInit.Instance.LecturerGoodAtList.FirstOrDefault(T => T.id == value.ObjToString());
            if (v != null) return v.good_at_name;
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
