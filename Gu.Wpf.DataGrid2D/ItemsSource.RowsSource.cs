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
            new PropertyMetadata(default(IEnumerable), OnRowsSourceChanged),
            OnValidateRowsSource);

        public static void SetRowsSource(this DataGrid element, IEnumerable value)
        {
            element.SetValue(RowsSourceProperty, value);
        }

        [AttachedPropertyBrowsableForChildren(IncludeDescendants = false)]
        [AttachedPropertyBrowsableForType(typeof(DataGrid))]
        public static IEnumerable GetRowsSource(this DataGrid element)
        {
            return (IEnumerable)element.GetValue(RowsSourceProperty);
        }

        private static void OnRowsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var dataGrid = (DataGrid)d;
            var array = (IEnumerable<IEnumerable>)e.NewValue;
            if (array == null)
            {
                BindingOperations.ClearBinding(dataGrid, ItemsControl.ItemsSourceProperty);
                return;
            }

            var view = Lists2DView.Create(array);
            dataGrid.Bind(ItemsControl.ItemsSourceProperty)
                    .OneWayTo(view);
            dataGrid.RaiseEvent(new RoutedEventArgs(Events.ColumnsChanged));
        }

        private static bool OnValidateRowsSource(object value)
        {
            if (value == null)
            {
                return true;
            }

            return value is IEnumerable<IEnumerable>;
        }
    }
}
