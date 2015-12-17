namespace Gu.Wpf.DataGrid2D
{
    using System.Windows;

    public static partial class ItemsSource
    {
        public static readonly RoutedEvent ColumnsChangedEvent = EventManager.RegisterRoutedEvent(
            "ColumnsChanged",
            RoutingStrategy.Direct,
            typeof(RoutedEventHandler),
            typeof(ItemsSource));

        public static readonly RoutedEvent RowsChangedEvent = EventManager.RegisterRoutedEvent(
            "RowsChanged",
            RoutingStrategy.Direct,
            typeof(RoutedEventHandler),
            typeof(ItemsSource));

        public static void AddColumnsChangedHandler(this UIElement o, RoutedEventHandler handler)
        {
            o.AddHandler(ColumnsChangedEvent, handler);
        }

        public static void RemoveColumnsChangedHandler(this UIElement o, RoutedEventHandler handler)
        {
            o.RemoveHandler(ColumnsChangedEvent, handler);
        }

        public static void AddRowsChangedHandler(this UIElement o, RoutedEventHandler handler)
        {
            o.AddHandler(RowsChangedEvent, handler);
        }

        public static void RemoveRowsChangedHandler(this UIElement o, RoutedEventHandler handler)
        {
            o.RemoveHandler(RowsChangedEvent, handler);
        }
    }
}
