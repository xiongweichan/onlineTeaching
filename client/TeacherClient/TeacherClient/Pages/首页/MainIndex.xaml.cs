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
    /// MainIndex.xaml 的交互逻辑
    /// </summary>
    public partial class MainIndex : UserControl, INavigation
    {
        public static MainIndex Current { get; set; }
        public MainIndex()
        {
            Current = this;
            InitializeComponent();
            this.DataContext = this;
            Type = 0;
        }
        TogetherInfo _togetherInfo = new TogetherInfo();
        TeacherInfo _teacherInfo = new TeacherInfo();
        CertificateInfo _certificateInfo = new CertificateInfo();
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

        // Using a DependencyProperty as the backing store for Type.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TypeProperty =
            DependencyProperty.Register("Type", typeof(int), typeof(MainIndex), new PropertyMetadata(-1, (obj, e) =>
            {
                var _this = obj as MainIndex;
                switch (_this.Type)
                {
                    case 0:
                        _this.frame.Content = _this._togetherInfo;
                        _this._togetherInfo.RefreshData();
                        break;
                    case 1:
                        _this.frame.Content = _this._teacherInfo;
                        _this._teacherInfo.RefreshData();
                        break;
                    case 2:
                        _this.frame.Content = _this._certificateInfo;
                        _this._certificateInfo.RefreshData();
                        break;
                }
            }));


    }
}
