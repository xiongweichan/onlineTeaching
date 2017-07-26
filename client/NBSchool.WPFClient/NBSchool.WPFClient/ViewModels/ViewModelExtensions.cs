using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Caliburn.Micro;

namespace NBSchool.WPFClient.ViewModels
{
    public static class ViewModelExtensions
    {
        public static bool Set<T>(this INotifyPropertyChangedEx propertyChanged, ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (propertyChanged == null)
                throw new ArgumentNullException("propertyChanged");

            if (String.IsNullOrEmpty(propertyName))
                throw new ArgumentNullException("propertyName");

            if (EqualityComparer<T>.Default.Equals(field, value))
                return false;

            field = value;

            propertyChanged.NotifyOfPropertyChange(propertyName);

            return true;
        }
    }
}
