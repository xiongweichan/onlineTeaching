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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Request = TeacherClient.Contract.Request;

namespace TeacherClient.Pages
{
    /// <summary>
    /// AddManyCourse.xaml 的交互逻辑
    /// </summary>
    public partial class AddManyCourse : UserControl
    {
        public AddManyCourse()
        {
            InitializeComponent();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

            var source = icCourses.ItemsSource as ObservableCollection<Request.course>;
            var tag = (sender as Control).Tag as Request.course;
            source.Remove(tag);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var list = this.DataContext as ObservableCollection<Request.course>;
            list.Add(new Request.course());
        }
    }
}
