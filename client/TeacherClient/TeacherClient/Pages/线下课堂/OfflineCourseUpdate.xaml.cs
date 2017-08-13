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

        public OfflineCourseUpdate(string id, bool canupdate=true)
        {
            InitializeComponent();
            content.IsEnabled = canupdate;
            if (!canupdate)
            {
                sp_operation.Visibility = Visibility.Collapsed;
                this.Title = "查看线下课程";                
            }
            Init(id);
        }

        async void Init(string id)
        {
            Request.RequestID l = new Request.RequestID() { lec_id = App.CurrentLogin.lec_id, token = App.CurrentLogin.token, id = id };

            var t = await WebHelper.doPost<Reponse.offCourseDetail, Request.RequestID>(Config.Interface_courseDetail, l);
            if (t != null)
            {
                Model = t;
                if (t.course_type == "0")
                    Lesson = t.lessonList[0];
                else
                    Lessons = new ObservableCollection<Contract.Reponse.lesson>(t.lessonList ?? new List<Contract.Reponse.lesson>());
            }
            this.DataContext = this;
            this.IsBusy = false;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {


            this.DialogResult = true;
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
