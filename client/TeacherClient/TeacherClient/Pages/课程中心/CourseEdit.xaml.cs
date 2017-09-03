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

        bool _isNew;
        string _id;
        public CourseEdit(bool isNew, string id = null)
        {
            InitializeComponent();
            Model = new CourseEditModel();
            _isNew = isNew;
            _id = id;
        }

        async void ButtonOK_Click(object sender, RoutedEventArgs e)
        {
            if (_isNew)
            {
                Request.courseAdd request = new Request.courseAdd();
                request.lec_id = App.CurrentLogin.lec_id;
                request.token = App.CurrentLogin.token;
                request.title = Model.Name;
                request.cat_id = Model.CatID1;
                request.cat_id_1 = Model.CatID2;
                request.cat_id_2 = Model.CatID3;
                request.image = Model.Image;
                request.intro = Model.Detail;
                var b = await WebHelper.doPost<Request.courseAdd>(Config.Interface_courseAdd, request);
                if (b)
                {
                    MessageWindow.Alter("提示", "添加成功");
                    CourseCenter.Current.ShowCourseManager( true);
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

        private void UploadCourse_Click(object sender, RoutedEventArgs e)
        {

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
        ObservableCollection<CourseModel> _ManyCourse;
        public ObservableCollection<CourseModel> ManyCourse
        {
            get { return _ManyCourse; }
            set
            {
                _ManyCourse = value;
                this.OnPropertyChanged("ManyCourse");
            }
        }
        CourseModel _OneCourse;
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
        string _CourseNumber;
        public string CourseNumber
        {
            get { return _CourseNumber; }
            set
            {
                _CourseNumber = value;
                this.OnPropertyChanged("CourseNumber");
            }
        }
        string _Price;
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
