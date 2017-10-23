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
using System.Windows.Controls.Primitives;
using System.Threading;

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
            Request.course l = new Request.course() { lec_id = App.CurrentLogin.lec_id, token = App.CurrentLogin.token, page = pagerData.PageIndex, pageSize = pagerData.PageSize, check = "1" };
            var t = await WebHelper.doPost<Reponse.listData<Reponse.course>, Request.course>(Config.Interface_courseList, l);
            if (t != null)
            {
                pagerData.TotalCount = t.totalCount;
                lb_course.ItemsSource = t.list;
            }
            MainWindow.Current.IsBusy = false;
        }

        async void MyTextBox_TextChanged(object sender, RoutedEventArgs e)
        {
            var tb = (sender as MyTextBox);
            var id = tb.Tag.ToString();
            Request.courseChangeTitle l = new Request.courseChangeTitle() { lec_id = App.CurrentLogin.lec_id, token = App.CurrentLogin.token, id = id, title = tb.Text };
            var t = await WebHelper.doPost<Request.courseChangeTitle>(Config.Interface_courseChangeTitle, l);
            if (t)
            {
                if (courseManager.IsChecked == true)
                    GetCheckedData();
                else
                    GetUnCheckedData();
            }

        }

        private void OpenCourse_Click(object sender, RoutedEventArgs e)
        {
            CourseCenter.Current.ShowCourseManager(false, false, (sender as Control).Tag.ObjToString());
        }

        async void DeleteCourse_Click(object sender, RoutedEventArgs e)
        {
            var id = (sender as Control).Tag.ToString();
            if (MessageWindow.Alter("确认删除", "确认要删除该课程文件吗？删除后文件不可恢复") == true)
            {
                Request.RequestID l = new Request.RequestID() { lec_id = App.CurrentLogin.lec_id, token = App.CurrentLogin.token, id = id };
                var t = await WebHelper.doPost<Request.RequestID>(Config.Interface_courseDel, l);
                if (t)
                {
                    if (courseManager.IsChecked == true)
                        GetCheckedData();
                    else
                        GetUnCheckedData();
                }
            }
        }

        //private void Property_Click(object sender, RoutedEventArgs e)
        //{
        //    CoursePropertyWindow win = new CoursePropertyWindow();
        //    win.ShowDialog();
        //}

        private void UploadCourse_Click(object sender, RoutedEventArgs e)
        {
            CourseCenter.Current.ShowCourseManager(false);
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
            Request.course l = new Request.course() { lec_id = App.CurrentLogin.lec_id, token = App.CurrentLogin.token, page = pagerData.PageIndex, pageSize = pagerData.PageSize, check = "0" };
            var t = await WebHelper.doPost<Reponse.listData<Reponse.course>, Request.course>(Config.Interface_courseList, l);
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
                GetUploadFiles();
        }

        async void GetUploadFiles()
        {
            await Task.Run(() =>
            {
                try
                {
                    var list = AliyunHelper.Instance.GetList(AliyunHelper.EnFileType.Course);
                    //var list = UploadFileHelper.Instance.GetList(UploadFileHelper.EnFileType.Course);
                    long size = list.Sum(T => T.UploadedBytes);
                    DateTime time = DateTime.Now;
                    bool b = true;
                    while (b)
                    {
                        list = AliyunHelper.Instance.GetList(AliyunHelper.EnFileType.Course);
                        //list = UploadFileHelper.Instance.GetList(UploadFileHelper.EnFileType.Course);
                        var size2 = list.Sum(T => T.UploadedBytes);
                        var time2 = DateTime.Now;
                        var l = 1024d * 1024d * (time2 - time).TotalSeconds;
                        string speed = "0MB/s";
                        if (l != 0)
                            speed = ((size2 - size) / l).ToString("f2") + "MB/s";
                        var total = list.Sum(T => T.FileSize);
                        this.Dispatcher.Invoke(() =>
                        {
                            dg_upload.ItemsSource = list;
                            tbl_allspeed.Text = speed;
                            pb_all.Value = total == 0 ? 0 : size2 * 100 / total;
                            b = courseTransform.IsChecked == true;
                        });

                        size = size2;
                        time = time2;
                        Thread.Sleep(1 * 1000);
                    }
                }
                catch (Exception)
                {

                }
            });
        }

        private void btnStartAll_Click(object sender, RoutedEventArgs e)
        {
            //(dg_upload.ItemsSource as List<UploadFileHelper.FileModel>).ForEach(T => T.Pause = false);
            (dg_upload.ItemsSource as List<AliyunHelper.FileModel>).ForEach(T => T.Pause = false);
        }

        private void btn_StopAll_Click(object sender, RoutedEventArgs e)
        {
            //(dg_upload.ItemsSource as List<UploadFileHelper.FileModel>).ForEach(T => T.Pause = true);
            (dg_upload.ItemsSource as List<AliyunHelper.FileModel>).ForEach(T => T.Pause = true);

        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            //((sender as Control).Tag as UploadFileHelper.FileModel).Cancel();
            ((sender as Control).Tag as AliyunHelper.FileModel).Cancel(); 
        }

        private void ToggleButton_Click(object sender, RoutedEventArgs e)
        {
            //((sender as Control).DataContext as UploadFileHelper.FileModel).Pause = (sender as ToggleButton).IsChecked.Value;
            ((sender as Control).DataContext as AliyunHelper.FileModel).Pause = (sender as ToggleButton).IsChecked.Value;
        }
    }

}
