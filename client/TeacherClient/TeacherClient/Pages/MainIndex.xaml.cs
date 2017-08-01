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
    /// MainIndex.xaml 的交互逻辑
    /// </summary>
    public partial class MainIndex : UserControl
    {
        public MainIndex()
        {
            InitializeComponent();
        }

        private void StackPanel_Checked(object sender, RoutedEventArgs e)
        {
            if (e.Source is RadioButton && (e.Source as RadioButton).Content != null)
            {
                var tag = (e.Source as RadioButton).Tag.ToString();
                switch (tag)
                {
                    case "1":
                        frame.Source = new Uri("/Pages/TogetherInfo.xaml", UriKind.Relative);
                        break;
                    case "2":
                        frame.Source = new Uri("/Pages/TogetherInfo.xaml", UriKind.Relative);
                        break;
                    case "3":
                        frame.Source = new Uri("/Pages/TogetherInfo.xaml", UriKind.Relative);
                        break;
                }
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            rb_info.IsChecked = true;
        }
    }
}
