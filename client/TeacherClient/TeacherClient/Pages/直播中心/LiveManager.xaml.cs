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
    /// LiveManager.xaml 的交互逻辑
    /// </summary>
    public partial class LiveManager : UserControl
    {
        public LiveManager()
        {
            InitializeComponent();
            Type = 1;
            this.DataContext = this;
        }
        public int Type
        {
            get { return (int)GetValue(TypeProperty); }
            set { SetValue(TypeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Type.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TypeProperty =
            DependencyProperty.Register("Type", typeof(int), typeof(LiveManager), new PropertyMetadata(-1, (obj, e) =>
            {
                var _this = obj as LiveManager;
                _this.ShowContent();
            }));

        private void ShowContent()
        {
            //0:全部,1:待直播,2:审核成功,3:待审核,4:已结束
            GetData();
        }

        private void pagerData_PagerIndexChanged(object sender, EventArgs e)
        {
            GetData();
        }
        async void GetData()
        {
            MainWindow.Current.IsBusy = true;
            Request.requestlivelist l = new Request.requestlivelist() { lec_id = App.CurrentLogin.lec_id, token = App.CurrentLogin.token, page = pagerData.PageIndex, pageSize = pagerData.PageSize, list_type = Type.ToString() };

            var t = await WebHelper.doPost<Reponse.listData<Reponse.live>, Request.requestlivelist>(Config.Interface_liveList, l);
            if (t != null)
            {
                pagerData.TotalCount = t.totalCount;
                datagrid.ItemsSource = t.list;
            }

            MainWindow.Current.IsBusy = false;

        }

        private void EditRequest_Click(object sender, RoutedEventArgs e)
        {

        }

        private void StartLive_Click(object sender, RoutedEventArgs e)
        {
            LiveCenter.Current.ShowMylive = true;
        }

        private void EditTime_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DelRecord_Click(object sender, RoutedEventArgs e)
        {
            if (MessageWindow.Alter("删除确认", "您确定要删除记录吗？") == true)
            {

            }
        }

        private void CancelRequest_Click(object sender, RoutedEventArgs e)
        {
            if (MessageWindow.Alter("确认取消", "确认要取消直播吗？取消后不可恢复") == true)
            {

            }
        }
    }    
}
