using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using Reponse = TeacherClient.Contract.Reponse;

namespace TeacherClient
{
    public class GiftToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is List<Reponse.gift>)
            {
                var gift = value as List<Reponse.gift>;
                return string.Join(" ", gift.Select(T => string.Format("{0}{1}", T.gift_title, T.gift_num)));
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
