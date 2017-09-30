using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
    /// AddOneCourse.xaml 的交互逻辑
    /// </summary>
    public partial class AddOneCourse : UserControl
    {
        public AddOneCourse()
        {
            InitializeComponent();
        }
        private void UploadCourse_Click(object sender, RoutedEventArgs e)
        {
            var path = FileSelectHelper.FileSelecte("视频文件|*.MP4;*.RMVB;*.AVI", 1000);
            if (string.IsNullOrWhiteSpace(path)) return;

            (DataContext as CourseModel).Vedio = path;
            (DataContext as CourseModel).VedioName = System.IO.Path.GetFileName(path);
        }
        
        private void UploadDocument_Click(object sender, RoutedEventArgs e)
        {
            var path = FileSelectHelper.FileSelecte("课件文件|*.doc;*.docx;*.ppt;*.pptx;*.png;*.bmp;*.jpg;*.txt;*.xls;*.xlsx", 10);
            if (string.IsNullOrWhiteSpace(path)) return;
            
            (DataContext as CourseModel).Document = path;
            (DataContext as CourseModel).DocumentName = System.IO.Path.GetFileName(path);

        }
    }
}
