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
using Request = TeacherClient.Contract.Request;
using Reponse = TeacherClient.Contract.Reponse;
using Telerik.Windows.Controls;
using System.Collections.ObjectModel;
using TeacherClient.Contract;

namespace TeacherClient.Pages
{
    /// <summary>
    /// CourseEdit.xaml 的交互逻辑
    /// </summary>
    public partial class CourseEdit : UserControl
    {
        public CourseEditModel Model
        {
            get { return (CourseEditModel)GetValue(ModelProperty); }
            set { SetValue(ModelProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Model.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ModelProperty =
            DependencyProperty.Register("Model", typeof(CourseEditModel), typeof(CourseEdit), new PropertyMetadata());



        public bool IsReadOnly
        {
            get { return (bool)GetValue(IsReadOnlyProperty); }
            set { SetValue(IsReadOnlyProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsReadOnly.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsReadOnlyProperty =
            DependencyProperty.Register("IsReadOnly", typeof(bool), typeof(CourseEdit), new PropertyMetadata(false));



        bool _isNew;
        string _id;
        public CourseEdit(bool isNew, string id = null)
        {
            InitializeComponent();
            Model = new CourseEditModel();
            _isNew = isNew;
            _id = id;
            this.DataContext = this;
            if (!_isNew)
            {
                Init();
                btn_OK.Visibility = Visibility.Collapsed;
                page_one.IsEnabled = false;
                page_many.IsEnabled = false;
                IsReadOnly = true;
            }
        }

        async void Init()
        {
            MainWindow.Current.IsBusy = true;
            Request.RequestID l = new Request.RequestID() { lec_id = App.CurrentLogin.lec_id, token = App.CurrentLogin.token, id = _id };
            var detail = await WebHelper.doPost<Reponse.courseDetail, Request.RequestID>(Config.Interface_courseDetail_LecCourse, l);
            if (detail != null)
            {
                Model.CatID1 = detail.cat_id;
                Model.CatID2 = detail.cat_id_1;
                Model.CatID3 = detail.cat_id_2;
                Model.CourseType = detail.course_type;
                Model.Detail = detail.intro;
                Model.id = detail.id;
                Model.Image = detail.image;
                Model.Name = detail.title;
                if (Model.CourseType == "0")
                    Model.OneCourse = Transfer(detail.lessonList[0], 1);
                else
                {
                    for (int i = 0; i < Model.ManyCourse.Count; i++)
                    {
                        Model.ManyCourse.Add(Transfer(detail.lessonList[i], i + 1));
                    }
                }
            }


            MainWindow.Current.IsBusy = false;
        }

        CourseModel Transfer(Reponse.courseDetailLesson d, int i)
        {
            CourseModel m = new CourseModel();
            m.CourseID = d.course_id;
            m.CourseNumber = (i + 1).ToString();
            m.Document = d.courseware;
            m.DocumentName = d.courseware_file_name;
            m.Price = d.price;
            m.Vedio = d.video;
            m.VedioName = d.video_file_name;
            return m;
        }

        async void ButtonOK_Click(object sender, RoutedEventArgs e)
        {
            if (_isNew)
            {
                bool bcheck = Model.Name.MyIsNullOrWhiteSpace("课程名称");
                bcheck = bcheck && Model.Image.MyIsNullOrWhiteSpace("课程封面");
                bcheck = bcheck && Model.Detail.MyIsNullOrWhiteSpace("课程详情");
                bcheck = bcheck && Model.CatID1.MyIsNullOrWhiteSpace("课程类型");
                bcheck = bcheck && Model.CatID2.MyIsNullOrWhiteSpace("课程类型");
                bcheck = bcheck && Model.CatID3.MyIsNullOrWhiteSpace("课程类型");
                bcheck = bcheck && Model.CourseType.MyIsNullOrWhiteSpace("课程数量");
                if (Model.CourseType == "0")
                {
                    bcheck = bcheck && Model.OneCourse.Price.MyIsNullOrWhiteSpace("课程售价");
                    bcheck = bcheck && Model.OneCourse.Vedio.MyIsNullOrWhiteSpace("课程视频");
                    bcheck = bcheck && Model.OneCourse.Document.MyIsNullOrWhiteSpace("上传课程");
                }
                else
                {
                    foreach (var item in Model.ManyCourse)
                    {
                        bcheck = bcheck && item.Price.MyIsNullOrWhiteSpace("课程售价");
                        bcheck = bcheck && item.Vedio.MyIsNullOrWhiteSpace("课程视频");
                        bcheck = bcheck && item.Document.MyIsNullOrWhiteSpace("上传课程");
                    }
                }
                if (!bcheck) return;
                Request.courseAdd request = new Request.courseAdd();
                request.lec_id = App.CurrentLogin.lec_id;
                request.token = App.CurrentLogin.token;
                request.title = Model.Name;
                request.cat_id = Model.CatID1;
                request.cat_id_1 = Model.CatID2;
                request.cat_id_2 = Model.CatID3;
                request.image = Model.Image;
                request.intro = Model.Detail;
                request.course_type = Model.CourseType;
                request.lessonList = Model.CourseType == "0" ?
                    new List<courseAddlesson> { new courseAddlesson
                    {
                        lesson_number = Model.OneCourse.CourseNumber,
                        price = Model.OneCourse.Price,
                        courseware_file_name = Model.OneCourse.DocumentName,
                        video_file_name = Model.OneCourse.VedioName,
                    } } :
                    Model.ManyCourse.Select(T => new courseAddlesson
                    {
                        lesson_number = (1 + Model.ManyCourse.IndexOf(T)).ToString(),
                        price = T.Price,
                        courseware_file_name = T.DocumentName,
                        video_file_name = T.VedioName,
                    }).ToList();
                var b = await WebHelper.doPost<string, Request.courseAdd>(Config.Interface_courseAdd, request);
                if (b != null)
                {
                    //
                    List<CourseModel> f = new List<CourseModel>();
                    if (Model.CourseType == "0")
                        f.Add(Model.OneCourse);
                    else f.AddRange(Model.ManyCourse);

                    Request.RequestID l = new Request.RequestID() { lec_id = App.CurrentLogin.lec_id, token = App.CurrentLogin.token, id = b };
                    var detail = await WebHelper.doPost<Reponse.courseDetail, Request.RequestID>(Config.Interface_courseDetail_LecCourse, l);
                    if (detail == null) return;
                    foreach (var item in detail.lessonList)
                    {
                        Request.courseLessonUpload t = new Contract.Request.courseLessonUpload()
                        {
                            lec_id = App.CurrentLogin.lec_id,
                            token = App.CurrentLogin.token,
                            file_name = item.video_file_name,
                            id = item.id,
                            type = "1"
                        };
                        Request.courseLessonUpload t1 = new Contract.Request.courseLessonUpload()
                        {
                            lec_id = App.CurrentLogin.lec_id,
                            token = App.CurrentLogin.token,
                            file_name = item.courseware_file_name,
                            id = item.id,
                            type = "0"
                        };
                        var data = await WebHelper.doPost<Reponse.getToken, Request.courseLessonUpload>(Config.Interface_courseLessonUpload, t);

                        var data1 = await WebHelper.doPost<Reponse.getToken, Request.courseLessonUpload>(Config.Interface_courseLessonUpload, t1);
                        if (data != null && data1 != null)
                        {
                            var v = f.FirstOrDefault(T => T.VedioName == item.video_file_name).Vedio;
                            var d = f.FirstOrDefault(T => T.DocumentName == item.courseware_file_name).Document;

                            AliyunHelper.Instance.Add(v, data.aliyun.accessid, data.aliyun.signature, data.aliyun.host, data.domain, data.key, AliyunHelper.EnFileType.Course);
                            AliyunHelper.Instance.Add(d, data.aliyun.accessid, data.aliyun.signature, data.aliyun.host, data.domain, data.key, AliyunHelper.EnFileType.Course);

                            //UploadFileHelper.Instance.Add(v, data.token, data.domain, data.key, UploadFileHelper.EnFileType.Course);
                            //UploadFileHelper.Instance.Add(d, data.token, data.domain, data.key, UploadFileHelper.EnFileType.Course);

                        }
                        //var v = f.FirstOrDefault(T => T.VedioName == item.video_file_name);
                        //var d = f.FirstOrDefault(T => T.DocumentName == item.courseware_file_name);
                        //UploadFileHelper.Instance.Add(v.Vedio, item.id, UploadFileHelper.EnFileType.Course, 1, "1");
                        //UploadFileHelper.Instance.Add(d.Document, item.id, UploadFileHelper.EnFileType.Course, 1, "0");

                    }

                    CourseCenter.Current.ShowCourseManager(true);
                }
            }
            else
            {
                //TODO:编辑
            }
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            CourseCenter.Current.ShowCourseManager(true);
        }

        async void UploadImage_Click(object sender, RoutedEventArgs e)
        {
            var str = await UploadImageHelper.UploadImage();
            if (!string.IsNullOrEmpty(str))
            {
                Model.Image = str;
            }
        }
    }

    public class CourseEditModel : ViewModelBase
    {
        string _id;
        public string id
        {
            get { return _id; }
            set
            {
                _id = value;
                this.OnPropertyChanged("id");
            }
        }
        string _Name;
        public string Name
        {
            get { return _Name; }
            set
            {
                _Name = value;
                this.OnPropertyChanged("Name");
            }
        }
        string _Image;
        public string Image
        {
            get { return _Image; }
            set
            {
                _Image = value;
                this.OnPropertyChanged("Image");
            }
        }
        string _CatID1;
        public string CatID1
        {
            get { return _CatID1; }
            set
            {
                _CatID1 = value;
                this.OnPropertyChanged("CatID1");
            }
        }
        string _CatID2;
        public string CatID2
        {
            get { return _CatID2; }
            set
            {
                _CatID2 = value;
                this.OnPropertyChanged("CatID2");
            }
        }
        string _CatID3;
        public string CatID3
        {
            get { return _CatID3; }
            set
            {
                _CatID3 = value;
                this.OnPropertyChanged("CatID3");
            }
        }
        string _Detail;
        public string Detail
        {
            get { return _Detail; }
            set
            {
                _Detail = value;
                this.OnPropertyChanged("Detail");
            }
        }
        string _CourseType;
        public string CourseType
        {
            get { return _CourseType; }
            set
            {
                _CourseType = value;
                this.OnPropertyChanged("CourseType");
            }
        }
        ObservableCollection<CourseModel> _ManyCourse = new ObservableCollection<CourseModel>();
        public ObservableCollection<CourseModel> ManyCourse
        {
            get { return _ManyCourse; }
            set
            {
                _ManyCourse = value;
                this.OnPropertyChanged("ManyCourse");
            }
        }
        CourseModel _OneCourse = new CourseModel();
        public CourseModel OneCourse
        {
            get { return _OneCourse; }
            set
            {
                _OneCourse = value;
                this.OnPropertyChanged("OneCourse");
            }
        }
    }
    public class CourseModel : ViewModelBase
    {
        string _ID;
        public string ID
        {
            get { return _ID; }
            set
            {
                _ID = value;
                this.OnPropertyChanged("ID");
            }
        }
        string _CourseID;
        public string CourseID
        {
            get { return _CourseID; }
            set
            {
                _CourseID = value;
                this.OnPropertyChanged("CourseID");
            }
        }
        string _CourseNumber = "0";
        public string CourseNumber
        {
            get { return _CourseNumber; }
            set
            {
                _CourseNumber = value;
                this.OnPropertyChanged("CourseNumber");
            }
        }
        string _Price = "0";
        public string Price
        {
            get { return _Price; }
            set
            {
                _Price = value;
                this.OnPropertyChanged("Price");
            }
        }
        string _Vedio;
        public string Vedio
        {
            get { return _Vedio; }
            set
            {
                _Vedio = value;
                this.OnPropertyChanged("Vedio");
            }
        }
        string _VedioName;
        public string VedioName
        {
            get { return _VedioName; }
            set
            {
                _VedioName = value;
                this.OnPropertyChanged("VedioName");
            }
        }
        string _Document;
        public string Document
        {
            get { return _Document; }
            set
            {
                _Document = value;
                this.OnPropertyChanged("Document");
            }
        }
        string _DocumentName;
        public string DocumentName
        {
            get { return _DocumentName; }
            set
            {
                _DocumentName = value;
                this.OnPropertyChanged("DocumentName");
            }
        }
    }
}
