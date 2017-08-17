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
            lb_course.ItemsSource = new List<test>
            {
                new test { id="1", image="/Public/test.png", title=DateTime.Now.ToString() },
                new test { id="2", image="/Public/test.png", title=DateTime.Now.ToString() },
                new test { id="3", image="/Public/test.png", title=DateTime.Now.ToString() }
            };

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

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void UploadCourse_Click(object sender, RoutedEventArgs e)
        {
            CourseCenter.Current.ShowCourseManager = false;
        }
    }
    public class test
    {
        public string id { get; set; }
        public string title { get; set; }
        public string image { get; set; }
    }
}
