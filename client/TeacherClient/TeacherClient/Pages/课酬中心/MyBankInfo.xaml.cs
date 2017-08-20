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
using System.Windows.Shapes;
using Reponse = TeacherClient.Contract.Reponse;
using Request = TeacherClient.Contract.Request;

namespace TeacherClient.Pages
{
    /// <summary>
    /// MyBankInfo.xaml 的交互逻辑
    /// </summary>
    public partial class MyBankInfo : WindowBase
    {
        public MyBankInfo()
        {
            InitializeComponent();
            Init();
        }
        public Reponse.bankcardDetail Model { get; set; }
        bool isnew = false;
        async void Init()
        {
            Request.ParamBase l = new Request.ParamBase() { lec_id = App.CurrentLogin.lec_id, token = App.CurrentLogin.token };
            var t = await WebHelper.doPost<Reponse.bankcardDetail, Request.ParamBase>(Config.Interface_bankcardDetail, l);
            if (t == null)
            {
                isnew = true;
                Model = new Contract.Reponse.bankcardDetail()
                {
                    mobile = App.CurrentLogin.user.phone,
                    real_name = App.CurrentLogin.user.realname,

                };
            }
            else
                Model = t;
            cb_banktype.ItemsSource = SystemInit.Instance.BankInfos;
            cb_province.ItemsSource = SystemInit.Instance.Regions;

            this.IsBusy = false;
            this.DataContext = this;
        }

        private void cb_province_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cb_province.SelectedItem is Reponse.region)
            {
                cb_city.ItemsSource = (cb_province.SelectedItem as Reponse.region).data;
                //cb_city.SelectedIndex = 0;
            }
        }

        async void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            if (isnew)
            {
                Request.bankcardAdd l = new Contract.Request.bankcardAdd()
                {
                    bank_card = Model.bank_card,
                    bank_site = Model.bank_site,
                    bank_type = Model.bank_type,
                    city = Model.city,
                    code = tb_Code.Text,
                    id_card = Model.id_card,
                    lec_id = App.CurrentLogin.lec_id,
                    mobile = Model.mobile,
                    province = Model.province,
                    real_name = Model.real_name,
                    token = App.CurrentLogin.token
                };
                if (await WebHelper.doPost<Request.bankcardAdd>(Config.Interface_bankcardAdd, l))
                {
                    MessageWindow.Alter("提示", "添加成功！");
                    this.Close();
                }
            }
            else
            {
                Request.bankcardEdit l = new Request.bankcardEdit()
                {
                    bank_card = Model.bank_card,
                    bank_site = Model.bank_site,
                    bank_type = Model.bank_type,
                    city = Model.city,
                    code = tb_Code.Text,
                    id_card = Model.id_card,
                    lec_id = App.CurrentLogin.lec_id,
                    mobile = Model.mobile,
                    province = Model.province,
                    real_name = Model.real_name,
                    token = App.CurrentLogin.token,
                    id = Model.id,
                };
                if (await WebHelper.doPost<Request.bankcardEdit>(Config.Interface_bankcardEdit, l))
                {
                    MessageWindow.Alter("提示", "修改成功！");
                    this.Close();
                }
            }

        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        async void btnGetCode_Click(object sender, RoutedEventArgs e)
        {
            Request.phoneCode l = new Contract.Request.phoneCode() { lec_id = App.CurrentLogin.lec_id, token = App.CurrentLogin.token, phone = Model.mobile, type = Config.phoneCode.registerBank };
            await WebHelper.doPost<Request.phoneCode>(Config.Interface_phoneCode, l);
        }
    }
}
