using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace TeacherClient
{
    public class TimerHelper : IDisposable
    {
        DispatcherTimer _timer = new DispatcherTimer(new TimeSpan(0, 0, 0, 0, 1000), DispatcherPriority.Background, callBack, App.Current.Dispatcher);

        private static void callBack(object sender, EventArgs e)
        {
            TimerEvent?.Invoke(null, null);
        }

        public void Dispose()
        {
            _timer.Stop();
        }

        public static event EventHandler TimerEvent;

        public static readonly TimerHelper Instance = new TimerHelper();
        public TimerHelper()
        {
            _timer.Start();
        }
    }
}
