namespace Gu.Wpf.DataGrid2D
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;

    public static partial class ItemsSource
    {
        public static readonly DependencyProperty ColumnHeadersSourceProperty = DependencyProperty.RegisterAttached(
            "ColumnHeadersSource",
            typeof(IEnumerable),
            typeof(ItemsSource),
            new PropertyMetadata(null, OnColumnHeadersChanged),
            OnValidateHeaders);

        private static readonly RoutedEventHandler OnColumnsChangedHandler = OnColumnsChanged;

        private static readonly DependencyProperty ColumnHeaderListenerProperty = DependencyProperty.RegisterAttached(
            "ColumnHeaderListener",
            typeof(ColumnHeaderListener),
            typeof(ItemsSource),
            new PropertyMetadata(default(ColumnHeaderListener)));

        public static void SetColumnHeadersSource(this DataGrid element, IEnumerable value)
        {
            element.SetValue(ColumnHeadersSourceProperty, value);
        }

        [AttachedPropertyBrowsableForChildren(IncludeDescendants = false)]
        [AttachedPropertyBrowsableForType(typeof(DataGrid))]
        public static IEnumerable GetColumnHeadersSource(this DataGrid element)
        {
            return (IEnumerable)element.GetValue(ColumnHeadersSourceProperty);
        }

        private static void OnColumnHeadersChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var dataGrid = (DataGrid)d;
            var headers = (IEnumerable)e.NewValue;
            (dataGrid.GetValue(ColumnHeaderListenerProperty) as IDisposable)?.Dispose();
            dataGrid.ClearValue(ColumnHeaderListenerProperty);

            if (headers == null)
            {
                foreach (var column in dataGrid.Columns)
                {
                    BindingOperations.ClearBinding(column, DataGridColumn.HeaderProperty);
                }

                dataGrid.RemoveHandler(ColumnsChangedEvent, OnColumnsChangedHandler);
                return;
            }

            dataGrid.SetValue(ColumnHeaderListenerProperty, new ColumnHeaderListener(dataGrid));
            dataGrid.UpdateHandler(ColumnsChangedEvent, OnColumnsChangedHandler);
            OnColumnsChanged(dataGrid, null);
        }

        private static void OnColumnsChanged(object sender, RoutedEventArgs routedEventArgs)
        {
            var dataGrid = (DataGrid)sender;
            var headers = dataGrid.GetColumnHeadersSource();
            var count = headers.Count();
            for (int i = 0; i < Math.Min(count, dataGrid.Columns.Count); i++)
            {
                var column = dataGrid.Columns[i];
                column.Bind(DataGridColumn.HeaderProperty)
                      .OneWayTo(headers, i);
            }
        }

        private static bool OnValidateHeaders(object value)
        {
            if (value == null)
            {
                return true;
            }

            var list = value as IList;
            if (list != null)
            {
                return true;
            }

            var rol = value as IReadOnlyList<object>;
            if (rol != null)
            {
                return true;
            }

            return false;
        }
    }
}
