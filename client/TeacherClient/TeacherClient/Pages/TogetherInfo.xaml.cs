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
using Telerik.Windows.Controls;
using TeacherClient.Core;
using Reponse = TeacherClient.Contract.Reponse;
using Request = TeacherClient.Contract.Request;

namespace TeacherClient.Pages
{
    /// <summary>
    /// TogetherInfo.xaml 的交互逻辑
    /// </summary>
    public partial class TogetherInfo : UserControl
    {
        public TogetherInfo()
        {
            InitializeComponent();
            Init();

        }

        async void Init()
        {
            (App.Current.MainWindow as WindowBase).IsBusy = true;
            Request.ParamBase l = new Request.ParamBase() { lec_id = App.CurrentLogin.lec_id, token = App.CurrentLogin.token };
            var t = await IPCHandle.doPost<Reponse.ResponseParam<Reponse.MainIndex>>(Config.Interface_mainIndex, l.ReturnRequestParam());

            this.DataContext = t.data;
            (App.Current.MainWindow as WindowBase).IsBusy = false;
        }
    }
}
