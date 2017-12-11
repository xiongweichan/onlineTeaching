using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeacherClient
{
    public static class Config
    {
        public static readonly string SystemConfigPath = AppDomain.CurrentDomain.BaseDirectory + "SysConfig\\SystemConfig.xml";
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
        public static readonly string Interface_checkEmailCode = "Live/LecLogin/checkEmailCode";
        public static readonly string Interface_register = "Live/LecLogin/register";
        public static readonly string Interface_checkCode = "Live/LecLogin/checkCode";
        public static readonly string Interface_resetPwd = "Live/LecLogin/resetPwd";


        public static readonly string Interface_mainIndex = "Live/LecUser/mainIndex";
        public static readonly string Interface_userInfo = "Live/LecUser/userInfo";
        public static readonly string Interface_userHeadSet = "Live/LecUser/userHeadSet";
        public static readonly string Interface_userPhotoSet = "Live/LecUser/userPhotoSet";
        public static readonly string Interface_userSet = "Live/LecUser/userSet";
        public static readonly string Interface_changePhone = "Live/LecUser/changePhone";
        public static readonly string Interface_changeEmail = "Live/LecUser/changeEmail";
        public static readonly string Interface_lecturerTypeList = "Live/LecUser/lecturerTypeList";
        public static readonly string Interface_lecturerGoodAtList = "Live/LecUser/lecturerGoodAtList";

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
        public static readonly string Interface_courseList = "Live/LecCourse/courseList";
        public static readonly string Interface_courseAdd = "Live/LecCourse/courseAdd";
        public static readonly string Interface_courseDetail_LecCourse = "Live/LecCourse/courseDetail";
        public static readonly string Interface_courseDel = "Live/LecCourse/courseDel";
        public static readonly string Interface_courseChangeTitle = "Live/LecCourse/courseChangeTitle";
        public static readonly string Interface_courseLessonUpload = "Live/LecCourse/courseLessonUploadAliyun";
        public static readonly string Interface_courseLessonUploadCompleted = "Live/LecCourse/courseLessonUploadAliyunComplete";
        public static readonly string Interface_coursewareAdd = "Live/LecCourse/coursewareAdd";
        public static readonly string Interface_coursewareDetail = "Live/LecCourse/coursewareDetail";
        public static readonly string Interface_coursewareUpload = "Live/LecCourse/liveWareUploadAliyun";
        public static readonly string Interface_coursewareUploadCompleted = "Live/LecCourse/liveWareUploadAliyunComplete";
        public static readonly string Interface_coursewareList = "Live/LecCourse/coursewareList";
        public static readonly string Interface_coursewareDel = "Live/LecCourse/coursewareDel";
        public static readonly string Interface_coursewareChangeTitle = "Live/LecCourse/coursewareChangeTitle";
        public static readonly string Interface_messageList = "Live/LecCourse/messageList";
        public static readonly string Interface_liveWareUpload = "Live/LecLive/liveWareUploadAliyun";
        public static readonly string Interface_liveWareUploadCompleted = "Live/LecLive/liveWareUploadAliyunComplete";


        public static readonly string Interface_imageUpLoad = "Live/Upload/imageUpLoad";
        public static readonly string Interface_upload = "Live/Upload/upload";

        public static readonly string Interface_liveList = "Live/LecLive/liveList";
        public static readonly string Interface_liveChangeTime = "Live/LecLive/liveChangeTime";
        public static readonly string Interface_liveDel = "Live/LecLive/liveDel";
        public static readonly string Interface_liveCancel = "Live/LecLive/liveCancel";
        public static readonly string Interface_liveAdd = "Live/LecLive/liveAdd";
        public static readonly string Interface_liveDetail = "Live/LecLive/liveDetail";
        public static readonly string Interface_liveReward = "Live/LecLive/liveReward";

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
