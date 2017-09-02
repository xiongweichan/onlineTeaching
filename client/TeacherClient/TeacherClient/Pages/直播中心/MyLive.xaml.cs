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

namespace TeacherClient.Pages
{
    /// <summary>
    /// MyLive.xaml 的交互逻辑
    /// </summary>
    public partial class MyLive : UserControl
    {


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

        public Contract.Reponse.live Live { get; set; }

        DispatcherTimer _timer;
        public MyLive(Contract.Reponse.live live)
        {
            Live = live;
            InitializeComponent();

            this.Loaded += MyLive_Loaded;
            this.DataContext = this;
        }

        private void MyLive_Loaded(object sender, RoutedEventArgs e)
        {
            _timer = new DispatcherTimer(new TimeSpan(0, 0, 0, 0, 40), DispatcherPriority.Send, callback, this.Dispatcher);
            _timer.Start();
        }
        int msgCounter = 0;
        private void callback(object sender, EventArgs e)
        {
            if (true)//(_live.start_time.GetTime() >= DateTime.Now)
            {
                Status = 1;
                //直播结束时间已经到了
                //if (_live.end_time.GetTime() < DateTime.Now)
                //{
                //    MessageWindow.Alter("提示", "直播结束！");
                //    _timer.Stop();
                //    return;
                //}
                img_main.Source = GetImage(img_main.Tag);
                img_thumbnail1.Source = GetImage(img_thumbnail1.Tag);
                tbl_thumbnail1.Text = GetName(img_thumbnail1.Tag);
                img_thumbnail2.Source = GetImage(img_thumbnail2.Tag);
                tbl_thumbnail2.Text = GetName(img_thumbnail2.Tag);
                tbl_livelong.Text = (DateTime.Now - Live.start_time.GetTime()).ToString(@"hh\:mm\:ss");

                msgCounter++;
                if (40 * msgCounter > 1000 * 10)
                {
                    msgCounter = 0;
                    tbl_studentcount.Text = string.Format("{0}位", 148);
                    tbl_giftcount.Text = string.Format("{0}个", 52);
                    tbl_moneycount.Text = string.Format("{0}元", 24);
                }
            }
            else
            {
                Status = 0;
                tbl_CountDown.Text = string.Format("距离您的直播开始时间还有{0}", (Live.start_time.GetTime() - DateTime.Now).ToString(@"hh\:mm\:ss"));
            }
        }

        private void OutLive_Click(object sender, RoutedEventArgs e)
        {
            if (MessageWindow.Alter("提示", "确定要退出直播吗？") == true)
            {
                LiveCenter.Current.ShowMylive = false;
                _timer.Stop();
            }
        }

        private void Grid_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var t = img_main.Tag;
            img_main.Tag = img_thumbnail1.Tag;
            img_thumbnail1.Tag = t;
        }

        private void Grid_PreviewMouseLeftButtonUp2(object sender, MouseButtonEventArgs e)
        {
            var t = img_main.Tag;
            img_main.Tag = img_thumbnail2.Tag;
            img_thumbnail2.Tag = t;
        }

        ImageSource GetImage(object tag)
        {
            var stream = MediaHelper.GetImage();
            if (tbtn_LB.IsChecked == true && tag.ObjToString() == "1")
            {
                //拿录屏数据
                BitmapImage bi = new BitmapImage();
                bi.BeginInit();
                bi.StreamSource = new MemoryStream(File.ReadAllBytes(@"C:\Users\CHENXIONGWEI\Pictures\Saved Pictures\tooopen_11425855.jpg"));
                bi.EndInit();
                return bi;
            }
            else if (tbtn_LX.IsChecked == true && tag.ObjToString() == "2")
            {
                //拿录像数据
                BitmapImage bi = new BitmapImage();
                bi.BeginInit();
                bi.StreamSource = new MemoryStream(File.ReadAllBytes(@"C:\Users\CHENXIONGWEI\Pictures\Saved Pictures\2.png"));
                bi.EndInit();
                return bi;
            }
            else if (tbtn_CJ.IsChecked == true && tag.ObjToString() == "3")
            {
                //拿场景数据
                BitmapImage bi = new BitmapImage();
                bi.BeginInit();
                bi.StreamSource = new MemoryStream(File.ReadAllBytes(@"C:\Users\CHENXIONGWEI\Pictures\Saved Pictures\3.jpg"));
                bi.EndInit();
                return bi;
            }
            return null;
        }
        string GetName(object tag)
        {
            if (tag.ObjToString() == "1")
            {
                return "录屏画面";
            }
            else if (tag.ObjToString() == "2")
            {
                return "录像画面";
            }
            else
            {
                return "场景画面";
            }

        }
    }
}
