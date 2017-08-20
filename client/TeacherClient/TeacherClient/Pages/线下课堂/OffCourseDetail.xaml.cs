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
    /// OffCourseDetail.xaml 的交互逻辑
    /// </summary>
    public partial class OffCourseDetail : UserControl
    {
        public string CourseImage
        {
            get { return (string)GetValue(CourseImageProperty); }
            set { SetValue(CourseImageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CourseImage.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CourseImageProperty =
            DependencyProperty.Register("CourseImage", typeof(string), typeof(OffCourseDetail), new PropertyMetadata(PropertyChangedCallback));
        static void PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((d as OffCourseDetail).DataContext as dynamic).Model.image = (d as OffCourseDetail).CourseImage;
        }
        public OffCourseDetail()
        {
            InitializeComponent();
        }

        async void btnUpload_Click(object sender, RoutedEventArgs e)
        {
            var str = await UploadImageHelper.UploadImage();
            if (!string.IsNullOrEmpty(str))
                CourseImage = str;
        }
    }
}
