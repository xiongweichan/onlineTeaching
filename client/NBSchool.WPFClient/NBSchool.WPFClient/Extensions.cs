using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace NBSchool.WPFClient
{
    public static class Extensions
    {
        public static string ObjToString(this object obj)
        {
            return obj == null ? string.Empty : obj.ToString();
        }

        public static Color ObjToColor(this object obj, object value)
        {
            var str = obj.ObjToString();
            var arr = str.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            if (arr.Length == 2)
            {
                int i, j;
                EnColorType type;
                if (Enum.TryParse<EnColorType>(arr[0], out type) && int.TryParse(value.ObjToString(), out i) && int.TryParse(arr[1].ObjToString(), out j))
                {
                    switch (type)
                    {
                        case EnColorType.Foreground:
                            if (i > j)
                                return Config.HighlightForegroundColor;
                            else return Config.NormalForegroundColor;
                        case EnColorType.Background:
                            if (i > j)
                                return Config.HighlightBackgroundColor;
                            else return Config.NormalBackgroundColor;
                    }
                }
            }
            return Colors.White;
        }
        public enum EnColorType
        {
            Foreground = 0,
            Background,
        }
    }
}
