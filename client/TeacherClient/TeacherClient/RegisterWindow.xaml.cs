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


        public RegisterModel Model
        {
            get { return (RegisterModel)GetValue(ModelProperty); }
            set { SetValue(ModelProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Model.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ModelProperty =
            DependencyProperty.Register("Model", typeof(RegisterModel), typeof(RegisterWindow), new PropertyMetadata());


        //public Request.register Model { get; set; }
        public RegisterWindow()
        {
            InitializeComponent();
            Model = new RegisterModel();
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
                if(string.IsNullOrWhiteSpace(Model.phone))
                    MessageWindow.Alter("提示", "手机号码不能为空");
                else if (string.IsNullOrWhiteSpace(Model.code))
                    MessageWindow.Alter("提示", "验证码不能为空");
                else if (string.IsNullOrWhiteSpace(Model.email))
                    MessageWindow.Alter("提示", "邮箱不能为空");
                else if (string.IsNullOrWhiteSpace(Model.password))
                    MessageWindow.Alter("提示", "密码不能为空");
                else if (Model.password != pwd_repeat.Pwd)
                    MessageWindow.Alter("提示", "两次密码输入不一致");
                else
                    secondpage.IsChecked = true;
            }
            else if (secondpage.IsChecked.HasValue && secondpage.IsChecked.Value)
            {
                this.IsBusy = true;
                Request.register l = new Contract.Request.register()
                {
                    body_photo = Model.body_photo,
                    code = Model.code,
                    email = Model.email,
                    id_card_back = Model.id_card_back,
                    id_card_front = Model.id_card_front,
                    lec_id = App.CurrentLogin.lec_id,
                    token = App.CurrentLogin.token,
                    password = Model.password,
                    phone = Model.phone,
                    qualification_cert = Model.qualification_cert
                };
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
            this.IsBusy = true;

            switch ((sender as Control).Tag.ObjToString())
            {
                case "1":
                    Model.body_photo = await UploadImageHelper.UploadImage();
                    break;
                case "2":
                    Model.id_card_front = await UploadImageHelper.UploadImage();
                    break;
                case "3":
                    Model.id_card_back = await UploadImageHelper.UploadImage();
                    break;
                case "4":
                    Model.qualification_cert = await UploadImageHelper.UploadImage();
                    break;

            }
            this.IsBusy = false;

        }

        //private async Task<string> UploadImage()
        //{
        //    OpenFileDialog dialog = new OpenFileDialog();
        //    dialog.Multiselect = false;
        //    dialog.Filter = "图片文件|*.png;*.bmp;*.jpg";
        //    if (dialog.ShowDialog() == true)
        //    {
        //        this.IsBusy = true;
        //        var path = dialog.FileName;
        //        var s = dialog.OpenFile();
        //        if(s.Length > 50*1024)
        //        {
        //            MessageWindow.Alter("提示", "图片过大");
        //            this.IsBusy = false;
        //            return string.Empty;
        //        }
        //        byte[] bys = new byte[s.Length];
        //        s.Read(bys, 0, (int)s.Length);
        //        var str = Convert.ToBase64String(bys);
        //        Request.imageUpLoad l = new Request.imageUpLoad() { lec_id = App.CurrentLogin.lec_id, token = App.CurrentLogin.token, upload = str };
        //        var t = await WebHelper.doPost<Reponse.imageUpLoad, Request.imageUpLoad>(Config.Interface_imageUpLoad, l);

        //        this.IsBusy = false;
        //        return t.url;
        //    }
        //    return string.Empty;
        //}
    }


    public class RegisterModel : ViewModelBase
    {
        string _phone;
        public string phone
        {
            get { return _phone; }
            set
            {
                _phone = value;
                this.OnPropertyChanged("phone");
            }
        }
        string _password;
        public string password
        {
            get { return _password; }
            set
            {
                _password = value;
                this.OnPropertyChanged("password");
            }
        }
        string _code;
        public string code
        {
            get { return _code; }
            set
            {
                _code = value;
                this.OnPropertyChanged("code");
            }
        }
        string _email;
        public string email
        {
            get { return _email; }
            set
            {
                _email = value;
                this.OnPropertyChanged("email");
            }
        }
        string _id_card_front;
        public string id_card_front
        {
            get { return _id_card_front; }
            set
            {
                _id_card_front = value;
                this.OnPropertyChanged("id_card_front");
            }
        }
        string _id_card_back;
        public string id_card_back
        {
            get { return _id_card_back; }
            set
            {
                _id_card_back = value;
                this.OnPropertyChanged("id_card_back");
            }
        }
        string _body_photo;
        public string body_photo
        {
            get { return _body_photo; }
            set
            {
                _body_photo = value;
                this.OnPropertyChanged("body_photo");
            }
        }
        string _qualification_cert;
        public string qualification_cert
        {
            get { return _qualification_cert; }
            set
            {
                _qualification_cert = value;
                this.OnPropertyChanged("qualification_cert");
            }
        }
        //public string phone { get; set; }
        //public string password { get; set; }
        //public string code { get; set; }
        //public string email { get; set; }
        //public string id_card_front { get; set; }
        //public string id_card_back { get; set; }
        //public string body_photo { get; set; }
        //public string qualification_cert { get; set; }
    }
}
