using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeacherClient.Contract.Reponse
{
    public class MainIndex
    {
        public string unreadCount { get; set; }
        public string liveCount { get; set; }
        public string courseCount { get; set; }
        public string coursewareCount { get; set; }
        public string livePreCount { get; set; }
        public string coursePreCount { get; set; }
        public string moneyWithdraw { get; set; }
        public List<Message> messageList { get; set; }
        public userInfo userInfo { get; set; }
    }

    public class Message
    {
        public int id { get; set; }

        public string type { get; set; }

        public string title { get; set; }

        public string summary { get; set; }

        public string content { get; set; }

        public string add_time { get; set; }

        public string lecturer_id { get; set; }
        public string status { get; set; }
    }
}
