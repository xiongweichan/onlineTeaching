using System;
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
        public static readonly string Interface_phoneCode = "Live/LecLogin/phoneCode";
        public static readonly string Interface_emailCode = "Live/LecLogin/emailCode";
        public static readonly string Interface_register = "Live/LecLogin/register";

        public static readonly string Interface_mainIndex = "Live/LecUser/mainIndex";
        public static readonly string Interface_userInfo = "Live/LecUser/userInfo";
        public static readonly string Interface_userHeadSet = "Live/LecUser/userHeadSet";
        public static readonly string Interface_userPhotoSet = "Live/LecUser/userPhotoSet";
        public static readonly string Interface_userSet = "Live/LecUser/userSet";
        public static readonly string Interface_changePhone = "Live/LecUser/changePhone";
        public static readonly string Interface_changeEmail = "Live/LecUser/changeEmail";

        //课酬中心
        public static readonly string Interface_liveRewardList = "Live/LecReward/liveRewardList";
        public static readonly string Interface_liveRewardIndex = "Live/LecReward/liveRewardIndex";
        public static readonly string Interface_liveGiftIndex = "Live/LecReward/liveGiftIndex";
        public static readonly string Interface_liveGiftList = "Live/LecReward/liveGiftList";
        
        public static readonly string Interface_withdrawIndex = "Live/LecWithdraw/withdrawIndex";
        public static readonly string Interface_withdrawAdd = "Live/LecWithdraw/withdrawAdd";
        public static readonly string Interface_withdrawList = "Live/LecWithdraw/withdrawList";
        public static readonly string Interface_bankcardDetail = "Live/LecWithdraw/bankcardDetail";
        public static readonly string Interface_bankcardAdd = "Live/LecWithdraw/bankcardAdd";
        public static readonly string Interface_bankcardEdit = "Live/LecWithdraw/bankcardEdit";
        public static readonly string Interface_withdrawCharge = "Live/LecWithdraw/withdrawCharge";

        //线下课程
        public static readonly string Interface_lessonList = "Live/LecOfflineCourse/lessonList";
        public static readonly string Interface_userList = "Live/LecOfflineCourse/userList";
        public static readonly string Interface_userAgree = "Live/LecOfflineCourse/userAgree";
        public static readonly string Interface_userDisagree = "Live/LecOfflineCourse/userDisagree";
        public static readonly string Interface_courseDetail = "Live/LecOfflineCourse/courseDetail";
        public static readonly string Interface_offlineCourseAdd = "Live/LecOfflineCourse/offlineCourseAdd";
        public static readonly string Interface_offlineCourseUpdate = "Live/LecOfflineCourse/offlineCourseUpdate";
        

        public static readonly string Interface_bankList = "Live/Common/bankList";
        public static readonly string Interface_regionList = "Live/Common/regionList";

        public static readonly string Interface_categoryList = "Live/LecCourse/categoryList";
        public static readonly string Interface_messageList = "Live/LecCourse/messageList";


        public static readonly string Interface_imageUpLoad = "Live/Upload/imageUpLoad";

        public static readonly string Interface_liveList = "Live/LecLive/liveList";
        

        public class phoneCode
        {
            public static readonly string registerAccount = "50";
            public static readonly string resetPassword = "51";
            public static readonly string editOldPhone = "52";
            public static readonly string editNewPhone = "53";
            public static readonly string registerBank = "54";
        }
        public class emailCode
        {
            public static readonly string editOldEmail = "52";
            public static readonly string editNewEmail = "53";
        }
    }

}
