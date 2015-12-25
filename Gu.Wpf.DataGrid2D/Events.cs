namespace Gu.Wpf.DataGrid2D
{
    using System.Windows;

    internal static class Events
    {
        internal static readonly RoutedEvent ColumnsChanged = EventManager.RegisterRoutedEvent(
            "ColumnsChanged",
            RoutingStrategy.Direct,
            typeof(RoutedEventHandler),
            typeof(ItemsSource));

        internal static readonly RoutedEvent RowsChanged = EventManager.RegisterRoutedEvent(
            "RowsChanged",
            RoutingStrategy.Direct,
            typeof(RoutedEventHandler),
            typeof(ItemsSource));

        internal static void AddColumnsChangedHandler(this UIElement o, RoutedEventHandler handler)
        {
            o.AddHandler(ColumnsChanged, handler);
        }

        internal static void RemoveColumnsChangedHandler(this UIElement o, RoutedEventHandler handler)
        {
            o.RemoveHandler(ColumnsChanged, handler);
        }

        internal static void AddRowsChangedHandler(this UIElement o, RoutedEventHandler handler)
        {
            o.AddHandler(RowsChanged, handler);
        }

        internal static void RemoveRowsChangedHandler(this UIElement o, RoutedEventHandler handler)
        {
            o.RemoveHandler(RowsChanged, handler);
        }
    }
}
