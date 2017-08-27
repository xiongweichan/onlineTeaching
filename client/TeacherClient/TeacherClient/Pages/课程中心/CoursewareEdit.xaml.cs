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
using Telerik.Windows.Controls;
using Request = TeacherClient.Contract.Request;
using Reponse = TeacherClient.Contract.Reponse;

namespace TeacherClient.Pages
{
    /// <summary>
    /// CoursewareEdit.xaml 的交互逻辑
    /// </summary>
    public partial class CoursewareEdit : UserControl
    {


        public Request.coursewareAdd Model
        {
            get { return (Request.coursewareAdd)GetValue(ModelProperty); }
            set { SetValue(ModelProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Model.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ModelProperty =
            DependencyProperty.Register("Model", typeof(Request.coursewareAdd), typeof(CoursewareEdit), new PropertyMetadata());



        public CoursewareEdit()
        {
            InitializeComponent();
            Model = new Request.coursewareAdd();
            this.DataContext = this;
        }

        private void UploadCourse_Click(object sender, RoutedEventArgs e)
        {
            Model.url = string.Empty;
        }

        async void Button_Click(object sender, RoutedEventArgs e)
        {
            Model.lec_id = App.CurrentLogin.lec_id;
            Model.token = App.CurrentLogin.token;
            var b = await WebHelper.doPost<Request.coursewareAdd>(Config.Interface_coursewareAdd, Model);
            if (b)
                CourseCenter.Current.ShowCoursewareManager = true;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            CourseCenter.Current.ShowCoursewareManager = true;
        }
    }

    //public class CoursewareModel : ViewModelBase
    //{
    //    string _title;
    //    public string title
    //    {
    //        get { return _title; }
    //        set
    //        {
    //            _title = value;
    //            this.OnPropertyChanged("title");
    //        }
    //    }
    //    string _intro;
    //    public string intro
    //    {
    //        get { return _intro; }
    //        set
    //        {
    //            _intro = value;
    //            this.OnPropertyChanged("intro");
    //        }
    //    }
    //    string _cat_id;
    //    public string cat_id
    //    {
    //        get { return _cat_id; }
    //        set
    //        {
    //            _cat_id = value;
    //            this.OnPropertyChanged("cat_id");
    //        }
    //    }
    //    string _cat_id_1;
    //    public string cat_id_1
    //    {
    //        get { return _cat_id_1; }
    //        set
    //        {
    //            _cat_id_1 = value;
    //            this.OnPropertyChanged("cat_id_1");
    //        }
    //    }
    //    string _cat_id_2;
    //    public string cat_id_2
    //    {
    //        get { return _cat_id_2; }
    //        set
    //        {
    //            _cat_id_2 = value;
    //            this.OnPropertyChanged("cat_id_2");
    //        }
    //    }
    //    string _price;
    //    public string price
    //    {
    //        get { return _price; }
    //        set
    //        {
    //            _price = value;
    //            this.OnPropertyChanged("price");
    //        }
    //    }
    //    string _url;
    //    public string url
    //    {
    //        get { return _url; }
    //        set
    //        {
    //            _url = value;
    //            this.OnPropertyChanged("url");
    //        }
    //    }
    //}
}
