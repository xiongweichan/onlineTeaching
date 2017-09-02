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
    /// NotStartCourse.xaml 的交互逻辑
    /// </summary>
    public partial class NotStartCourse : UserControl
    {
        public NotStartCourse()
        {
            InitializeComponent();
        }

        private void btnModify_Click(object sender, RoutedEventArgs e)
        {

            var id = (sender as Control).Tag.ObjToString();
            OfflineCourseUpdate win = new OfflineCourseUpdate(id);
            win.ShowDialog();
        }

        private void btnShowStudent_Click(object sender, RoutedEventArgs e)
        {

            var id = (sender as Control).Tag.ObjToString();
            StudentListWindow win = new StudentListWindow(id);
            win.ShowDialog();
        }

        private void pagerData_PagerIndexChanged(object sender, EventArgs e)
        {
            GetData();
        }
        async void GetData()
        {
            MainWindow.Current.IsBusy = true;
            Request.OffCourselist l = new Request.OffCourselist() { lec_id = App.CurrentLogin.lec_id, token = App.CurrentLogin.token, page = pagerData.PageIndex, pageSize = pagerData.PageSize, time_type="1" };

            var t = await WebHelper.doPost<Reponse.listData<Reponse.lessonList>, Request.OffCourselist>(Config.Interface_lessonList, l);
            if (t != null)
            {
                pagerData.TotalCount = t.totalCount;
                datagrid.ItemsSource = t.list;
            }

            MainWindow.Current.IsBusy = false;

        }
    }
}
