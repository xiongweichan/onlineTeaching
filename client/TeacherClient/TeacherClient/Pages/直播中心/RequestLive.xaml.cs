using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
using Reponse = TeacherClient.Contract.Reponse;
using Request = TeacherClient.Contract.Request;

namespace TeacherClient.Pages
{
    /// <summary>
    /// RequestLive.xaml 的交互逻辑
    /// </summary>
    public partial class RequestLive : UserControl
    {


        public LiveModel Model
        {
            get { return (LiveModel)GetValue(ModelProperty); }
            set { SetValue(ModelProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Model.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ModelProperty =
            DependencyProperty.Register("Model", typeof(LiveModel), typeof(RequestLive), new PropertyMetadata());



        public RequestLive()
        {
            InitializeComponent();
            Model = new LiveModel() { Price = "0", RelateLiveID = "0" };
            this.DataContext = this;
            Init();
        }

        async void Init()
        {
            MainWindow.Current.IsBusy = true;
            Request.requestlivelist l = new Request.requestlivelist() { lec_id = App.CurrentLogin.lec_id, token = App.CurrentLogin.token, page = 0, pageSize = 100, list_type = "0" };

            var t = await WebHelper.doPost<Reponse.listData<Reponse.live>, Request.requestlivelist>(Config.Interface_liveList, l);
            if (t != null)
            {
                //pagerData.TotalCount = t.totalCount;
                cb_relatelive.ItemsSource = t.list;
            }

            MainWindow.Current.IsBusy = false;
        }

        void UploadCourse_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Multiselect = false;
            dialog.Filter = "课件文件|*.png;*.bmp;*.jpg;*.doc;*.docx;*.ppt;*.pptx";
            if (dialog.ShowDialog() == true)
            {
                var path = dialog.FileName;
                var s = dialog.OpenFile();
                if (s.Length > 10 * 1024 * 1024)
                {
                    MessageWindow.Alter("提示", "文件过大");
                    return;
                }

                Model.Courseware = dialog.FileName;
            }
        }
        CancellationTokenSource _token;
        async void btnNext_Click(object sender, RoutedEventArgs e)
        {
            if (firstpage.IsChecked == true)
                secondpage.IsChecked = true;
            else if (secondpage.IsChecked == true)
                thirdpage.IsChecked = true;
            else if (thirdpage.IsChecked == true)
            {
                MainWindow.Current.IsBusy = true;
                Request.liveAdd l = new Request.liveAdd();
                l.cat_id = Model.CatID;
                l.cat_id_1 = Model.CatID1;
                l.cat_id_2 = Model.CatID2;
                l.courseware = Model.Courseware;
                l.end_time = Model.EndTime.ConvertDateTimeInt().ToString();
                l.image = Model.Image;
                l.intro = Model.Intro;
                l.is_first = Model.IsFirst;
                l.lec_id = App.CurrentLogin.lec_id;
                l.price = Model.Price;
                l.relate_live_id = Model.RelateLiveID;
                l.start_time = Model.StartTime.ConvertDateTimeInt().ToString();
                l.syllabus = Model.Syllabus;
                l.title = Model.Title;
                l.token = App.CurrentLogin.token;
                var t = await WebHelper.doPost<string, Request.liveAdd>(Config.Interface_liveAdd, l);
                if (t != null)
                {
                    Request.getToken gt = new Contract.Request.getToken()
                    {
                        lec_id = App.CurrentLogin.lec_id,
                        token = App.CurrentLogin.token,
                        file_name = System.IO.Path.GetFileName(Model.Courseware),
                        id = t
                    };
                    var data = await WebHelper.doPost<Reponse.getToken, Request.getToken>(Config.Interface_liveWareUpload, gt);
                    if (data != null)
                    {
                        UploadFileHelper.Instance.Add(Model.Courseware, data.token, data.domain, data.key, UploadFileHelper.EnFileType.Courseware);
                        fourthpage.IsChecked = true;
                        _token = new CancellationTokenSource();
                        await Task.Factory.StartNew(new Action(() =>
                        {
                            for (int i = 0; i < 5; i++)
                            {
                                _token.Token.ThrowIfCancellationRequested();
                                Thread.Sleep(1000);
                            }
                            _token.Token.ThrowIfCancellationRequested();
                            this.Dispatcher.Invoke(new Action(() =>
                            {
                                LiveCenter.Current.Type = 0;
                            }));
                        }), _token.Token);
                    }
                }

                MainWindow.Current.IsBusy = false;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _token.Cancel();
            LiveCenter.Current.Type = 0;
        }

        async void UploadImage_Click(object sender, RoutedEventArgs e)
        {
            var str = await UploadImageHelper.UploadImage();
            if (!string.IsNullOrEmpty(str))
                Model.Image = str;
        }
    }
    public class LiveModel : ViewModelBase
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
        string _Title;
        public string Title
        {
            get { return _Title; }
            set
            {
                _Title = value;
                this.OnPropertyChanged("Title");
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
        string _CatID;
        public string CatID
        {
            get { return _CatID; }
            set
            {
                _CatID = value;
                this.OnPropertyChanged("CatID");
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
        string _IsFirst;
        public string IsFirst
        {
            get { return _IsFirst; }
            set
            {
                _IsFirst = value;
                this.OnPropertyChanged("IsFirst");
            }
        }
        string _Intro;
        public string Intro
        {
            get { return _Intro; }
            set
            {
                _Intro = value;
                this.OnPropertyChanged("Intro");
            }
        }
        string _Syllabus;
        public string Syllabus
        {
            get { return _Syllabus; }
            set
            {
                _Syllabus = value;
                this.OnPropertyChanged("Syllabus");
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
        string _RelateLiveID;
        public string RelateLiveID
        {
            get { return _RelateLiveID; }
            set
            {
                _RelateLiveID = value;
                this.OnPropertyChanged("RelateLiveID");
            }
        }
        DateTime _StartTime;
        public DateTime StartTime
        {
            get { return _StartTime; }
            set
            {
                _StartTime = value;
                this.OnPropertyChanged("StartTime");
            }
        }
        DateTime _EndTime;
        public DateTime EndTime
        {
            get { return _EndTime; }
            set
            {
                _EndTime = value;
                this.OnPropertyChanged("EndTime");
            }
        }
        string _Courseware;
        public string Courseware
        {
            get { return _Courseware; }
            set
            {
                _Courseware = value;
                this.OnPropertyChanged("Courseware");
            }
        }

    }
}
