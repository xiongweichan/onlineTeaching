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
    /// MyTextBox.xaml 的交互逻辑
    /// </summary>
    public partial class MyTextBox : UserControl
    {


        public bool IsEdit
        {
            get { return (bool)GetValue(IsEditProperty); }
            set { SetValue(IsEditProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsEdit.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsEditProperty =
            DependencyProperty.Register("IsEdit", typeof(bool), typeof(MyTextBox), new PropertyMetadata(false));



        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Text.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(MyTextBox), new FrameworkPropertyMetadata(string.Empty) { BindsTwoWayByDefault = true });

        public static readonly RoutedEvent TextChangedEvent = EventManager.RegisterRoutedEvent(
        "TextChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(MyTextBox));

        // Provide CLR accessors for the event
        public event RoutedEventHandler TextChanged
        {
            add { AddHandler(TextChangedEvent, value); }
            remove { RemoveHandler(TextChangedEvent, value); }
        }

        public MyTextBox()
        {
            InitializeComponent();
        }

        private void Grid_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!this.IsEdit && e.ClickCount > 1)
                this.IsEdit = true;
        }

        private void btnOKClick(object sender, RoutedEventArgs e)
        {
            RoutedEventArgs newEventArgs = new RoutedEventArgs(MyTextBox.TextChangedEvent, this);
            RaiseEvent(newEventArgs);
            this.IsEdit = false;
        }

        private void btnCancelClick(object sender, RoutedEventArgs e)
        {
            this.IsEdit = false;
        }
    }
}
