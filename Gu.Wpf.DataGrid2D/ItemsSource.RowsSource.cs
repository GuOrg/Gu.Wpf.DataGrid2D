namespace Gu.Wpf.DataGrid2D
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;

    public static partial class ItemsSource
    {
        public static readonly DependencyProperty RowsSourceProperty = DependencyProperty.RegisterAttached(
            "RowsSource",
            typeof(IEnumerable),
            typeof(ItemsSource),
            new PropertyMetadata(
                default(IEnumerable),
                OnRowsSourceChanged),
            ValidateRowsSource);

        /// <summary>
        /// Helper for setting RowsSource property on a DataGrid.
        /// </summary>
        /// <param name="element">DataGrid to set RowsSource property on.</param>
        /// <param name="value">RowsSource property value.</param>
        public static void SetRowsSource(this DataGrid element, IEnumerable value)
        {
            element.SetValue(RowsSourceProperty, value);
        }

        /// <summary>
        /// Helper for reading RowsSource property from a DataGrid.
        /// </summary>
        /// <param name="element">DataGrid to read RowsSource property from.</param>
        /// <returns>RowsSource property value.</returns>
        [AttachedPropertyBrowsableForChildren(IncludeDescendants = false)]
        [AttachedPropertyBrowsableForType(typeof(DataGrid))]
        public static IEnumerable GetRowsSource(this DataGrid element)
        {
            return (IEnumerable)element.GetValue(RowsSourceProperty);
        }

        private static void OnRowsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var dataGrid = (DataGrid)d;
            if (!(e.NewValue is IEnumerable<IEnumerable>))
            {
                BindingOperations.ClearBinding(dataGrid, ItemsControl.ItemsSourceProperty);
                BindingOperations.ClearBinding(dataGrid, ItemsSourceProxyProperty);
                return;
            }

            dataGrid.Bind(ItemsSourceProxyProperty)
                    .OneWayTo(dataGrid, ItemsControl.ItemsSourceProperty);
            UpdateItemsSource(dataGrid);
        }

        private static bool ValidateRowsSource(object value)
        {
            if (value == null)
            {
                return true;
            }

            return value is IEnumerable<IEnumerable>;
        }
    }
}
