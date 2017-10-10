using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using Reponse = TeacherClient.Contract.Reponse;
using TeacherClient.Core;
using Telerik.Windows.Controls;
using System.Windows.Media;

namespace TeacherClient
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            LocalizationManager.Manager = new MyLocalizationManager();
            StyleManager.ApplicationTheme = new Windows8Theme();

            ConfigManagerHelper.Init(Config.SystemConfigPath, true);
            SetAutoStartup();
            IPCHandle.Init();
            var d = TimerHelper.Instance;
            App.Current.DispatcherUnhandledException += Current_DispatcherUnhandledException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;

        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            try
            {
                Exception error = e.ExceptionObject as Exception;
                if (error != null)
                {
                    Log.Error("未捕获的异常", error);
                }
                else
                {
                    Log.Error(string.Format("未捕获的异常:{0}", e));
                }

                Log.Error("未捕获的异常" + e.ExceptionObject.ToJson());
                MessageWindow.Alter("错误", "程序出现未知异常，请联系管理员处理！");
            }
            catch (Exception) { }
        }

        private void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            try
            {
                Log.Error("异步操作异常", e.Exception);
            }
            catch (Exception) { }
        }

        private void Current_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            try
            {
                Log.Error("程序崩溃", e.Exception);
                MessageWindow.Alter("错误", "程序出现未知异常，请联系管理员处理！");
            }
            catch (Exception) { }
        }

        /// <summary>
        /// 设置开机启动
        /// </summary>
        private static void SetAutoStartup()
        {
            try
            {
                if (App.IsAutoStartup)
                {
                    string path = App.Current.GetType().Assembly.Location;
                    RegistryKey rk = Registry.LocalMachine;
                    RegistryKey rk2 = rk.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run");
                    if (rk2.GetValue("LJSchool").ObjToString() != path)
                        rk2.SetValue("LJSchool", path);
                    rk2.Close();
                    rk.Close();
                }
                else
                {
                    string path = App.Current.GetType().Assembly.Location;
                    RegistryKey rk = Registry.LocalMachine;
                    RegistryKey rk2 = rk.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run");
                    rk2.DeleteValue("LJSchool", false);
                    rk2.Close();
                    rk.Close();
                }
            }
            catch (Exception ex)
            {
                Log.Error("设置开机启动失败", ex);
            }
        }



        public static bool IsAutoStartup
        {
            get
            {
                bool b;
                return bool.TryParse(ConfigManagerHelper.GetConfigByName(Config.IsAutoStart), out b) && b;
            }
            set
            {
                ConfigManagerHelper.SetConfigByName(Config.IsAutoStart, value.ToString());
                SetAutoStartup();
            }
        }

        public static string CurrentVersion
        {
            get { return Assembly.GetExecutingAssembly().GetName().Version.ToString(); }
        }

        public static Reponse.Login CurrentLogin
        {
            get; set;
        }
    }
}
