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
    /// MessageCenter.xaml 的交互逻辑
    /// </summary>
    public partial class MessageCenter : UserControl
    {


        string Type
        {
            get
            {
                if (system.IsChecked.HasValue && system.IsChecked.Value)
                    return "0";
                //if (student.IsChecked.HasValue && student.IsChecked.Value)
                //    return "1";
                if (studentleave.IsChecked.HasValue && studentleave.IsChecked.Value)
                    return "2";
                return null;
            }
        }
        public MessageCenter()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        private void pagerData_PagerIndexChanged(object sender, EventArgs e)
        {
            GetData();
        }
        async void GetData()
        {
            MainWindow.Current.IsBusy = true;
            Request.requestType l = new Request.requestType() { lec_id = App.CurrentLogin.lec_id, token = App.CurrentLogin.token, page = pagerData.PageIndex, pageSize = pagerData.PageSize, type = this.Type};

            var t = await WebHelper.doPost<Reponse.listData<Reponse.message>, Request.requestType>(Config.Interface_messageList, l);
            if (t != null)
            {
                pagerData.TotalCount = t.totalCount;
                datagrid.ItemsSource = t.list;
            }

            MainWindow.Current.IsBusy = false;

        }

        private void StackPanel_Checked(object sender, RoutedEventArgs e)
        {
            if (pagerData == null) return;
            pagerData.PageIndex = 0;
            GetData();
        }

        private void datagrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var datagrid = sender as DataGrid;
            if (datagrid == null) return;
            var item = datagrid.SelectedItem as Reponse.message;
            new DetailMessageWindow() { Title = item.title, Summary = item.summary, Message = item.content }.ShowDialog();
        }

        private void DeleteCourse_Click(object sender, RoutedEventArgs e)
        {
            MessageWindow.Alter("提示", "服务端接口没有！");
        }
    }
}
