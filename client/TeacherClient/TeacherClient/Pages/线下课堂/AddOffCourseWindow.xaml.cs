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
    /// AddOffCourseWindow.xaml 的交互逻辑
    /// </summary>
    public partial class AddOffCourseWindow : WindowBase
    {
        public Request.lesson Lesson { get; set; }
        public ObservableCollection<Request.lesson> Lessons { get; set; }

        public Request.offlineCourseAdd Model { get; set; }
        public AddOffCourseWindow()
        {
            Model = new Contract.Request.offlineCourseAdd() { course_type = "0", lecturer_id = App.CurrentLogin.user.id, lec_id = App.CurrentLogin.lec_id, token = App.CurrentLogin.token };
            Lessons = new ObservableCollection<Request.lesson>(new List<Contract.Request.lesson> { new Contract.Request.lesson() });
            Lesson = new Request.lesson();
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
            Model.content = "1";
            if (Model.course_type == "0")
            {
                Lesson.lesson_number = "1";
                Lesson.id = "1";
                Lesson.end_time = Lesson.start_time.GetTime().AddMinutes(double.Parse(Lesson.Long)).ConvertDateTimeInt().ToString();
                Model.lessonList = new List<Contract.Request.lesson>() { Lesson }.ToJson();
            }
            else
            {
                var list = Lessons.ToList();
                list.ForEach(T =>
                {
                    T.lesson_number = (list.IndexOf(T) + 1).ToString();
                    T.id = (list.IndexOf(T) + 1).ToString();
                });
                Model.lessonList = list.ToJson();
            }
            if (await WebHelper.doPost<Request.offlineCourseAdd>(Config.Interface_offlineCourseAdd, Model))
                MessageWindow.Alter("提示", "添加成功");
        }
    }
}
