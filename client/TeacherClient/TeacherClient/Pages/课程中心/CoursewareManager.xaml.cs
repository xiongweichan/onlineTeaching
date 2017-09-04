﻿using System;
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
using System.Threading;

namespace TeacherClient.Pages
{
    /// <summary>
    /// CoursewareManager.xaml 的交互逻辑
    /// </summary>
    public partial class CoursewareManager : UserControl
    {
        public CoursewareManager()
        {
            InitializeComponent();
            //lb_course.ItemsSource = new List<test>
            //{
            //    new test { id="1", image="/Public/test.png", title=DateTime.Now.ToString() },
            //    new test { id="2", image="/Public/test.png", title=DateTime.Now.ToString() },
            //    new test { id="3", image="/Public/test.png", title=DateTime.Now.ToString() }
            //};

        }

        public void RefreshData()
        {
            courseManager.IsChecked = true;
        }
        async void GetCheckedData()
        {
            MainWindow.Current.IsBusy = true;
            Request.courseware l = new Request.courseware() { lec_id = App.CurrentLogin.lec_id, token = App.CurrentLogin.token, page = pagerData.PageIndex, pageSize = pagerData.PageSize, check = "1" };
            var t = await WebHelper.doPost<Reponse.listData<Reponse.courseware>, Request.courseware>(Config.Interface_coursewareList, l);
            if (t != null)
            {
                pagerData.TotalCount = t.totalCount;
                lb_course.ItemsSource = t.list;
            }
            MainWindow.Current.IsBusy = false;
        }

        private void OpenCourse_Click(object sender, RoutedEventArgs e)
        {
            CourseCenter.Current.ShowCoursewareManager(false, false, (sender as Control).Tag.ObjToString());
        }

        async void DeleteCourse_Click(object sender, RoutedEventArgs e)
        {
            var id = (sender as Control).Tag.ToString();
            if (MessageWindow.Alter("确认删除", "确认要删除该课件吗？删除后不可恢复") == true)
            {
                Request.RequestID l = new Request.RequestID() { lec_id = App.CurrentLogin.lec_id, token = App.CurrentLogin.token, id = id };
                var t = await WebHelper.doPost<Request.RequestID>(Config.Interface_coursewareDel, l);
                if (t)
                {
                    if (courseManager.IsChecked == true)
                        GetCheckedData();
                    else
                        GetUnCheckedData();
                }
            }
        }

        private void Property_Click(object sender, RoutedEventArgs e)
        {
            var c = (sender as Control).Tag as Reponse.courseware;
            CoursePropertyWindow win = new CoursePropertyWindow(c);
            win.ShowDialog();
        }

        private void BtnUploadCourseware_Click(object sender, RoutedEventArgs e)
        {
            CourseCenter.Current.ShowCoursewareManager(false, true);
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
            Request.courseware l = new Request.courseware() { lec_id = App.CurrentLogin.lec_id, token = App.CurrentLogin.token, page = pagerData.PageIndex, pageSize = pagerData.PageSize, check = "0" };
            var t = await WebHelper.doPost<Reponse.listData<Reponse.courseware>, Request.courseware>(Config.Interface_coursewareList, l);
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
                long size = 0;
                DateTime _time = DateTime.Now;
                bool b = true;
                while (b)
                {
                    var list = UploadFileHelper.Instance.GetList(UploadFileHelper.EnFileType.Courseware);                    
                    var size2 = list.Sum(T => T.UploadedBytes);
                    this.Dispatcher.Invoke(() =>
                    {
                        dg_upload.ItemsSource = list;
                        tbl_allspeed.Text = ((size2 - size) / 1024d / 1024d / (DateTime.Now - _time).TotalSeconds).ToString("f2") + "MB/s";
                        var total = list.Sum(T => T.FileSize);
                        pb_all.Value = total == 0 ? 0 : size2 * 100 / total;
                        b = courseTransform.IsChecked == true;
                    });
                    
                    size = size2;
                    Thread.Sleep(5 * 1000);
                }
            });            
        }

        async void MyTextBox_TextChanged(object sender, RoutedEventArgs e)
        {
            var tb = (sender as MyTextBox);
            var id = tb.Tag.ToString();
            Request.coursewareChangeTitle l = new Request.coursewareChangeTitle() { lec_id = App.CurrentLogin.lec_id, token = App.CurrentLogin.token, id = id, title = tb.Text };
            var t = await WebHelper.doPost<Request.coursewareChangeTitle>(Config.Interface_coursewareChangeTitle, l);
            if (t)
            {
                if (courseManager.IsChecked == true)
                    GetCheckedData();
                else
                    GetUnCheckedData();
            }

        }

        private void btnStartAll_Click(object sender, RoutedEventArgs e)
        {
            (dg_upload.ItemsSource as List<UploadFileHelper.FileModel>).ForEach(T => T.Pause = false);
        }

        private void btn_StopAll_Click(object sender, RoutedEventArgs e)
        {
            (dg_upload.ItemsSource as List<UploadFileHelper.FileModel>).ForEach(T => T.Pause = true);
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            ((sender as Control).Tag as UploadFileHelper.FileModel).Cancel();
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            ((sender as Control).Tag as UploadFileHelper.FileModel).Pause = false;
        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            ((sender as Control).Tag as UploadFileHelper.FileModel).Pause = true;

        }
    }
}
