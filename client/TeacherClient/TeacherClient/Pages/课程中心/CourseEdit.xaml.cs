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
    /// CourseEdit.xaml 的交互逻辑
    /// </summary>
    public partial class CourseEdit : UserControl
    {
        public Request.courseAdd Model
        {
            get { return (Request.courseAdd)GetValue(ModelProperty); }
            set { SetValue(ModelProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Model.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ModelProperty =
            DependencyProperty.Register("Model", typeof(Request.courseAdd), typeof(CourseEdit), new PropertyMetadata());



        public CourseEdit()
        {
            InitializeComponent();
        }

        async void Button_Click(object sender, RoutedEventArgs e)
        {
            Model.lec_id = App.CurrentLogin.lec_id;
            Model.token = App.CurrentLogin.token;
            var b = await WebHelper.doPost<Request.courseAdd>(Config.Interface_courseAdd, Model);
            if (b)
                CourseCenter.Current.ShowCourseManager = true;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            CourseCenter.Current.ShowCourseManager = true;
        }

        private void UploadCourse_Click(object sender, RoutedEventArgs e)
        {

        }

        async void UploadImage_Click(object sender, RoutedEventArgs e)
        {
            var str = await UploadImageHelper.UploadImage();
            if (!string.IsNullOrEmpty(str))
            {
                Model.image = str;
            }
        }
    }
}
