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


        public string moneyWithdraw
        {
            get { return (string)GetValue(moneyWithdrawProperty); }
            set { SetValue(moneyWithdrawProperty, value); }
        }

        // Using a DependencyProperty as the backing store for moneyWithdraw.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty moneyWithdrawProperty =
            DependencyProperty.Register("moneyWithdraw", typeof(string), typeof(WithDrawAdd), new PropertyMetadata());



        public WithDrawAdd()
        {
            InitializeComponent();
        }

        public void RefreshData()
        {
            Init();
        }
        async void Init()
        {
            MainWindow.Current.IsBusy = true;
            Request.ParamBase l = new Request.ParamBase() { lec_id = App.CurrentLogin.lec_id, token = App.CurrentLogin.token };
            var t = await WebHelper.doPost<Reponse.MainIndex, Request.ParamBase>(Config.Interface_mainIndex, l);

            moneyWithdraw = t.moneyWithdraw;

            this.DataContext = this;
            MainWindow.Current.IsBusy = false;
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



        public double WithMoney
        {
            get { return (double)GetValue(WithMoneyProperty); }
            set { SetValue(WithMoneyProperty, value); }
        }

        // Using a DependencyProperty as the backing store for WithMoney.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty WithMoneyProperty =
            DependencyProperty.Register("WithMoney", typeof(double), typeof(WithDrawAdd), new PropertyMetadata(0d, PropertyChangedCallback));

        static void PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as WithDrawAdd).GetServiceMoney();
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
                MessageWindow.Alter("提示", "提现申请提交成功，等待审核中");            
            else
                MessageWindow.Alter("错误", "服务器连接失败！");
            MainWindow.Current.IsBusy = false;
        }

        async void GetServiceMoney()
        {
            MainWindow.Current.IsBusy = true;
            Request.withdrawCharge request = new Contract.Request.withdrawCharge()
            {
                lec_id = App.CurrentLogin.lec_id,
                token = App.CurrentLogin.token,
                money = this.WithMoney.ToString(),
            };
            var t = await WebHelper.doPost<Reponse.withdrawCharge, Request.withdrawCharge> (Config.Interface_withdrawCharge, request);
            if(t != null)
            {
                tbl_ServiceMoney.Text = t.charge;
                tbl_ToMoney.Text = (this.WithMoney - double.Parse(t.charge)).ToString();
            }

            MainWindow.Current.IsBusy = false;
        }
    }
}
