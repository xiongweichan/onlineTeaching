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
using Microsoft.Win32;
using System.IO;

namespace TeacherClient.Pages
{
    /// <summary>
    /// CoursewareEdit.xaml 的交互逻辑
    /// </summary>
    public partial class CoursewareEdit : UserControl
    {


        public CoursewareModel Model
        {
            get { return (CoursewareModel)GetValue(ModelProperty); }
            set { SetValue(ModelProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Model.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ModelProperty =
            DependencyProperty.Register("Model", typeof(CoursewareModel), typeof(CoursewareEdit), new PropertyMetadata());



        public bool IsReadOnly
        {
            get { return (bool)GetValue(IsReadOnlyProperty); }
            set { SetValue(IsReadOnlyProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsReadOnly.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsReadOnlyProperty =
            DependencyProperty.Register("IsReadOnly", typeof(bool), typeof(CoursewareEdit), new PropertyMetadata(false));


        bool _isNew;
        string _id;
        public CoursewareEdit(bool isNew, string id = null)
        {
            InitializeComponent();
            Model = new CoursewareModel();
            _isNew = isNew;
            _id = id;
            this.DataContext = this;
            if (!_isNew)
            {
                Init();
                IsReadOnly = true;
                btn_OK.Visibility = Visibility.Collapsed;
            }
        }

        async void Init()
        {
            MainWindow.Current.IsBusy = true;
            Request.RequestID l = new Contract.Request.RequestID()
            {
                id = _id,
                lec_id = App.CurrentLogin.lec_id,
                token = App.CurrentLogin.token,
            };
            var b = await WebHelper.doPost<Reponse.courseware, Request.RequestID>(Config.Interface_coursewareDetail, l);
            if(b != null)
            {
                Model.cat_id = b.cat_id;
                Model.cat_id_1 = b.cat_id_1;
                Model.cat_id_2 = b.cat_id_2;
                Model.intro = b.intro;
                Model.price = b.price;
                Model.title = b.title;
                Model.url = b.url;
            }
            MainWindow.Current.IsBusy = false;
        }

        private void UploadCourse_Click(object sender, RoutedEventArgs e)
        {
            var path = FileSelectHelper.FileSelecte("课件文件|*.doc;*.docx;*.ppt;*.pptx;*.png;*.bmp;*.jpg;*.txt;*.xls;*.xlsx", 10);
            if (string.IsNullOrWhiteSpace(path)) return;
            Model.url = path;
        }

        async void Button_Click(object sender, RoutedEventArgs e)
        {
            if (_isNew)
            {

                bool bcheck = Model.title.MyIsNullOrWhiteSpace("课件名称");
                bcheck = bcheck && Model.intro.MyIsNullOrWhiteSpace("课件详情");
                bcheck = bcheck && Model.cat_id.MyIsNullOrWhiteSpace("课件类型");
                bcheck = bcheck && Model.cat_id_1.MyIsNullOrWhiteSpace("课件类型");
                bcheck = bcheck && Model.cat_id_2.MyIsNullOrWhiteSpace("课件类型");
                bcheck = bcheck && Model.price.MyIsNullOrWhiteSpace("课件售价");
                bcheck = bcheck && Model.url.MyIsNullOrWhiteSpace("上传课件");
                if (!bcheck) return;

                Request.coursewareAdd l = new Contract.Request.coursewareAdd();
                l.lec_id = App.CurrentLogin.lec_id;
                l.token = App.CurrentLogin.token;
                l.cat_id = Model.cat_id;
                l.cat_id_1 = Model.cat_id_1;
                l.cat_id_2 = Model.cat_id_2;
                l.intro = Model.intro;
                l.price = Model.price;
                l.title = Model.title;
                l.url = Model.url;
                var b = await WebHelper.doPost<string, Request.coursewareAdd>(Config.Interface_coursewareAdd, l);
                if (b != null)
                {
                    Request.getToken t = new Contract.Request.getToken()
                    {
                        lec_id = App.CurrentLogin.lec_id,
                        token = App.CurrentLogin.token,
                        file_name = System.IO.Path.GetFileName(Model.url),
                        id = b
                    };
                    var data = await WebHelper.doPost<Reponse.getToken, Request.getToken>(Config.Interface_coursewareUpload, t);
                    if (data != null)
                    {
                        UploadFileHelper.Instance.Add(Model.url, data.token, data.domain, data.key, UploadFileHelper.EnFileType.Courseware);
                        CourseCenter.Current.ShowCoursewareManager(true);
                    }

                    //UploadFileHelper.Instance.Add(Model.url, b, UploadFileHelper.EnFileType.Courseware, 2, string.Empty);
                    //CourseCenter.Current.ShowCoursewareManager(true);
                }
            }
            else
            {

            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            CourseCenter.Current.ShowCoursewareManager(true);
        }
    }

    public class CoursewareModel : ViewModelBase
    {
        string _title;
        public string title
        {
            get { return _title; }
            set
            {
                _title = value;
                this.OnPropertyChanged("title");
            }
        }
        string _intro;
        public string intro
        {
            get { return _intro; }
            set
            {
                _intro = value;
                this.OnPropertyChanged("intro");
            }
        }
        string _cat_id;
        public string cat_id
        {
            get { return _cat_id; }
            set
            {
                _cat_id = value;
                this.OnPropertyChanged("cat_id");
            }
        }
        string _cat_id_1;
        public string cat_id_1
        {
            get { return _cat_id_1; }
            set
            {
                _cat_id_1 = value;
                this.OnPropertyChanged("cat_id_1");
            }
        }
        string _cat_id_2;
        public string cat_id_2
        {
            get { return _cat_id_2; }
            set
            {
                _cat_id_2 = value;
                this.OnPropertyChanged("cat_id_2");
            }
        }
        string _price;
        public string price
        {
            get { return _price; }
            set
            {
                _price = value;
                this.OnPropertyChanged("price");
            }
        }
        string _url;
        public string url
        {
            get { return _url; }
            set
            {
                _url = value;
                this.OnPropertyChanged("url");
            }
        }
    }
}
