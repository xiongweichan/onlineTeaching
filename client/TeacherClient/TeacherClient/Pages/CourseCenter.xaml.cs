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

namespace TeacherClient.Pages
{
    /// <summary>
    /// CourseCenter.xaml 的交互逻辑
    /// </summary>
    public partial class CourseCenter : UserControl, INavigation
    {
        public static CourseCenter Current { get; set; }
        public CourseCenter()
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

        //MyLive _myLive = new MyLive();
        //RequestLive _requestLive = new RequestLive();
        //LiveManager _liveManager = new LiveManager();
        bool _ShowCourseManager = true;
        public bool ShowCourseManager
        {
            get { return _ShowCourseManager; }
            set
            {
                _ShowCourseManager = value;
                if (!value)
                    _courseEdit = new CourseEdit();
                ShowContent();
            }
        }
        CourseEdit _courseEdit = new CourseEdit();
        CourseManager _courseManager = new CourseManager();
        CoursewareManager _coursewareManager = new CoursewareManager();

        // Using a DependencyProperty as the backing store for Type.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TypeProperty =
            DependencyProperty.Register("Type", typeof(int), typeof(CourseCenter), new PropertyMetadata(-1, (obj, e) =>
            {
                var _this = obj as CourseCenter;
                _this.ShowContent();
            }));

        private void ShowContent()
        {
            switch (Type)
            {
                case 0:
                    if (ShowCourseManager)
                        frame.Content = _courseManager;
                    else
                        frame.Content = _courseEdit;
                    break;
                case 1:
                    frame.Content = _coursewareManager;
                    break;
                case 2:
                    //_this.frame.Content = _this._requestLive;
                    break;
            }
        }
    }
}
