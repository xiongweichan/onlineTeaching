using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace NBSchool.WPFClient
{
    public static class Config
    {
        /// <summary>
        /// 蓝色
        /// </summary>
        public static Color HighlightBackgroundColor { get { return (Color)ColorConverter.ConvertFromString("#63A1FF"); } }
        /// <summary>
        /// 灰色
        /// </summary>
        public static Color NormalBackgroundColor { get { return (Color)ColorConverter.ConvertFromString("#D6D6D6"); } }


        /// <summary>
        /// 白色
        /// </summary>
        public static Color HighlightForegroundColor { get { return (Color)ColorConverter.ConvertFromString("#FFFFFF"); } }
        /// <summary>
        /// 灰色
        /// </summary>
        public static Color NormalForegroundColor { get { return (Color)ColorConverter.ConvertFromString("#888888"); } }
    }
}
