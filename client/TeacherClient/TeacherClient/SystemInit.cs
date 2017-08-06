using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeacherClient.Core;
using Reponse = TeacherClient.Contract.Reponse;
using Request = TeacherClient.Contract.Request;

namespace TeacherClient
{
    public class SystemInit
    {
        public static readonly SystemInit Instance = new SystemInit();

        public async void Init()
        {
            try
            {
                Request.ParamBase l = new Request.ParamBase() { lec_id = App.CurrentLogin.lec_id, token = App.CurrentLogin.token };

                BankInfos = await WebHelper.doPost<List<Reponse.bankinfo>, Request.ParamBase>(Config.Interface_bankList, l);

                Regions = await WebHelper.doPost<List<Reponse.region>, Request.ParamBase>(Config.Interface_regionList, l);


            }
            catch(Exception ex)
            {
                Log.Error("初始化基础数据失败", ex);
            }
        }

        public List<Reponse.bankinfo> BankInfos { get; private set; }
        public List<Reponse.region> Regions { get; private set; }
    }
}
