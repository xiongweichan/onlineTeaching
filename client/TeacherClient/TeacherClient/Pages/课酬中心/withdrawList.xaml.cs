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
using Reponse = TeacherClient.Contract.Reponse;
using Request = TeacherClient.Contract.Request;

namespace TeacherClient.Pages
{
    /// <summary>
    /// withdrawList.xaml 的交互逻辑
    /// </summary>
    public partial class withdrawList : UserControl
    {
        public withdrawList()
        {
            InitializeComponent();
        }

        private void pagerData_PagerIndexChanged(object sender, EventArgs e)
        {
            GetData();
        }
        async void GetData()
        {
            MainWindow.Current.IsBusy = true;
            Request.PageParam l = new Request.PageParam() { lec_id = App.CurrentLogin.lec_id, token = App.CurrentLogin.token, page = pagerData.PageIndex, pageSize = pagerData.PageSize };

            var t = await WebHelper.doPost<Reponse.listData<Reponse.withdrawList>, Request.PageParam>(Config.Interface_withdrawList, l);
            if (t != null)
            {
                pagerData.TotalCount = t.totalCount;
                datagrid.ItemsSource = t.list;
            }

            MainWindow.Current.IsBusy = false;

        }
    }
}
