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
    /// AddOffCourseWindow.xaml 的交互逻辑
    /// </summary>
    public partial class AddOffCourseWindow : WindowBase
    {
        public Request.lesson Lesson { get; set; }
        public ObservableCollection<Request.lesson> Lessons { get; set; }

        public Request.offlineCourseAdd Model { get; set; }
        public AddOffCourseWindow()
        {
            Model = new Contract.Request.offlineCourseAdd() { course_type = "0", lecturer_id = App.CurrentLogin.user.id, lessonList = new List<Contract.Request.lesson>() { new Contract.Request.lesson() } };
            Lessons = new ObservableCollection<Request.lesson>(new List<Contract.Request.lesson> { new Contract.Request.lesson() });
            Lesson = new Request.lesson();
            InitializeComponent();
            this.DataContext = this;
            this.IsBusy = false;
        }
    }
}
