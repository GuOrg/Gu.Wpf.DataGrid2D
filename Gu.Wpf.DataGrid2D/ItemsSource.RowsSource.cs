namespace Gu.Wpf.DataGrid2D
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;

    public static partial class ItemsSource
    {
        /// <summary>
        /// An <see cref="IEnumerable"/> of rows where each row is an <see cref="IEnumerable"/> with the values.
        /// </summary>
        public static readonly DependencyProperty RowsSourceProperty = DependencyProperty.RegisterAttached(
            "RowsSource",
            typeof(IEnumerable),
            typeof(ItemsSource),
            new PropertyMetadata(
                default(IEnumerable),
                OnRowsSourceChanged),
            x => x is null || x is IEnumerable<IEnumerable>);

        /// <summary>Helper for setting <see cref="RowsSourceProperty"/> on <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="DataGrid"/> to set <see cref="RowsSourceProperty"/> on.</param>
        /// <param name="value">RowsSource property value.</param>
        public static void SetRowsSource(this DataGrid element, IEnumerable value)
        {
            if (element is null)
            {
                throw new System.ArgumentNullException(nameof(element));
            }

            element.SetValue(RowsSourceProperty, value);
        }

        /// <summary>Helper for getting <see cref="RowsSourceProperty"/> from <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="DataGrid"/> to read <see cref="RowsSourceProperty"/> from.</param>
        /// <returns>RowsSource property value.</returns>
        [AttachedPropertyBrowsableForChildren(IncludeDescendants = false)]
        [AttachedPropertyBrowsableForType(typeof(DataGrid))]
        public static IEnumerable GetRowsSource(this DataGrid element)
        {
            if (element is null)
            {
                throw new System.ArgumentNullException(nameof(element));
            }

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

            _ = dataGrid.Bind(ItemsSourceProxyProperty)
                        .OneWayTo(dataGrid, ItemsControl.ItemsSourceProperty);
            UpdateItemsSource(dataGrid);
        }
    }
}
