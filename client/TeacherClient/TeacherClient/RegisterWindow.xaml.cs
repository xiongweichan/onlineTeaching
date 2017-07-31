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
using Telerik.Windows.Controls;

namespace TeacherClient
{
    /// <summary>
    /// RegisterWindow.xaml 的交互逻辑
    /// </summary>
    public partial class RegisterWindow : WindowBase
    {
        RegisterModel Model { get; set; }
        public RegisterWindow()
        {
            InitializeComponent();
            Model = new RegisterModel();
            this.DataContext = this;
            this.IsBusy = false;
        }

        private void BtnSure_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Run_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }
    }

    public class RegisterModel : ViewModelBase
    {

        public RegisterModel()
        {
            ShowFirstPage = true;
        }

        bool _ShowFirstPage;
        /// <summary>
        /// 页面1
        /// </summary>
        public bool ShowFirstPage
        {
            get { return _ShowFirstPage; }
            set
            {
                if (value != _ShowFirstPage)
                {
                    _ShowFirstPage = value;
                    OnPropertyChanged("ShowFirstPage");
                }
            }
        }

        bool _ShowSecondPage;
        /// <summary>
        /// 页面2
        /// </summary>
        public bool ShowSecondPage
        {
            get { return _ShowSecondPage; }
            set
            {
                if (value != _ShowSecondPage)
                {
                    _ShowSecondPage = value;
                    OnPropertyChanged("ShowSecondPage");
                }
            }
        }

        bool _ShowThirdPage;
        /// <summary>
        /// 页面3
        /// </summary>
        public bool ShowThirdPage
        {
            get { return _ShowThirdPage; }
            set
            {
                if (value != _ShowThirdPage)
                {
                    _ShowThirdPage = value;
                    OnPropertyChanged("ShowThirdPage");
                }
            }
        }


    }
}
