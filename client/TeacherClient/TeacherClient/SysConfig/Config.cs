﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeacherClient
{
    public static class Config
    {
        public static readonly string SystemConfigPath = "SysConfig/SystemConfig.xml";
        public static readonly string IsAutoStart = "IS_AUTO_STARTUP";
        public static readonly string CacheFilePath = "CACHE_PATH";


        public static readonly string IsAutoLogin = "IsAutoLogin";
        public static readonly string IsRememberPwd = "IsRememberPwd";
        public static readonly string UserAccount = "UserAccount";
        public static readonly string Password = "Password";


        public static readonly int SuccessCode = 1;



        public static readonly string Interface_login = "Live/LecLogin/login";
        public static readonly string Interface_mainIndex = "Live/LecUser/mainIndex";
        public static readonly string Interface_userInfo = "Live/LecUser/userInfo";


        //课酬中心
        public static readonly string Interface_liveRewardList = "Live/LecReward/liveRewardList";
        public static readonly string Interface_liveRewardIndex = "Live/LecReward/liveRewardIndex";
        public static readonly string Interface_liveGiftIndex = "Live/LecReward/liveGiftIndex";
        public static readonly string Interface_liveGiftList = "Live/LecReward/liveGiftList";
        
    }
}
