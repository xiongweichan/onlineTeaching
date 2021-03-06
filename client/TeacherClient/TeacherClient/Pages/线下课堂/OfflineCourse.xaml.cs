﻿using System;
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

namespace TeacherClient.Pages
{
    /// <summary>
    /// OfflineCourse.xaml 的交互逻辑
    /// </summary>
    public partial class OfflineCourse : UserControl, INavigation
    {
        public static OfflineCourse Current { get; set; }
        public OfflineCourse()
        {
            Current = this;
            InitializeComponent();
            this.DataContext = this;
            Type = 0;
        }
        public void NavigateToPage(int index, object data)
        {
            Type = index;
            if (frame.Content is IChangeData && data != null)
                (frame.Content as IChangeData).ChangeData(data);
        }

        public int Type
        {
            get { return (int)GetValue(TypeProperty); }
            set { SetValue(TypeProperty, value); }
        }

        AllCourse _all = new AllCourse();
        NotStartCourse _notstart = new NotStartCourse();
        EndCourse _end = new EndCourse();

        // Using a DependencyProperty as the backing store for Type.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TypeProperty =
            DependencyProperty.Register("Type", typeof(int), typeof(OfflineCourse), new PropertyMetadata(-1, (obj, e) =>
            {
                var _this = obj as OfflineCourse;
                switch (_this.Type)
                {
                    case 0:
                        _this.frame.Content = _this._all;
                        break;
                    case 1:
                        _this.frame.Content = _this._notstart;
                        break;
                    case 2:
                        _this.frame.Content = _this._end;
                        break;
                }
            }));

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            AddOffCourseWindow win = new AddOffCourseWindow();
            win.ShowDialog();
        }
    }
}
