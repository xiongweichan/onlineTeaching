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
        public void ShowCourseManager(bool value, bool isNew = true, string id = null)
        {
            _ShowCourseManager = value;
            if (!value)
                _courseEdit = new CourseEdit(isNew, id);
            ShowContent();
        }
        bool _ShowCoursewareManager = true;
        public void ShowCoursewareManager(bool value, bool isNew = true, string id = null)
        {
            _ShowCoursewareManager = value;
            if (!value)
                _coursewareEdit = new CoursewareEdit(isNew, id);
            ShowContent();
        }

        CourseEdit _courseEdit = new CourseEdit(true);
        CourseManager _courseManager = new CourseManager();
        CoursewareManager _coursewareManager = new CoursewareManager();
        CoursewareEdit _coursewareEdit = new CoursewareEdit(true);
        MessageCenter _messageCenter = new MessageCenter();

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
                    if (_ShowCourseManager)
                        frame.Content = _courseManager;
                    else
                        frame.Content = _courseEdit;
                    break;
                case 1:
                    if (_ShowCoursewareManager)
                    {
                        frame.Content = _coursewareManager;
                        _coursewareManager.RefreshData();
                    }
                    else
                        frame.Content = _coursewareEdit;
                    break;
                case 2:
                    frame.Content = _messageCenter;
                    break;
            }
        }
    }
}
