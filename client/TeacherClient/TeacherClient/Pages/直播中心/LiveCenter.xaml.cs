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
    /// LiveCenter.xaml 的交互逻辑
    /// </summary>
    public partial class LiveCenter : UserControl, INavigation
    {
        public static LiveCenter Current { get; set; }
        public LiveCenter()
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

        bool _ShowMylive;
        public void ShowMylive(bool value, string id = null)
        {
            _ShowMylive = value;
            if (value)
            {
                _myLive = new MyLive(id);
            }
            ShowContent();
        }

        public void ModifyRequestLive(string id)
        {
            _requestLive = new RequestLive();
            _requestLive.IsNew = false;
            _requestLive.ID = id;
            if (Type != 1)
                Type = 1;
            else
                ShowContent();
        }

        MyLive _myLive;
        RequestLive _requestLive = new RequestLive();
        LiveManager _liveManager = new LiveManager();

        public void ClearRequestLive()
        {
            _requestLive = new RequestLive();
        }

        // Using a DependencyProperty as the backing store for Type.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TypeProperty =
            DependencyProperty.Register("Type", typeof(int), typeof(LiveCenter), new PropertyMetadata(-1, (obj, e) =>
            {
                var _this = obj as LiveCenter;
                _this.ShowContent();
            }));
        private void ShowContent()
        {
            switch (Type)
            {
                case 0:
                    if (!_ShowMylive)
                        frame.Content = _liveManager;
                    else
                    {
                        frame.Content = _myLive;
                        _myLive.RefreshData();
                    }
                    break;
                case 1:
                    frame.Content = _requestLive;
                    _requestLive.firstpage.IsChecked = true;
                    break;
            }
        }
    }
}
