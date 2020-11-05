namespace Gu.Wpf.DataGrid2D
{
    using System.Windows;

    internal static class Events
    {
        internal static readonly RoutedEvent ColumnsChangedEvent = EventManager.RegisterRoutedEvent(
            "ColumnsChanged",
            RoutingStrategy.Direct,
            typeof(RoutedEventHandler),
            typeof(Events));

        internal static readonly RoutedEvent RowsChangedEvent = EventManager.RegisterRoutedEvent(
            "RowsChanged",
            RoutingStrategy.Direct,
            typeof(RoutedEventHandler),
            typeof(Events));

        internal static void AddColumnsChangedHandler(this UIElement o, RoutedEventHandler handler)
        {
            o.AddHandler(ColumnsChangedEvent, handler);
        }

        internal static void RemoveColumnsChangedHandler(this UIElement o, RoutedEventHandler handler)
        {
            o.RemoveHandler(ColumnsChangedEvent, handler);
        }

        internal static void AddRowsChangedHandler(this UIElement o, RoutedEventHandler handler)
        {
            o.AddHandler(RowsChangedEvent, handler);
        }

        internal static void RemoveRowsChangedHandler(this UIElement o, RoutedEventHandler handler)
        {
            o.RemoveHandler(RowsChangedEvent, handler);
        }
    }
}
