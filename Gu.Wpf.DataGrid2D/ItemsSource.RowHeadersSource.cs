namespace Gu.Wpf.DataGrid2D
{
    using System;
    using System.Collections;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;

    public static partial class ItemsSource
    {
        public static readonly DependencyProperty RowHeadersSourceProperty = DependencyProperty.RegisterAttached(
            "RowHeadersSource",
            typeof(IEnumerable),
            typeof(ItemsSource),
            new PropertyMetadata(null, OnRowHeadersChanged),
            OnValidateHeaders);

        private static readonly RoutedEventHandler OnRowsChangedHandler = OnRowsChanged;

        private static readonly DependencyProperty RowHeaderListenerProperty = DependencyProperty.RegisterAttached(
            "RowHeaderListener",
            typeof(RowHeaderListener),
            typeof(ItemsSource),
            new PropertyMetadata(default(RowHeaderListener)));

        public static void SetRowHeadersSource(this DataGrid element, IEnumerable value)
        {
            element.SetValue(RowHeadersSourceProperty, value);
        }

        [AttachedPropertyBrowsableForChildren(IncludeDescendants = false)]
        [AttachedPropertyBrowsableForType(typeof(DataGrid))]
        public static IEnumerable GetRowHeadersSource(this DataGrid element)
        {
            return (IEnumerable)element.GetValue(RowHeadersSourceProperty);
        }

        private static void OnRowHeadersChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var dataGrid = (DataGrid)d;
            var headers = (IEnumerable)e.NewValue;
            (dataGrid.GetValue(RowHeaderListenerProperty) as IDisposable)?.Dispose();
            dataGrid.ClearValue(RowHeaderListenerProperty);

            if (headers == null)
            {
                foreach (DataGridRow row in dataGrid.Items)
                {
                    BindingOperations.ClearBinding(row, DataGridRow.HeaderProperty);
                }

                dataGrid.RemoveHandler(RowsChangedEvent, OnRowsChangedHandler);
                return;
            }

            dataGrid.SetValue(RowHeaderListenerProperty, new RowHeaderListener(dataGrid));
            dataGrid.UpdateHandler(RowsChangedEvent, OnRowsChangedHandler);
            OnRowsChanged(dataGrid, null);
        }

        private static void OnRowsChanged(object sender, RoutedEventArgs routedEventArgs)
        {
            var dataGrid = (DataGrid)sender;
            var headers = dataGrid.GetRowHeadersSource();
            var count = headers.Count();
            for (int i = 0; i < Math.Min(count, dataGrid.Items.Count); i++)
            {
                var row = dataGrid.ItemContainerGenerator.ContainerFromIndex(i) as DataGridRow;
                row?.Bind(DataGridRow.HeaderProperty)
                    .OneWayTo(headers, i);
            }
        }
    }
}
