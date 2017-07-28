using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace TeacherClient
{
    public class IntToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                int i, j;
                if (int.TryParse(value.ObjToString(), out i) && int.TryParse(parameter.ObjToString(), out j) && i > j)
                    return Visibility.Visible;
            }
            catch (Exception) { }
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (value != null && value.ToString() == Visibility.Visible.ToString())
                    return 1;
            }
            catch (Exception) { }
            return 0;
        }
    }
}
