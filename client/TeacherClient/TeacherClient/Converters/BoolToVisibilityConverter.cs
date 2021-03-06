﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace TeacherClient
{
    public class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (value != null && value.ToString() == true.ToString())
                    return Visibility.Visible;
            }
            catch (Exception) { }
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (value != null && value.ToString() == Visibility.Visible.ToString())
                    return true;
            }
            catch (Exception) { }
            return false;
        }
    }

    public class BoolToVisibilityConverter2 : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (value != null && value.ToString() == true.ToString())
                    return Visibility.Collapsed;
            }
            catch (Exception) { }
            return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (value != null && value.ToString() == Visibility.Collapsed.ToString())
                    return true;
            }
            catch (Exception) { }
            return false;
        }
    }
}
