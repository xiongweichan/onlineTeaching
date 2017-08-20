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
using TeacherClient.Core;
using Reponse = TeacherClient.Contract.Reponse;
using Request = TeacherClient.Contract.Request;

namespace TeacherClient.Pages
{
    /// <summary>
    /// OfflineCourseUpdate.xaml 的交互逻辑
    /// </summary>
    public partial class OfflineCourseUpdate : WindowBase
    {
        public Reponse.lesson Lesson { get; set; }
        public ObservableCollection<Reponse.lesson> Lessons { get; set; }

        public Reponse.offCourseDetail Model { get; set; }

        public OfflineCourseUpdate(string id, bool canupdate = true)
        {
            InitializeComponent();
            content.sp.IsEnabled = canupdate;
            if (!canupdate)
            {
                sp_operation.Visibility = Visibility.Collapsed;
                this.Title = "查看线下课程";
            }
            Lessons = new ObservableCollection<Reponse.lesson>(new List<Contract.Reponse.lesson> { new Contract.Reponse.lesson() });
            Lesson = new Reponse.lesson();
            Init(id);
        }

        async void Init(string id)
        {
            Request.RequestID l = new Request.RequestID() { lec_id = App.CurrentLogin.lec_id, token = App.CurrentLogin.token, id = id };

            var t = await WebHelper.doPost<Reponse.offCourseDetail, Request.RequestID>(Config.Interface_courseDetail, l);
            if (t != null)
            {
                Model = t;
                if(t.lessonList != null)
                {
                    if (t.course_type == "0" && t.lessonList.Count > 0)
                        Lesson = t.lessonList[0];
                    else if (t.course_type == "1")
                        Lessons = new ObservableCollection<Contract.Reponse.lesson>(t.lessonList);
                }
                
            }
            this.DataContext = this;
            this.IsBusy = false;
        }

        async void Button_Click(object sender, RoutedEventArgs e)
        {
            var l = new Request.offlineCourseUpdate()
            {
                content = Model.content,
                course_type = Model.course_type,
                id = Model.id,
                image = Model.image,
                intro = Model.intro,
                lec_id = App.CurrentLogin.lec_id,
                title = Model.title,
                token = App.CurrentLogin.token
            };
            if (Model.course_type == "0")
            {
                Lesson.lesson_number = "1";
                Lesson.id = "1";
                Lesson.end_time = Lesson.start_time.GetTime().AddMinutes(double.Parse(Lesson.Long)).ConvertDateTimeInt().ToString();
                l.lessonList = new List<Contract.Reponse.lesson>() { Lesson }.ToJson();
            }
            else
            {
                var list = Lessons.ToList();
                list.ForEach(T =>
                {
                    T.lesson_number = (list.IndexOf(T) + 1).ToString();
                    T.end_time = T.start_time.GetTime().AddMinutes(double.Parse(T.Long)).ConvertDateTimeInt().ToString();
                    T.id = (list.IndexOf(T) + 1).ToString();
                });
                l.lessonList = list.ToJson();
            }
            if (await WebHelper.doPost<Request.offlineCourseUpdate>(Config.Interface_offlineCourseUpdate, l))
            {
                MessageWindow.Alter("提示", "修改成功");
                this.DialogResult = true;
                this.Close();
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
