using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using TeacherClient.Contract;
using TeacherClient.Core;
using Reponse = TeacherClient.Contract.Reponse;
using Request = TeacherClient.Contract.Request;

namespace TeacherClient.Pages
{
    /// <summary>
    /// AddOffCourseWindow.xaml 的交互逻辑
    /// </summary>
    public partial class AddOffCourseWindow : WindowBase
    {
        public lesson Lesson { get; set; }
        public ObservableCollection<lesson> Lessons { get; set; }

        public Request.offlineCourseAdd Model { get; set; }
        public AddOffCourseWindow()
        {
            Model = new Contract.Request.offlineCourseAdd() { course_type = "0", lecturer_id = App.CurrentLogin.user.id, lec_id = App.CurrentLogin.lec_id, token = App.CurrentLogin.token };
            Lessons = new ObservableCollection<lesson>(new List<Contract.lesson> { new Contract.lesson() { start_time = DateTime.Now.ConvertDateTimeInt().ToString() } });
            Lesson = new lesson() { start_time = DateTime.Now.ConvertDateTimeInt().ToString() };
            InitializeComponent();
            this.DataContext = this;
            this.IsBusy = false;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        async void btnOk_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Model.title))
                MessageWindow.Alter("提示", "课程名称不能为空");
            if (Model.title.Length > 45)
                MessageWindow.Alter("提示", "课程名称不能超过45个字符");
            else if (string.IsNullOrWhiteSpace(Model.intro))
                MessageWindow.Alter("提示", "课程简介不能为空");
            else if (Model.intro.Length < 10)
                MessageWindow.Alter("提示", "课程简介不能少于10个字");
            else if (string.IsNullOrWhiteSpace(Model.course_type))
                MessageWindow.Alter("提示", "课程数量不能为空");
            else if (string.IsNullOrWhiteSpace(Model.image))
                MessageWindow.Alter("提示", "课程图片不能为空");
            //else if (string.IsNullOrWhiteSpace(Model.content))
            //    MessageWindow.Alter("提示", "课程详情不能为空");
            else
            {
                Model.content = "1";
                if (Model.course_type == "0")
                {
                    if (string.IsNullOrWhiteSpace(Lesson.start_time))
                        MessageWindow.Alter("提示", "上课时间不能为空");
                    else if (string.IsNullOrWhiteSpace(Lesson.Long))
                        MessageWindow.Alter("提示", "课程时长不能为空");
                    else if (string.IsNullOrWhiteSpace(Lesson.province) ||
                        string.IsNullOrWhiteSpace(Lesson.city) ||
                        string.IsNullOrWhiteSpace(Lesson.district) ||
                        string.IsNullOrWhiteSpace(Lesson.school))
                        MessageWindow.Alter("提示", "上课学校不能为空");
                    else if (string.IsNullOrWhiteSpace(Lesson.address))
                        MessageWindow.Alter("提示", "详细地址不能为空");
                    else if (string.IsNullOrWhiteSpace(Lesson.school_intro))
                        MessageWindow.Alter("提示", "学校简介不能为空");
                    else if (Lesson.school_intro.Length < 10)
                        MessageWindow.Alter("提示", "学校简介不能少于10个字");
                    else if (string.IsNullOrWhiteSpace(Lesson.num))
                        MessageWindow.Alter("提示", "上课人数不能为空");
                    else if (string.IsNullOrWhiteSpace(Lesson.tel))
                        MessageWindow.Alter("提示", "负责人联系电话不能为空");
                    else if (string.IsNullOrWhiteSpace(Lesson.wechat))
                        MessageWindow.Alter("提示", "负责人微信号不能为空");
                    else
                    {
                        Lesson.lesson_number = "1";
                        Lesson.id = "1";
                        Lesson.end_time = Lesson.start_time.GetTime().AddMinutes(double.Parse(Lesson.Long)).ConvertDateTimeInt().ToString();
                        Model.lessonList = new List<Contract.lesson>() { Lesson };
                    }
                }
                else
                {
                    var list = Lessons.ToList();
                    list.ForEach(T =>
                    {
                        if (string.IsNullOrWhiteSpace(T.start_time))
                            MessageWindow.Alter("提示", "上课时间不能为空");
                        else if (string.IsNullOrWhiteSpace(T.Long))
                            MessageWindow.Alter("提示", "课程时长不能为空");
                        else if (string.IsNullOrWhiteSpace(T.province) ||
                            string.IsNullOrWhiteSpace(T.city) ||
                            string.IsNullOrWhiteSpace(T.district) ||
                            string.IsNullOrWhiteSpace(T.school))
                            MessageWindow.Alter("提示", "上课学校不能为空");
                        else if (string.IsNullOrWhiteSpace(T.address))
                            MessageWindow.Alter("提示", "详细地址不能为空");
                        else if (string.IsNullOrWhiteSpace(T.school_intro))
                            MessageWindow.Alter("提示", "学校简介不能为空");
                        else if (T.school_intro.Length < 10)
                            MessageWindow.Alter("提示", "学校简介不能少于10个字");
                        else if (string.IsNullOrWhiteSpace(T.num))
                            MessageWindow.Alter("提示", "上课人数不能为空");
                        else if (string.IsNullOrWhiteSpace(T.tel))
                            MessageWindow.Alter("提示", "负责人联系电话不能为空");
                        else if (string.IsNullOrWhiteSpace(T.wechat))
                            MessageWindow.Alter("提示", "负责人微信号不能为空");
                        else
                        {
                            T.lesson_number = (list.IndexOf(T) + 1).ToString();
                            T.end_time = T.start_time.GetTime().AddMinutes(double.Parse(T.Long)).ConvertDateTimeInt().ToString();
                            T.id = (list.IndexOf(T) + 1).ToString();
                        }                            
                    });
                    Model.lessonList = list;
                }
                if (await WebHelper.doPost<Request.offlineCourseAdd>(Config.Interface_offlineCourseAdd, Model))
                {
                    MessageWindow.Alter("提示", "添加成功");
                    this.DialogResult = true;
                    this.Close();
                }
            }            
        }
    }
}
