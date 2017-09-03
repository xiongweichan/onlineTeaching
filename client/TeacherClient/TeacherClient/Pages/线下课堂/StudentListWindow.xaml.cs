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
using System.Windows.Shapes;
using TeacherClient.Core;
using Reponse = TeacherClient.Contract.Reponse;
using Request = TeacherClient.Contract.Request;

namespace TeacherClient.Pages
{
    /// <summary>
    /// StudentListWindow.xaml 的交互逻辑
    /// </summary>
    public partial class StudentListWindow : WindowBase
    {
        string _courseID;
        public StudentListWindow(string cid)
        {
            _courseID = cid;
            InitializeComponent();
            this.IsBusy = false;
        }


        private void pagerData_PagerIndexChanged(object sender, EventArgs e)
        {
            GetData();
        }
        async void GetData()
        {
            MainWindow.Current.IsBusy = true;
            Request.courseid l = new Request.courseid() { lec_id = App.CurrentLogin.lec_id, token = App.CurrentLogin.token, page = pagerData.PageIndex, pageSize = pagerData.PageSize, course_id = _courseID };

            var t = await WebHelper.doPost<Reponse.listData<Reponse.offUserlist>, Request.courseid>(Config.Interface_userList, l);
            //t = new Contract.Reponse.listData<Contract.Reponse.offUserlist>()
            //{
            //    list = new List<Contract.Reponse.offUserlist> {
            //         new Contract.Reponse.offUserlist
            //         {
            //              id = 1, add_time = DateTime.Now.ConvertDateTimeInt().ToString(),
            //               desc="hellokittle", lesson_number="1", name="长征", phone="13812345678", status="0"
            //               , course_id="1", user_id="1"
            //         },
            //         new Contract.Reponse.offUserlist
            //         {
            //              id = 2, add_time = DateTime.Now.ConvertDateTimeInt().ToString(),
            //               desc="hellokittle", lesson_number="1", name="长征", phone="13812345678", status="1"
            //               , course_id="1", user_id="1"
            //         },
            //         new Contract.Reponse.offUserlist
            //         {
            //              id = 3, add_time = DateTime.Now.ConvertDateTimeInt().ToString(),
            //               desc="hellokittle", lesson_number="1", name="长征", phone="13812345678", status="2"
            //               , course_id="1", user_id="1"
            //         },
            //         new Contract.Reponse.offUserlist
            //         {
            //              id = 4, add_time = DateTime.Now.ConvertDateTimeInt().ToString(),
            //               desc="hellokittle", lesson_number="1", name="长征", phone="13812345678", status="3"
            //               , course_id="1", user_id="1"
            //         }
            //     }
            //};
            if (t != null)
            {
                pagerData.TotalCount = t.totalCount;
                datagrid.ItemsSource = t.list;
            }

            MainWindow.Current.IsBusy = false;

        }

        async void btnAgree_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.Current.IsBusy = true;
            var id = (sender as Control).Tag.ObjToString();
            Request.RequestID l = new Request.RequestID() { lec_id = App.CurrentLogin.lec_id, token = App.CurrentLogin.token, id = id };

            var t = await WebHelper.doPost<string, Request.RequestID>(Config.Interface_userAgree, l);
            MainWindow.Current.IsBusy = false;
            GetData();
        }

        async void btnRefuse_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.Current.IsBusy = true;
            var id = (sender as Control).Tag.ObjToString();
            Request.RequestID l = new Request.RequestID() { lec_id = App.CurrentLogin.lec_id, token = App.CurrentLogin.token, id = id };

            var t = await WebHelper.doPost<string, Request.RequestID>(Config.Interface_userDisagree, l);
            MainWindow.Current.IsBusy = false;
            GetData();
        }
    }
}
