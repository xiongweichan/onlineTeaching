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
using Reponse = TeacherClient.Contract.Reponse;
using Request = TeacherClient.Contract.Request;

namespace TeacherClient.Pages
{
    /// <summary>
    /// CertificateInfo.xaml 的交互逻辑
    /// </summary>
    public partial class CertificateInfo : UserControl
    {
        public CertificateInfo()
        {
            InitializeComponent();
        }

        public void RefreshData()
        {
            Init();
        }
        Reponse.userInfo _copy;
        async void Init()
        {
            MainWindow.Current.IsBusy = true;
            Request.ParamBase l = new Request.ParamBase() { lec_id = App.CurrentLogin.lec_id, token = App.CurrentLogin.token };
            var t = await WebHelper.doPost<Reponse.userInfo, Request.ParamBase>(Config.Interface_userInfo, l);
            _copy = t;
            SetDataSource(t);
            this.DataContext = this;

            MainWindow.Current.IsBusy = false;
        }

        private void SetDataSource(Reponse.userInfo t)
        {
            this.id_card_back = t.id_card_back;
            this.id_card_front = t.id_card_front;
            this.body_photo = t.body_photo;
            this.qualification_cert = t.qualification_cert;
        }
        void UpdateDataSource(Reponse.userInfo t)
        {
            t.id_card_back = this.id_card_back;
            t.id_card_front = this.id_card_front;
            t.body_photo = this.body_photo;
            t.qualification_cert = this.qualification_cert;
        }

        public string id_card_front
        {
            get { return (string)GetValue(id_card_frontProperty); }
            set { SetValue(id_card_frontProperty, value); }
        }

        // Using a DependencyProperty as the backing store for id_card_front.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty id_card_frontProperty =
            DependencyProperty.Register("id_card_front", typeof(string), typeof(CertificateInfo), new PropertyMetadata());

        public string id_card_back
        {
            get { return (string)GetValue(id_card_backProperty); }
            set { SetValue(id_card_backProperty, value); }
        }

        // Using a DependencyProperty as the backing store for id_card_back.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty id_card_backProperty =
            DependencyProperty.Register("id_card_back", typeof(string), typeof(CertificateInfo), new PropertyMetadata());

        public string body_photo
        {
            get { return (string)GetValue(body_photoProperty); }
            set { SetValue(body_photoProperty, value); }
        }

        // Using a DependencyProperty as the backing store for body_photo.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty body_photoProperty =
            DependencyProperty.Register("body_photo", typeof(string), typeof(CertificateInfo), new PropertyMetadata());

        public string qualification_cert
        {
            get { return (string)GetValue(qualification_certProperty); }
            set { SetValue(qualification_certProperty, value); }
        }

        // Using a DependencyProperty as the backing store for qualification_cert.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty qualification_certProperty =
            DependencyProperty.Register("qualification_cert", typeof(string), typeof(CertificateInfo), new PropertyMetadata());

        async void btn_UploadImage1(object sender, RoutedEventArgs e)
        {
            var str = await UploadImageHelper.UploadImage();
            if (!string.IsNullOrEmpty(str))
                this.body_photo = str;
        }
        async void btn_UploadImage2(object sender, RoutedEventArgs e)
        {
            var str = await UploadImageHelper.UploadImage();
            if (!string.IsNullOrEmpty(str))
                this.id_card_front = str;
        }
        async void btn_UploadImage3(object sender, RoutedEventArgs e)
        {
            var str = await UploadImageHelper.UploadImage();
            if (!string.IsNullOrEmpty(str))
                this.id_card_back = str;
        }
        async void btn_UploadImage4(object sender, RoutedEventArgs e)
        {
            var str = await UploadImageHelper.UploadImage();
            if (!string.IsNullOrEmpty(str))
                this.qualification_cert = str;
        }

        async void btnOk_Click(object sender, RoutedEventArgs e)
        {
            Request.userPhotoSet l = new Contract.Request.userPhotoSet()
            {
                lec_id = App.CurrentLogin.lec_id,
                token = App.CurrentLogin.token,
                body_photo = this.body_photo,
                id_card_back = this.id_card_back,
                id_card_front = this.id_card_front,
                qualification_cert = this.qualification_cert
            };
            if(await WebHelper.doPost<Request.userPhotoSet>(Config.Interface_userPhotoSet, l))
            {
                UpdateDataSource(_copy);
                MessageWindow.Alter("提示", "保存成功");
            }
            else SetDataSource(_copy);

        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            SetDataSource(_copy);
        }
    }
}
