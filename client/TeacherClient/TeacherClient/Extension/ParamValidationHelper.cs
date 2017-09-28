using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeacherClient
{
    public static class ParamValidationHelper
    {
        public static bool MyIsNullOrWhiteSpace(this string str, string msg)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                MessageWindow.Alter("提示", msg + "不能为空");
                return false;
            }
            else return true;
        }
    }
}
