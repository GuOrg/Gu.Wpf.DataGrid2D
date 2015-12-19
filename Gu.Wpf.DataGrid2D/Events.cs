namespace Gu.Wpf.DataGrid2D
{
    using System.Windows;

    public static class Events
    {
        public static readonly RoutedEvent ColumnsChanged = EventManager.RegisterRoutedEvent(
            "ColumnsChanged",
            RoutingStrategy.Direct,
            typeof (RoutedEventHandler),
            typeof (ItemsSource));

        public static readonly RoutedEvent RowsChanged = EventManager.RegisterRoutedEvent(
            "RowsChanged",
            RoutingStrategy.Direct,
            typeof (RoutedEventHandler),
            typeof (ItemsSource));

        public static void AddColumnsChangedHandler(this UIElement o, RoutedEventHandler handler)
        {
            o.AddHandler(ColumnsChanged, handler);
        }

        public static void RemoveColumnsChangedHandler(this UIElement o, RoutedEventHandler handler)
        {
            o.RemoveHandler(ColumnsChanged, handler);
        }

        public static void AddRowsChangedHandler(this UIElement o, RoutedEventHandler handler)
        {
            o.AddHandler(RowsChanged, handler);
        }

        public static void RemoveRowsChangedHandler(this UIElement o, RoutedEventHandler handler)
        {
            o.RemoveHandler(RowsChanged, handler);
        }
    }
}
