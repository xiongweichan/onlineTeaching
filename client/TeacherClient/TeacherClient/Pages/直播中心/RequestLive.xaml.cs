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
using TeacherClient.Core;
using Reponse = TeacherClient.Contract.Reponse;
using Request = TeacherClient.Contract.Request;
using System.IO;

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

        public bool IsNew { get; set; }
        string _ID;
        public string ID
        {
            get { return _ID; }
            set
            {
                _ID = value;
                RefreshData();
            }
        }

        async void RefreshData()
        {
            MainWindow.Current.IsBusy = true;
            Request.RequestID l = new Request.RequestID();
            l.lec_id = App.CurrentLogin.lec_id;
            l.token = App.CurrentLogin.token;
            l.id = _ID;
            var d = await WebHelper.doPost<Reponse.liveDetail, Request.RequestID>(Config.Interface_liveDetail, l);
            Model.CatID = d.cat_id;
            Model.CatID1 = d.cat_id_1;
            Model.CatID2 = d.cat_id_2;
            Model.Courseware = d.courseware;
            Model.EndTime = d.end_time.GetTime();
            Model.ID = d.id;
            Model.Image = d.image;
            Model.Intro = d.intro;
            Model.IsFirst = d.is_first;
            Model.Price = d.price;
            Model.RelateLiveID = d.relate_live_id;
            Model.StartTime = d.start_time.GetTime();
            Model.Syllabus = d.syllabus;
            Model.Title = d.title;
            Model.PushUrl = d.publishUrl;
            MainWindow.Current.IsBusy = false;
        }

        public RequestLive()
        {
            IsNew = true;
            InitializeComponent();
            Model = new LiveModel() { Price = "0", RelateLiveID = "0", StartTime = DateTime.Now, EndTime = DateTime.Now };
            this.DataContext = this;
            Init();
            this.Loaded += RequestLive_Loaded;
        }

        private void RequestLive_Loaded(object sender, RoutedEventArgs e)
        {
            //var edit = new MyHtmlEditor() { Width = 800, Height = 200 };
            //edit.SetBinding(MyHtmlEditor.TextProperty, new Binding { Path = new PropertyPath("Model.Intro") });
            //sp_liveDetail.Children.Add(edit);
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
            var path = FileSelectHelper.FileSelecte("课件文件|*.png;*.bmp;*.jpg;*.doc;*.docx;*.ppt;*.pptx", 10);
            if (string.IsNullOrWhiteSpace(path)) return;

            Model.Courseware = path;            
        }
        CancellationTokenSource _token;
        async void btnNext_Click(object sender, RoutedEventArgs e)
        {
            if (firstpage.IsChecked == true)
            {
                if (string.IsNullOrWhiteSpace(Model.Title))
                    MessageWindow.Alter("提示", "标题不能为空");
                else if (string.IsNullOrWhiteSpace(Model.Image))
                    MessageWindow.Alter("提示", "课程封面不能为空");
                else if (string.IsNullOrWhiteSpace(Model.IsFirst))
                    MessageWindow.Alter("提示", "直播次数不能为空");
                else
                    secondpage.IsChecked = true;
            }
            else if (secondpage.IsChecked == true)
            {
                if (string.IsNullOrWhiteSpace(Model.CatID) ||
                    string.IsNullOrWhiteSpace(Model.CatID1) ||
                    string.IsNullOrWhiteSpace(Model.CatID2))
                    MessageWindow.Alter("提示", "直播类型不能为空");
                else if(string.IsNullOrWhiteSpace(Model.Intro))
                    MessageWindow.Alter("提示", "直播描述不能为空");
                else if(string.IsNullOrWhiteSpace(Model.Courseware))
                    MessageWindow.Alter("提示", "直播课件不能为空");
                else if(string.IsNullOrWhiteSpace(Model.Price))
                    MessageWindow.Alter("提示", "直播售价不能为空");
                else if(string.IsNullOrWhiteSpace(Model.Intro))
                    MessageWindow.Alter("提示", "直播详情不能为空");
                else
                    thirdpage.IsChecked = true;
            }
            else if (thirdpage.IsChecked == true)
            {
                MainWindow.Current.IsBusy = true;
                if (IsNew)
                {
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
                            aliyun = "1",
                            id = t
                        };
                        var data = await WebHelper.doPost<Reponse.getToken, Request.getToken>(Config.Interface_liveWareUpload, gt);
                        if (data != null)
                        {
                            AliyunHelper.Instance.Add(Model.Courseware, data.aliyun.accessid, data.aliyun.signature, data.aliyun.host, data.domain, data.key, AliyunHelper.EnFileType.Courseware);
                            //UploadFileHelper.Instance.Add(Model.Courseware, data.token, data.domain, data.key, UploadFileHelper.EnFileType.Courseware);
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
                        //UploadFileHelper.Instance.Add(Model.Courseware, t, UploadFileHelper.EnFileType.Courseware, 3, string.Empty );
                        //fourthpage.IsChecked = true;
                        //_token = new CancellationTokenSource();
                        //await Task.Factory.StartNew(new Action(() =>
                        //{
                        //    for (int i = 0; i < 5; i++)
                        //    {
                        //        _token.Token.ThrowIfCancellationRequested();
                        //        Thread.Sleep(1000);
                        //    }
                        //    _token.Token.ThrowIfCancellationRequested();
                        //    this.Dispatcher.Invoke(new Action(() =>
                        //    {
                        //        LiveCenter.Current.Type = 0;
                        //    }));
                        //}), _token.Token);
                    }
                }
                else
                {
                    MessageWindow.Alter("提示", "接口未实现");
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
                if (EndTime < _StartTime)
                    EndTime = _StartTime;
                this.OnPropertyChanged("StartTime");
            }
        }
        DateTime _EndTime;
        public DateTime EndTime
        {
            get { return _EndTime; }
            set
            {
                _EndTime = value < _StartTime ? _StartTime : value;
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

        public string PushUrl
        {
            get
            {
                return _PushUrl;
            }
            set
            {
                _PushUrl = value;
                this.OnPropertyChanged("PushUrl");
            }
        }

        string _PushUrl;
    }
}
