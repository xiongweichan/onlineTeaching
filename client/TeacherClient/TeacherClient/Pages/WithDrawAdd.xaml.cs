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
using TeacherClient.Core;
using Reponse = TeacherClient.Contract.Reponse;
using Request = TeacherClient.Contract.Request;

namespace TeacherClient.Pages
{
    /// <summary>
    /// WithDrawAdd.xaml 的交互逻辑
    /// </summary>
    public partial class WithDrawAdd : UserControl
    {
        public WithDrawAdd()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //银行信息
            MyBankInfo win = new MyBankInfo();
            win.ShowDialog();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //平台服务说明
            WithDrawDescription win = new WithDrawDescription();
            win.ShowDialog();
        }



        public int WithMoney
        {
            get { return (int)GetValue(WithMoneyProperty); }
            set { SetValue(WithMoneyProperty, value); }
        }

        // Using a DependencyProperty as the backing store for WithMoney.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty WithMoneyProperty =
            DependencyProperty.Register("WithMoney", typeof(int), typeof(WithDrawAdd), new PropertyMetadata(0, PropertyChangedCallback));

        static void PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var _this = d as WithDrawAdd;
            if (_this != null && e.Property == WithDrawAdd.WithMoneyProperty)
            {
                var c = _this.WithMoney * 0.01;
                _this.tbl_ServiceMoney.Text = c.ToString("f2");
                _this.tbl_ToMoney.Text = (_this.WithMoney - c).ToString("f2");
            }
        }
        public string Description { get; set; }

        private async void Button_Click_2(object sender, RoutedEventArgs e)
        {
            MainWindow.Current.IsBusy = true;
            Request.withdrawAdd request = new Contract.Request.withdrawAdd()
            {
                lec_id = App.CurrentLogin.lec_id,
                token = App.CurrentLogin.token,
                money = this.WithMoney.ToString(),
                remark = this.Description,
            };
            var t = await IPCHandle.doPost<Reponse.ResponseParam<string>>(Config.Interface_withdrawAdd, request.ReturnRequestParam());
            if (t != null)            
                MessageWindow.Alter("提示", t.info);            
            else
                MessageWindow.Alter("错误", "服务器连接失败！");
            MainWindow.Current.IsBusy = false;
        }
    }
}
