namespace Gu.Wpf.DataGrid2D
{
    using System;
    using System.Collections;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;

    public static partial class ItemsSource
    {
        /// <summary>
        /// An <see cref="IEnumerable"/> with values to display in the row headers.
        /// </summary>
        public static readonly DependencyProperty RowHeadersSourceProperty = DependencyProperty.RegisterAttached(
            "RowHeadersSource",
            typeof(IEnumerable),
            typeof(ItemsSource),
            new PropertyMetadata(null, OnRowHeadersSourceChanged),
            HeadersSourceValidateValue);

        private static readonly RoutedEventHandler OnRowsChangedHandler = OnRowsChanged;

        private static readonly DependencyProperty RowHeaderListenerProperty = DependencyProperty.RegisterAttached(
            "RowHeaderListener",
            typeof(RowHeaderListener),
            typeof(ItemsSource),
            new PropertyMetadata(default(RowHeaderListener)));

        /// <summary>Helper for setting <see cref="RowHeadersSourceProperty"/> on <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="DataGrid"/> to set <see cref="RowHeadersSourceProperty"/> on.</param>
        /// <param name="value">RowHeadersSource property value.</param>
        public static void SetRowHeadersSource(this DataGrid element, IEnumerable? value)
        {
            if (element is null)
            {
                throw new ArgumentNullException(nameof(element));
            }

            element.SetValue(RowHeadersSourceProperty, value);
        }

        /// <summary>Helper for getting <see cref="RowHeadersSourceProperty"/> from <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="DataGrid"/> to read <see cref="RowHeadersSourceProperty"/> from.</param>
        /// <returns>RowHeadersSource property value.</returns>
        [AttachedPropertyBrowsableForChildren(IncludeDescendants = false)]
        [AttachedPropertyBrowsableForType(typeof(DataGrid))]
        public static IEnumerable? GetRowHeadersSource(this DataGrid element)
        {
            if (element is null)
            {
                throw new ArgumentNullException(nameof(element));
            }

            return (IEnumerable)element.GetValue(RowHeadersSourceProperty);
        }

        private static void OnRowHeadersSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var dataGrid = (DataGrid)d;
            var headers = (IEnumerable)e.NewValue;
#pragma warning disable IDISP007 // Don't dispose injected.
            (dataGrid.GetValue(RowHeaderListenerProperty) as IDisposable)?.Dispose();
#pragma warning restore IDISP007 // Don't dispose injected.
            dataGrid.ClearValue(RowHeaderListenerProperty);

            if (headers == null)
            {
                for (var i = 0; i < dataGrid.Items.Count; i++)
                {
                    if (dataGrid.ItemContainerGenerator.ContainerFromIndex(i) is DataGridRow row)
                    {
                        BindingOperations.ClearBinding(row, DataGridRow.HeaderProperty);
                    }
                }

                dataGrid.RemoveHandler(Events.RowsChangedEvent, OnRowsChangedHandler);
                return;
            }

#pragma warning disable IDISP004, CA2000  // Don't ignore return value of type IDisposable. disposed in beginning of method.
            dataGrid.SetCurrentValue(RowHeaderListenerProperty, new RowHeaderListener(dataGrid));
#pragma warning restore IDISP004, CA2000  // Don't ignore return value of type IDisposable.

            dataGrid.UpdateHandler(Events.RowsChangedEvent, OnRowsChangedHandler);
            OnRowsChanged(dataGrid, null);
        }

        private static void OnRowsChanged(object sender, RoutedEventArgs routedEventArgs)
        {
            var dataGrid = (DataGrid)sender;
            var headers = dataGrid.GetRowHeadersSource();
            var count = headers.Count();
            for (var i = 0; i < Math.Min(count, dataGrid.Items.Count); i++)
            {
                var row = dataGrid.ItemContainerGenerator.ContainerFromIndex(i) as DataGridRow;
                row?.Bind(DataGridRow.HeaderProperty)
                    .OneWayTo(headers, i);
            }
        }
    }
}
