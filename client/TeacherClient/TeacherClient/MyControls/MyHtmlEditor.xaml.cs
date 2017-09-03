using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
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
using TeacherClient.Core;
using Telerik.Windows.Documents.FormatProviders.Html;

namespace TeacherClient
{

    /// <summary>
    /// MyHtmlEditor.xaml 的交互逻辑
    /// </summary>
    public partial class MyHtmlEditor : UserControl
    {



        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Text.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(MyHtmlEditor), new FrameworkPropertyMetadata() { BindsTwoWayByDefault = true });



        public MyHtmlEditor()
        {
            InitializeComponent();
            //SuppressScriptErrors(this.webBrowser, true);
        }

        private void MyHtmlEditor_Loaded(object sender, RoutedEventArgs e)
        {
            //this.webBrowser.Source = new Uri(AppDomain.CurrentDomain.BaseDirectory + "ueditor1_4_3_3-utf8-net\\index.html");
            //this.webBrowser.Navigate(new Uri(AppDomain.CurrentDomain.BaseDirectory + "ueditor1_4_3_3-utf8-net\\index.html").AbsoluteUri);
        }
        //public string GetHTMLContent()
        //{
        //    try
        //    {
        //        return xamlDataProvider.Html;//this.webBrowser.InvokeScript("getContent").ObjToString();

        //    }
        //    catch(COMException ex)
        //    {
        //        MessageWindow.Alter("提示", "请在IE--->工具--->Internet选项--->高级--->\"允许活动内容在我的计算机上的文件中运行\" 勾选");
        //        Log.Error("获取html报错", ex);
        //        return string.Empty;
        //    }
        //}


        //static void SuppressScriptErrors(WebBrowser webBrowser, bool hide)
        //{
        //    webBrowser.Navigating += (s, e) =>
        //    {
        //        var fiComWebBrowser = typeof(WebBrowser).GetField("_axIWebBrowser2", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
        //        if (fiComWebBrowser == null)
        //            return;

        //        object objComWebBrowser = fiComWebBrowser.GetValue(webBrowser);
        //        if (objComWebBrowser == null)
        //            return;

        //        objComWebBrowser.GetType().InvokeMember("Silent", System.Reflection.BindingFlags.SetProperty, null, objComWebBrowser, new object[] { hide });
        //    };
        //}
    }
}
