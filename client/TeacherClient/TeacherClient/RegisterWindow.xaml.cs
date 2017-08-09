using Microsoft.Win32;
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
using System.Windows.Shapes;
using System.Windows.Threading;
using Telerik.Windows.Controls;
using Reponse = TeacherClient.Contract.Reponse;
using Request = TeacherClient.Contract.Request;

namespace TeacherClient
{
    /// <summary>
    /// RegisterWindow.xaml 的交互逻辑
    /// </summary>
    public partial class RegisterWindow : WindowBase
    {
        public Request.register Model { get; set; }
        public RegisterWindow()
        {
            InitializeComponent();
            Model = new Contract.Request.register();
            this.DataContext = this;
            this.IsBusy = false;
            _timer = new DispatcherTimer(new TimeSpan(0, 0, 1), DispatcherPriority.Normal, Time_Tick, this.Dispatcher);
        }

        private void Time_Tick(object sender, EventArgs e)
        {
            Count--;
            run_time.Text = Count.ToString();
            if (Count == 0)
            {
                BtnSure_Click(null, null);
                _timer.Stop();
            }
        }

        private void BtnSure_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Run_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        DispatcherTimer _timer;
        public int Count { get; set; }

        async void btnNextStep_Click(object sender, RoutedEventArgs e)
        {
            if (firstpage.IsChecked.HasValue && firstpage.IsChecked.Value)
            {
                Model.password = pwd_new.Password;
                if (Model.password != pwd_repeat.Password)
                    MessageWindow.Alter("提示", "两次密码输入不一致");
                else
                    secondpage.IsChecked = true;
            }
            else if (secondpage.IsChecked.HasValue && secondpage.IsChecked.Value)
            {
                this.IsBusy = true;
                Request.register l = Model;
                var t = await WebHelper.doPost<object, Request.register>(Config.Interface_register, l);
                this.IsBusy = false;

                if(t != null)
                {
                    thirdpage.IsChecked = true;
                    Count = 5;
                    run_time.Text = Count.ToString();
                    _timer.Start();
                }
            }

        }

        private async void btn_GetPhoneCode(object sender, RoutedEventArgs e)
        {
            this.IsBusy = true;
            Request.phoneCode l = new Request.phoneCode() { lec_id = App.CurrentLogin.lec_id, token = App.CurrentLogin.token, phone = Model.phone, type = Config.phoneCode.registerAccount };
            var t = await WebHelper.doPost<object, Request.phoneCode>(Config.Interface_phoneCode, l);
            this.IsBusy = false;
        }

        async void btn_UploadImage(object sender, RoutedEventArgs e)
        {
            switch((sender as Control).Tag.ObjToString())
            {
                case "1":
                    Model.body_photo = await UploadImage();
                    break;
                case "2":
                    Model.id_card_front = await UploadImage();
                    break;
                case "3":
                    Model.id_card_back = await UploadImage();
                    break;
                case "4":
                    Model.qualification_cert = await UploadImage();
                    break;

            }
            this.DataContext = null;
            this.DataContext = this;
        }

        private async Task<string> UploadImage()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Multiselect = false;
            dialog.Filter = "图片文件|*.png;*.bmp;*.jpg";
            if (dialog.ShowDialog() == true)
            {
                this.IsBusy = true;
                var path = dialog.FileName;
                var s = dialog.OpenFile();
                if(false)//(s.Length > 50*1024)
                {
                    MessageWindow.Alter("提示", "图片过大");
                    this.IsBusy = false;
                    return string.Empty;
                }
                byte[] bys = new byte[s.Length];
                s.Read(bys, 0, (int)s.Length);
                var str = Convert.ToBase64String(bys);
                Request.imageUpLoad l = new Request.imageUpLoad() { lec_id = App.CurrentLogin.lec_id, token = App.CurrentLogin.token, upload = str };
                var t = await WebHelper.doPost<Reponse.imageUpLoad, Request.imageUpLoad>(Config.Interface_imageUpLoad, l);

                this.IsBusy = false;
                return t.url;
            }
            return string.Empty;
        }
    }
}
