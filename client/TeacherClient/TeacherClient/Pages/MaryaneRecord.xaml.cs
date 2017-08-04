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
    /// MaryaneRecord.xaml 的交互逻辑
    /// </summary>
    public partial class MaryaneRecord : UserControl
    {
        public MaryaneRecord()
        {
            InitializeComponent();
            Init();
        }

        private void Init()
        {
            MainWindow.Current.IsBusy = true;

            LiveRewardListProvider provider = new LiveRewardListProvider();
            listview.ItemsSource = new AsyncVirtualizingCollection<Reponse.liveRewardList>(provider, 20, 10 * 1000);
            MainWindow.Current.IsBusy = false;
        }
    }
}
