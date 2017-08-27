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
    /// EditLiveTime.xaml 的交互逻辑
    /// </summary>
    public partial class EditLiveTime : WindowBase
    {
        string _id;
        public EditLiveTime(string id)
        {
            _id = id;
            InitializeComponent();
            this.IsBusy = false;
        }

        async void btnOk_Click(object sender, RoutedEventArgs e)
        {
            Request.liveChangeTime l = new Request.liveChangeTime() { lec_id = App.CurrentLogin.lec_id, token = App.CurrentLogin.token, id = _id, start_time= liveStartTime.SelectedValue.ConvertDateTimeInt().ToString(), end_time = liveEndTime.SelectedValue.ConvertDateTimeInt().ToString() };

            this.DialogResult = await WebHelper.doPost<Request.liveChangeTime>(Config.Interface_liveChangeTime, l);
            if (this.DialogResult == true)
                this.Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
