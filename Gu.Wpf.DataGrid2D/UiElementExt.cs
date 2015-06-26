namespace Gu.Wpf.DataGrid2D
{
    using System;
    using System.Windows;

    public static class UiElementExt
    {
        internal static void UpdateHandler(this UIElement element, RoutedEvent routedEvent, Delegate handler)
        {
            element.RemoveHandler(routedEvent, handler);
            element.AddHandler(routedEvent, handler);
        }
    }
}
