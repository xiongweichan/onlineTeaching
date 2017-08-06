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
    /// WithDrawDescription.xaml 的交互逻辑
    /// </summary>
    public partial class WithDrawDescription : WindowBase
    {
        public WithDrawDescription()
        {
            InitializeComponent();
            Init();
        }

        async void Init()
        {
            this.IsBusy = true;
            Request.ParamBase l = new Request.ParamBase() { lec_id = App.CurrentLogin.lec_id, token = App.CurrentLogin.token };
            var t = await WebHelper.doPost<Reponse.withdrawIndex, Request.ParamBase>(Config.Interface_withdrawIndex, l);
            this.DataContext = t;
            this.IsBusy = false;

        }
    }
}
