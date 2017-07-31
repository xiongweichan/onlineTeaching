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
using TeacherClient.Core;

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
            ConfigManagerHelper.Init(Config.SystemConfigPath, true);
            SetAutoStartup();
            IPCHandle.Init();
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
                ConfigManagerHelper.SetConfigByName(Config.IsAutoStart, false.ToString());
                SetAutoStartup();
            }
        }




        public static string CurrentVersion
        {
            get { return Assembly.GetExecutingAssembly().GetName().Version.ToString(); }
        }
    }
}
