using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeacherClient
{
    public interface INavigation
    {
        void NavigateToPage(int index, object data);
    }
}
