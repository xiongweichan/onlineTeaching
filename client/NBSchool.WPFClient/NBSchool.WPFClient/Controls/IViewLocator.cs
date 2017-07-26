using System;
using System.Windows;

namespace NBSchool.WPFClient.Controls
{
    public interface IViewLocator
    {
        UIElement GetOrCreateViewType(Type viewType);
    }
}