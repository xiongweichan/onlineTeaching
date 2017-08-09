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
                Model = new Contract.Reponse.bankcardDetail();
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

        private void cb_city_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cb_city.SelectedItem is Reponse.region)
            {
                //cb_district.ItemsSource = (cb_city.SelectedItem as Reponse.region).data;
                //cb_district.SelectedIndex = 0;
            }
        }

        private void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            if (isnew)
            {

            }
            else
            {

            }

        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
