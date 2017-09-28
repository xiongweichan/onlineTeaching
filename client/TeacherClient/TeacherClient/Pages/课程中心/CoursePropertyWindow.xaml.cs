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
using System.Windows.Shapes;
using Reponse = TeacherClient.Contract.Reponse;

namespace TeacherClient.Pages
{
    /// <summary>
    /// CoursePropertyWindow.xaml 的交互逻辑
    /// </summary>
    public partial class CoursePropertyWindow : WindowBase
    {
        public CoursePropertyWindow(Reponse.courseware cw)
        {
            InitializeComponent();
            run_filename.Text = cw.file_name;
            run_filesize.Text = (int.Parse(cw.file_size)/1024.0/1024.0).ToString("f2")+ "MB";
            run_filetype.Text = FileTypeConverter.GetFileType(cw.file_mime);
            run_uploadtime.Text = cw.upload_time.GetTime().ToString("yyyy年MM月dd日 HH:mm:ss");
            this.IsBusy = false;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
