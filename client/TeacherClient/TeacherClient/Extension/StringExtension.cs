using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeacherClient
{
    public static class StringExtension
    {
        public static string ObjToString(this object obj)
        {
            return obj == null ? string.Empty : obj.ToString();
        }

    }
}
