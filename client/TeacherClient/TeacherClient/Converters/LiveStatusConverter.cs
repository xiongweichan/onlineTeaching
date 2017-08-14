using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace TeacherClient
{
    public class LiveStatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var live = value as Contract.Reponse.live;
            if (live != null)
                switch (live.status.ObjToString())
                {
                    case "0":
                        return "等待审核";
                    case "2":
                        return "审核失败";
                    case "1":
                        if (live.start_time != null && live.start_time.GetTime().AddHours(48) > DateTime.Now)
                            return "等待直播";
                        else if (live.end_time != null && live.end_time.GetTime() > DateTime.Now)
                            return "直播结束";
                        else
                            return "审核成功";
                }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
