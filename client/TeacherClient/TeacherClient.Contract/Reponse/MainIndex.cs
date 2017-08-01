﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeacherClient.Contract.Reponse
{
    public class MainIndex
    {
        public string liveCount { get; set; }
        public string courseCount { get; set; }
        public string coursewareCount { get; set; }
        public string livePreCount { get; set; }
        public string coursePreCount { get; set; }
        public string moneyWithdraw { get; set; }
        public List<Message> messageList { get; set; }
        public UserInfo userInfo { get; set; }
    }
    public class UserInfo
    {
        public string id { get; set; }

        public string phone { get; set; }

        public string email { get; set; }

        public string is_check { get; set; }

        public string check_reason { get; set; }

        public string auth_time { get; set; }

        public string status { get; set; }

        public string id_card_front { get; set; }

        public string id_card_back { get; set; }

        public string body_photo { get; set; }

        public string qualification_cert { get; set; }

        public string money { get; set; }

        public string nickname { get; set; }
        public string head { get; set; }

        public string intro { get; set; }


        public string realname { get; set; }

        public string sex { get; set; }

        public string age { get; set; }

        public string birth { get; set; }

        public string province { get; set; }
        public string city { get; set; }

        public string district { get; set; }

        public string tel { get; set; }

        public string job { get; set; }
        public string add_time { get; set; }

        public string last_login { get; set; }

        public string wechat { get; set; }

        public string lecturer_type { get; set; }

        public string good_at { get; set; }

        public string province_name { get; set; }

        public string city_name { get; set; }

        public string district_name { get; set; }
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
