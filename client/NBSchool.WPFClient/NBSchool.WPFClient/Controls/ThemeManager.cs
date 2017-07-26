using System;
using System.ComponentModel.Composition;
using System.Windows;

namespace NBSchool.WPFClient.Controls
{
    [Export(typeof(IThemeManager))]
    public class ThemeManager : IThemeManager
    {
        private readonly ResourceDictionary themeResources;

        public ThemeManager()
        {
            this.themeResources = new ResourceDictionary
                                      {
                                          Source =
                                              new Uri("pack://application:,,,/Resources/MahApps.Theme.xaml")
                                      };
        }

        public ResourceDictionary GetThemeResources()
        {
            return this.themeResources;
        }
    }
}