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

namespace TeacherClient
{
    /// <summary>
    /// MyPassword.xaml 的交互逻辑
    /// </summary>
    public partial class MyPassword : UserControl
    {
        public MyPassword()
        {
            InitializeComponent();
            this.DataContext = this;
        }


        public string Password
        {
            get { return (string)GetValue(PasswordProperty); }
            set { SetValue(PasswordProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Password.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PasswordProperty =
            DependencyProperty.Register("Password", typeof(string), typeof(MyPassword), new FrameworkPropertyMetadata(string.Empty, PropertyChangedCallback) { BindsTwoWayByDefault = true });

        static void PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var _this = d as MyPassword;
            if (_this.passwordBox.Password != e.NewValue.ToString())
                _this.passwordBox.Password = e.NewValue.ToString();
        }

        public string Watermark
        {
            get { return (string)GetValue(WatermarkProperty); }
            set { SetValue(WatermarkProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Watermark.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty WatermarkProperty =
            DependencyProperty.Register("Watermark", typeof(string), typeof(MyPassword), new PropertyMetadata(string.Empty));

        private void passwordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            SetValue(MyPassword.PasswordProperty, passwordBox.Password);            
        }
    }
}
