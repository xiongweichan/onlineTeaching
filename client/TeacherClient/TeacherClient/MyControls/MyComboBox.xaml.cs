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

namespace TeacherClient
{
    /// <summary>
    /// MyComboBox.xaml 的交互逻辑
    /// </summary>
    public partial class MyComboBox : UserControl
    {


        public string cat_id
        {
            get { return (string)GetValue(cat_idProperty); }
            set { SetValue(cat_idProperty, value); }
        }

        // Using a DependencyProperty as the backing store for cat_id.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty cat_idProperty =
            DependencyProperty.Register("cat_id", typeof(string), typeof(MyComboBox), new FrameworkPropertyMetadata(propertyChangedCallback) { BindsTwoWayByDefault = true });


        public string cat_id_1
        {
            get { return (string)GetValue(cat_id_1Property); }
            set { SetValue(cat_id_1Property, value); }
        }

        // Using a DependencyProperty as the backing store for cat_id_1.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty cat_id_1Property =
            DependencyProperty.Register("cat_id_1", typeof(string), typeof(MyComboBox), new FrameworkPropertyMetadata(propertyChangedCallback) { BindsTwoWayByDefault = true });



        public string cat_id_2
        {
            get { return (string)GetValue(cat_id_2Property); }
            set { SetValue(cat_id_2Property, value); }
        }

        // Using a DependencyProperty as the backing store for cat_id_2.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty cat_id_2Property =
            DependencyProperty.Register("cat_id_2", typeof(string), typeof(MyComboBox), new FrameworkPropertyMetadata(propertyChangedCallback) { BindsTwoWayByDefault = true });



        private static void propertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as MyComboBox).SetDataSource();
        }
        

        public MyComboBox()
        {
            InitializeComponent();
            //cat_id = "1"; cat_id_1 = "2"; cat_id_2 = "3";

        }


        private void Grid_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            popup.IsOpen = !popup.IsOpen;
            SetDataSource();
        }

        private void SetDataSource()
        {
            var list = SystemInit.Instance.CategoryList;
            ic_first.ItemsSource = list;
            if (!string.IsNullOrEmpty(cat_id))
                ic_first.SelectedItem = list.FirstOrDefault(T => T.id == cat_id);
        }

        private void ic_first_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ic_first.SelectedItem == null) return;
            var list = (ic_first.SelectedItem as Reponse.category).data;
            ic_second.ItemsSource = list;
            if (!string.IsNullOrEmpty(cat_id_1))
                ic_second.SelectedItem = list.FirstOrDefault(T => T.id == cat_id_1);
        }

        private void ic_second_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ic_second.SelectedItem == null) return;
            var list = (ic_second.SelectedItem as Reponse.category).data;
            ic_third.ItemsSource = list;
            if (!string.IsNullOrEmpty(cat_id_2))
                ic_third.SelectedItem = list.FirstOrDefault(T => T.id == cat_id_2);
        }

        private void ic_third_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            tb_Text.Text = string.Format("{0} / {1} / {2}"
                , (ic_first.SelectedItem as Reponse.category).name
                , (ic_second.SelectedItem as Reponse.category).name
                , (ic_third.SelectedItem as Reponse.category).name);
            this.cat_id = (ic_first.SelectedItem as Reponse.category).id;
            this.cat_id_1 = (ic_second.SelectedItem as Reponse.category).id;
            this.cat_id_2 = (ic_third.SelectedItem as Reponse.category).id;
        }
    }
}
