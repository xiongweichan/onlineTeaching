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
using Request = TeacherClient.Contract.Request;
using Reponse = TeacherClient.Contract.Reponse;

namespace TeacherClient.Pages
{
    /// <summary>
    /// CourseManager.xaml 的交互逻辑
    /// </summary>
    public partial class CourseManager : UserControl
    {
        public CourseManager()
        {
            InitializeComponent();
        }

        public void RefreshData()
        {
            courseManager.IsChecked = true;
        }
        async void GetCheckedData()
        {
            MainWindow.Current.IsBusy = true;
            Request.course l = new Request.course() { lec_id = App.CurrentLogin.lec_id, token = App.CurrentLogin.token, page = pagerData.PageIndex, pageSize = pagerData.PageSize, check = "0" };
            var t = await WebHelper.doPost<Reponse.listData<Reponse.course>, Request.course>(Config.Interface_courseList, l);
            if (t != null)
            {
                pagerData.TotalCount = t.totalCount;
                lb_course.ItemsSource = t.list;
            }
            MainWindow.Current.IsBusy = false;
        }

        private void MyTextBox_TextChanged(object sender, RoutedEventArgs e)
        {

        }

        private void OpenCourse_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DeleteCourse_Click(object sender, RoutedEventArgs e)
        {
            var id = (sender as Control).Tag.ToString();
            MessageWindow.Alter("确认删除", "确认要删除该课程文件吗？删除后文件不可恢复");
        }

        private void Property_Click(object sender, RoutedEventArgs e)
        {
            CoursePropertyWindow win = new CoursePropertyWindow();
            win.ShowDialog();
        }

        private void UploadCourse_Click(object sender, RoutedEventArgs e)
        {
            CourseCenter.Current.ShowCourseManager = false;
        }


        private void pagerData_PagerIndexChanged(object sender, EventArgs e)
        {
            GetCheckedData();
        }

        private void pagerData2_PagerIndexChanged(object sender, EventArgs e)
        {
            GetUnCheckedData();
        }
        async void GetUnCheckedData()
        {
            MainWindow.Current.IsBusy = true;
            Request.course l = new Request.course() { lec_id = App.CurrentLogin.lec_id, token = App.CurrentLogin.token, page = pagerData.PageIndex, pageSize = pagerData.PageSize, check = "1" };
            var t = await WebHelper.doPost<Reponse.listData<Reponse.course>, Request.course>(Config.Interface_coursewareList, l);
            if (t != null)
            {
                pagerData2.TotalCount = t.totalCount;
                dataGrid.ItemsSource = t.list;
            }
            MainWindow.Current.IsBusy = false;
        }

        private void StackPanel_Checked(object sender, RoutedEventArgs e)
        {
            if (pagerData == null || pagerData2 == null) return;
            if (courseManager.IsChecked == true)
                GetCheckedData();
            else if (courseCheck.IsChecked == true)
                GetUnCheckedData();
            else
                ;
        }
    }
    public class test
    {
        public string id { get; set; }
        public string title { get; set; }
        public string image { get; set; }
    }
}
