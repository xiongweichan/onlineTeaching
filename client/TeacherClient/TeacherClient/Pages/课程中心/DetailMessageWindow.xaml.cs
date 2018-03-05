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

namespace TeacherClient.Pages
{
    /// <summary>
    /// DetailMessageWindow.xaml 的交互逻辑
    /// </summary>
    public partial class DetailMessageWindow : WindowBase
    {
        public string Summary
        {
            get { return (string)GetValue(SummaryProperty); }
            set { SetValue(SummaryProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Message.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SummaryProperty =
            DependencyProperty.Register("Summary", typeof(string), typeof(DetailMessageWindow), new PropertyMetadata());

        public string Message
        {
            get { return (string)GetValue(MessageProperty); }
            set { SetValue(MessageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Message.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MessageProperty =
            DependencyProperty.Register("Message", typeof(string), typeof(DetailMessageWindow), new PropertyMetadata());



        public DetailMessageWindow()
        {
            InitializeComponent();
            this.IsBusy = false;
            this.DataContext = this;
        }

        private void BtnOK_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
