namespace Gu.Wpf.DataGrid2D
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using Gu.Wpf.DataGrid2D.Internals;

    public static partial class ItemsSource
    {
        public static readonly DependencyProperty ColumnsSourceProperty = DependencyProperty.RegisterAttached(
            "ColumnsSource",
            typeof(IEnumerable),
            typeof(ItemsSource),
            new PropertyMetadata(default(IEnumerable), OnColumnsSourceChanged),
            ValidateColumnsSource);

        /// <summary>
        /// Helper for setting ColumnsSource property on a DataGrid.
        /// </summary>
        /// <param name="element">DataGrid to set ColumnsSource property on.</param>
        /// <param name="value">ColumnsSource property value.</param>
        public static void SetColumnsSource(this DataGrid element, IEnumerable value)
        {
            element.SetValue(ColumnsSourceProperty, value);
        }

        /// <summary>
        /// Helper for reading ColumnsSource property from a DataGrid.
        /// </summary>
        /// <param name="element">DataGrid to read ColumnsSource property from.</param>
        /// <returns>ColumnsSource property value.</returns>
        [AttachedPropertyBrowsableForChildren(IncludeDescendants = false)]
        [AttachedPropertyBrowsableForType(typeof(DataGrid))]
        public static IEnumerable GetColumnsSource(this DataGrid element)
        {
            return (IEnumerable)element.GetValue(ColumnsSourceProperty);
        }

        private static void OnColumnsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var dataGrid = (DataGrid)d;
            if (!(e.NewValue is IEnumerable<IEnumerable>))
            {
                BindingOperations.ClearBinding(dataGrid, ItemsControl.ItemsSourceProperty);
                BindingOperations.ClearBinding(dataGrid, ItemsSourceProxyProperty);
                return;
            }

            dataGrid.Bind(ItemsSourceProxyProperty)
                    .OneWayTo(dataGrid, ItemsControl.ItemsSourceProperty)
                    .IgnoreReturnValue();
            UpdateItemsSource(dataGrid);
        }

        private static bool ValidateColumnsSource(object value)
        {
            return ValidateRowsSource(value);
        }
    }
}
