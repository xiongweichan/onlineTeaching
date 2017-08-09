using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TeacherClient.Core;
using Reponse = TeacherClient.Contract.Reponse;
using Request = TeacherClient.Contract.Request;

namespace TeacherClient.Pages
{
    /// <summary>
    /// MaryaneRecord.xaml 的交互逻辑
    /// </summary>
    public partial class MaryaneRecord : UserControl
    {
        public MaryaneRecord()
        {
            InitializeComponent();
            Init();
        }
        async void Init()
        {
            MainWindow.Current.IsBusy = true;
            Request.ParamBase l = new Request.ParamBase() { lec_id = App.CurrentLogin.lec_id, token = App.CurrentLogin.token};
            var t = await WebHelper.doPost<Reponse.liveRewardIndex, Request.ParamBase>(Config.Interface_liveRewardIndex, l);
            grid_total.DataContext = t;
            MainWindow.Current.IsBusy = false;            
        }

        private void pagerData_PagerIndexChanged(object sender, EventArgs e)
        {
            GetData();
        }

        async void GetData()
        {
            MainWindow.Current.IsBusy = true;
            Request.PageParam l = new Request.PageParam() { lec_id = App.CurrentLogin.lec_id, token = App.CurrentLogin.token, page = pagerData.PageIndex, pageSize = pagerData.PageSize };

            var t = await WebHelper.doPost<Reponse.listData<Reponse.liveRewardList>, Request.PageParam>(Config.Interface_liveRewardList, l);
            if(t != null)
            {
                pagerData.TotalCount = t.totalCount;
                datagrid.ItemsSource = t.list;
            }

            MainWindow.Current.IsBusy = false;

        }
    }
}
