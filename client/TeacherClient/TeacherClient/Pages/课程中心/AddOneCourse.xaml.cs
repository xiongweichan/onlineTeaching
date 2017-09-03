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
        UploadFileHelper.FileModel _vedio;
        private void UploadCourse_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Multiselect = false;
            dialog.Filter = "视频文件|*.MP4;*.RMVB;*.AVI";
            if (dialog.ShowDialog() == true)
            {
                var path = dialog.FileName;
                if (new FileInfo(path).Length > 1000 * 1024 * 1024)
                {
                    MessageWindow.Alter("提示", "文件过大");
                    return;
                }
                _vedio = UploadFileHelper.Instance.Add("", UploadFileHelper.EnFileType.Course);
            }

        }

        UploadFileHelper.FileModel _doc;
        private void UploadDocument_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Multiselect = false;
            dialog.Filter = "课件文件|*.dox;*.docx;*.ppt;*.pptx;*.png;*.bmp;*.jpg";
            if (dialog.ShowDialog() == true)
            {
                var path = dialog.FileName;
                if (new FileInfo(path).Length > 10 * 1024 * 1024)
                {
                    MessageWindow.Alter("提示", "文件过大");
                    return;
                }
                _doc = UploadFileHelper.Instance.Add(path, UploadFileHelper.EnFileType.Course);
            }
        }
    }
}
