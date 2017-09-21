using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Threading;
using TeacherClient.Core;
using Reponse = TeacherClient.Contract.Reponse;
using Request = TeacherClient.Contract.Request;

namespace TeacherClient.Pages
{
    /// <summary>
    /// MyLive.xaml 的交互逻辑
    /// </summary>
    public partial class MyLive : UserControl
    {
        public LiveModel Live
        {
            get { return (LiveModel)GetValue(ModelProperty); }
            set { SetValue(ModelProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Model.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ModelProperty =
            DependencyProperty.Register("Live", typeof(LiveModel), typeof(RequestLive), new PropertyMetadata());


        public int Status
        {
            get { return (int)GetValue(StatusProperty); }
            set { SetValue(StatusProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Status.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StatusProperty =
            DependencyProperty.Register("Status", typeof(int), typeof(MyLive), new PropertyMetadata(-1, PropertyChangedCallBack));

        private static void PropertyChangedCallBack(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var _this = d as MyLive;
            if (_this.Status == 0)//待直播
            {

            }
            else if (_this.Status == 1)//开始直播
            {

            }
        }

        async void RefreshData(string id)
        {
            MainWindow.Current.IsBusy = true;
            Request.RequestID l = new Request.RequestID();
            l.lec_id = App.CurrentLogin.lec_id;
            l.token = App.CurrentLogin.token;
            l.id = id;
            var d = await WebHelper.doPost<Reponse.liveDetail, Request.RequestID>(Config.Interface_liveDetail, l);
            Live = new LiveModel();
            Live.CatID = d.cat_id;
            Live.CatID1 = d.cat_id_1;
            Live.CatID2 = d.cat_id_2;
            Live.Courseware = d.courseware;
            Live.EndTime = d.end_time.GetTime();
            Live.ID = d.id;
            Live.Image = d.image;
            Live.Intro = d.intro;
            Live.IsFirst = d.is_first;
            Live.Price = d.price;
            Live.RelateLiveID = d.relate_live_id;
            Live.StartTime = d.start_time.GetTime();
            Live.Syllabus = d.syllabus;
            Live.Title = d.title;
            this.DataContext = this;

            MediaHelper.Instance.PushStream(d.publishUrl);
            _timer.Start();

            MainWindow.Current.IsBusy = false;
        }

        DispatcherTimer _timer;
        public MyLive(string id)
        {
            InitializeComponent();
            _timer = new DispatcherTimer(new TimeSpan(0, 0, 0, 0, 40), DispatcherPriority.Send, callback, this.Dispatcher);

            this.Loaded += MyLive_Loaded;
            RefreshData(id);
        }

        private void MyLive_Loaded(object sender, RoutedEventArgs e)
        {
        }
        int msgCounter = 0;
        private void callback(object sender, EventArgs e)
        {
            if (Live == null) return;
#if DEBUG
            if (true)
#else
            if (Live.StartTime >= DateTime.Now)
#endif
            {
                Status = 1;
                //直播结束时间已经到了
#if DEBUG
            if (false)
#else
                if (Live.EndTime < DateTime.Now)
#endif
                {
                    MessageWindow.Alter("提示", "直播结束！");
                    _timer.Stop();
                    return;
                }
                if (msgCounter % 10 == 0)
                    img_main.Source = GetImage();
                //img_main.Source = GetImage(img_main.Tag);
                //img_thumbnail1.Source = GetImage(img_thumbnail1.Tag);
                //tbl_thumbnail1.Text = GetName(img_thumbnail1.Tag);
                //img_thumbnail2.Source = GetImage(img_thumbnail2.Tag);
                //tbl_thumbnail2.Text = GetName(img_thumbnail2.Tag);
                tbl_livelong.Text = (DateTime.Now - Live.StartTime).ToString(@"hh\:mm\:ss");

                msgCounter++;
                if (40 * msgCounter > 1000 * 10)
                {
                    GetLiveWoard();
                    msgCounter = 0;
                }
            }
            else
            {
                Status = 0;
                tbl_CountDown.Text = string.Format("距离您的直播开始时间还有{0}", (Live.StartTime - DateTime.Now).ToString(@"hh\:mm\:ss"));
            }
        }

        async void GetLiveWoard()
        {
            Request.RequestID l = new Contract.Request.RequestID() { id = Live.ID, lec_id = App.CurrentLogin.lec_id, token = App.CurrentLogin.token };
            var r = await WebHelper.doPost<Reponse.liveReward, Request.RequestID>(Config.Interface_liveReward, l);
            if (r != null)
            {
                tbl_studentcount.Text = string.Format("{0}位", r.user_num);
                tbl_giftcount.Text = string.Format("{0}个", r.gift_num);
                tbl_moneycount.Text = string.Format("{0}元", r.reward_money);
                icComment.ItemsSource = r.commentList;
            }
        }

        private void OutLive_Click(object sender, RoutedEventArgs e)
        {
            if (MessageWindow.Alter("提示", "确定要退出直播吗？") == true)
            {
                MediaHelper.Instance.Close();
                LiveCenter.Current.ShowMylive(false);
                _timer.Stop();
            }
        }

        //private void Grid_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        //{
        //    var t = img_main.Tag;
        //    img_main.Tag = img_thumbnail1.Tag;
        //    img_thumbnail1.Tag = t;
        //}

        //private void Grid_PreviewMouseLeftButtonUp2(object sender, MouseButtonEventArgs e)
        //{
        //    var t = img_main.Tag;
        //    img_main.Tag = img_thumbnail2.Tag;
        //    img_thumbnail2.Tag = t;
        //}
        ImageSource GetImage()
        {
            var stream = MediaHelper.Instance.GetImage();
            BitmapImage bi = new BitmapImage();
            bi.BeginInit();
            bi.StreamSource = stream;
            bi.EndInit();
            return bi;
        }
        //大屏、上、下 0：桌面；1：摄像头；2：场景
        int[] _catch = new int[] { 0, 1, 2 };
        private void img_main_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var img = sender as Image;
            var p = e.GetPosition(img);
            if (p.X / img.ActualWidth > (double)(MediaHelper.Instance.localWidth - MediaHelper.Instance.localSmallWidth) / MediaHelper.Instance.localWidth)
            {
                if (p.Y / img.ActualHeight < (double)MediaHelper.Instance.localSmallHeight / MediaHelper.Instance.localHeight)
                //切换1
                {
                    var i = _catch[0];
                    //_catch[0] = _catch[1];
                    //_catch[1] = i;
                    ChangeCatch(i);
                    MediaHelper.Instance.SetLocalStreamIndex(_catch[0]);
                }
                else if (p.Y / img.ActualHeight < 2 * (double)MediaHelper.Instance.localSmallHeight / MediaHelper.Instance.localHeight)
                //切换2
                {
                    var i = _catch[0];
                    //_catch[0] = _catch[2];
                    //_catch[2] = i;
                    ChangeCatch(i);
                    MediaHelper.Instance.SetLocalStreamIndex(_catch[0]);
                }
            }
        }
        void ChangeCatch(int i)
        {
            switch (i)
            {
                case 0:
                    _catch[0] = i;
                    _catch[1] = 1;
                    _catch[1] = 2;
                    break;
                case 1:
                    _catch[0] = i;
                    _catch[1] = 0;
                    _catch[1] = 2;
                    break;
                case 2:
                    _catch[0] = i;
                    _catch[1] = 0;
                    _catch[1] = 1;
                    break;
            }
        }

        private void tbtn_Click(object sender, RoutedEventArgs e)
        {
            var a = GetIsOpen(_catch[0]);
            var b = GetIsOpen(_catch[1]);
            var c = GetIsOpen(_catch[2]);
            MediaHelper.Instance.SetShowSmallStream(a, b, c);
        }
        bool GetIsOpen(int i)
        {
            switch (i)
            {
                case 0:
                    return tbtn_LB.IsChecked == true;
                case 1:
                    return tbtn_LX.IsChecked == true;
                case 2:
                    return tbtn_CJ.IsChecked == true;
            }
            return false;
        }

        //ImageSource GetImage(object tag)
        //{
        //if (tbtn_LB.IsChecked == true && tag.ObjToString() == "1")
        //{
        //    //拿录屏数据
        //    BitmapImage bi = new BitmapImage();
        //    bi.BeginInit();
        //    bi.StreamSource = new MemoryStream(File.ReadAllBytes(@"C:\Users\CHENXIONGWEI\Pictures\Saved Pictures\tooopen_11425855.jpg"));
        //    bi.EndInit();
        //    return bi;
        //}
        //else if (tbtn_LX.IsChecked == true && tag.ObjToString() == "2")
        //{
        //    //拿录像数据
        //    BitmapImage bi = new BitmapImage();
        //    bi.BeginInit();
        //    bi.StreamSource = new MemoryStream(File.ReadAllBytes(@"C:\Users\CHENXIONGWEI\Pictures\Saved Pictures\2.png"));
        //    bi.EndInit();
        //    return bi;
        //}
        //else if (tbtn_CJ.IsChecked == true && tag.ObjToString() == "3")
        //{
        //    //拿场景数据
        //    BitmapImage bi = new BitmapImage();
        //    bi.BeginInit();
        //    bi.StreamSource = new MemoryStream(File.ReadAllBytes(@"C:\Users\CHENXIONGWEI\Pictures\Saved Pictures\3.jpg"));
        //    bi.EndInit();
        //    return bi;
        //}
        //    return null;
        //}
        //string GetName(object tag)
        //{
        //    if (tag.ObjToString() == "1")
        //    {
        //        return "录屏画面";
        //    }
        //    else if (tag.ObjToString() == "2")
        //    {
        //        return "录像画面";
        //    }
        //    else
        //    {
        //        return "场景画面";
        //    }

        //}
    }
}
