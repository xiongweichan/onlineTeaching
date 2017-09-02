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
using Reponse = TeacherClient.Contract.Reponse;
using Request = TeacherClient.Contract.Request;

namespace TeacherClient.Pages
{
    /// <summary>
    /// AddOneOffCourse.xaml 的交互逻辑
    /// </summary>
    public partial class AddOneOffCourse : UserControl
    {
        public string SchoolImage
        {
            get { return (string)GetValue(SchoolImageProperty); }
            set { SetValue(SchoolImageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SchoolImage.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SchoolImageProperty =
            DependencyProperty.Register("SchoolImage", typeof(string), typeof(AddOneOffCourse), new PropertyMetadata(PropertyChangedCallback));

        static void PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.Property == AddOneOffCourse.SchoolImageProperty)
                ((d as AddOneOffCourse).DataContext as dynamic).school_image = (d as AddOneOffCourse).SchoolImage;
        }
        public AddOneOffCourse()
        {
            InitializeComponent();
            Init();
            this.SetBinding(AddOneOffCourse.SchoolImageProperty, new Binding("school_image") { Mode = BindingMode.OneTime });

        }

        private void Init()
        {
            cb_province.ItemsSource = SystemInit.Instance.Regions;
        }

        async void btnUpload_Click(object sender, RoutedEventArgs e)
        {
            var str = await UploadImageHelper.UploadImage();
            if (!string.IsNullOrEmpty(str))
                SchoolImage = str;
        }

        private void cb_province_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cb_province.SelectedItem is Reponse.region)
            {
                cb_city.ItemsSource = (cb_province.SelectedItem as Reponse.region).data;
            }
        }

        private void cb_city_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cb_city.SelectedItem is Reponse.region)
            {
                cb_district.ItemsSource = (cb_city.SelectedItem as Reponse.region).data;
            }
        }
    }
}
