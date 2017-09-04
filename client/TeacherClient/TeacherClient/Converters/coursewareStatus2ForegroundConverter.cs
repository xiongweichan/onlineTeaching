using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace TeacherClient
{
    public class coursewareStatus2ForegroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch (value.ObjToString())
            {
                case "0":
                    return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#424e67"));
                case "1":
                    return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#424e67"));
                case "2":
                    return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ff2501"));
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
