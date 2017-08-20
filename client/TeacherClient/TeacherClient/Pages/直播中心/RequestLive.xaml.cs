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
    /// RequestLive.xaml 的交互逻辑
    /// </summary>
    public partial class RequestLive : UserControl
    {
        public RequestLive()
        {
            InitializeComponent();
        }

        private void UploadCourse_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            if (firstpage.IsChecked == true)
                secondpage.IsChecked = true;
            else if (secondpage.IsChecked == true)
                thirdpage.IsChecked = true;
            else if (thirdpage.IsChecked == true)
                fourthpage.IsChecked = true;
        }
    }
}
