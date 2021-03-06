﻿using System;
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
    /// GiftReward.xaml 的交互逻辑
    /// </summary>
    public partial class GiftReward : UserControl
    {
        public GiftReward()
        {
            InitializeComponent();
            Init();
        }
        async void Init()
        {
            MainWindow.Current.IsBusy = true;
            Request.ParamBase l = new Request.ParamBase() { lec_id = App.CurrentLogin.lec_id, token = App.CurrentLogin.token };
            var t = await WebHelper.doPost<Reponse.liveGiftIndex, Request.ParamBase>(Config.Interface_liveGiftIndex, l);
            grid_total.DataContext = t;
            MainWindow.Current.IsBusy = false;
        }

        private void pagerData_PagerIndexChanged(object sender, EventArgs e)
        {
            GetData();
        }
        async void GetData()
        {
            MainWindow.Current.IsBusy = true;
            Request.PageParam l = new Request.PageParam() { lec_id = App.CurrentLogin.lec_id, token = App.CurrentLogin.token, page = pagerData.PageIndex, pageSize = pagerData.PageSize };

            var t = await WebHelper.doPost<Reponse.listData<Reponse.liveGiftList>, Request.PageParam>(Config.Interface_liveGiftList, l);
            if (t != null)
            {
                pagerData.TotalCount = t.totalCount;
                datagrid.ItemsSource = t.list;
            }

            MainWindow.Current.IsBusy = false;

        }

        private void datagrid_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            GiftDetailWindow win = new GiftDetailWindow();
            win.DataContext = (datagrid.SelectedItem as Reponse.liveGiftList).gift_list;

            win.ShowDialog();
        }
    }
}
