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
    public class LiveRewardListProvider : IItemsProvider<Reponse.liveRewardList>
    {
        public int FetchCount()
        {
            return 1;
        }

        public IList<Reponse.liveRewardList> FetchRange(int startIndex, int count)
        {
            Request.PageParam l = new Request.PageParam() { lec_id = App.CurrentLogin.lec_id, token = App.CurrentLogin.token, page = 0, pageSize = 20 };
            var t = IPCHandle.doPost<Reponse.ResponseParam<List<Reponse.liveRewardList>>>(Config.Interface_liveRewardList, l.ReturnRequestParam());
            t.Wait();
            if(t.Result.status != Config.SuccessCode)
            {
                Telerik.Windows.Controls.ViewModelBase.InvokeOnUIThread(() =>
                {
                    MessageWindow win = new MessageWindow();
                    win.Title = "错误";
                    win.Message = t.Result.info;
                    win.ShowDialog();
                });
            }
            return t.Result.data;
        }
    }
}
