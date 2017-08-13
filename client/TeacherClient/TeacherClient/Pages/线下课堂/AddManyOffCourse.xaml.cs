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
using Reponse = TeacherClient.Contract.Reponse;
using Request = TeacherClient.Contract.Request;

namespace TeacherClient.Pages
{
    /// <summary>
    /// AddManyOffCourse.xaml 的交互逻辑
    /// </summary>
    public partial class AddManyOffCourse : UserControl
    {
        public AddManyOffCourse()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var list = this.DataContext as ObservableCollection<Request.lesson>;
            list.Add(new Contract.Request.lesson());
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var source = icCourses.ItemsSource as ObservableCollection<Request.lesson>;
            var tag = (sender as Control).Tag as Request.lesson;
            source.Remove(tag);
        }
    }
}
