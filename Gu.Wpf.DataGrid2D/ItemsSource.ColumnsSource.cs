namespace Gu.Wpf.DataGrid2D
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;

    public static partial class ItemsSource
    {
        public static readonly DependencyProperty ColumnsSourceProperty = DependencyProperty.RegisterAttached(
            "ColumnsSource",
            typeof(IEnumerable),
            typeof(ItemsSource),
            new PropertyMetadata(default(IEnumerable), OnColumnsSourceChanged),
            OnValidateRowsSource);

        public static void SetColumnsSource(this DataGrid element, IEnumerable value)
        {
            element.SetValue(ColumnsSourceProperty, value);
        }

        [AttachedPropertyBrowsableForChildren(IncludeDescendants = false)]
        [AttachedPropertyBrowsableForType(typeof(DataGrid))]
        public static IEnumerable GetColumnsSource(this DataGrid element)
        {
            return (IEnumerable)element.GetValue(ColumnsSourceProperty);
        }

        private static void OnColumnsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var dataGrid = (DataGrid)d;
            var source = (IEnumerable<IEnumerable>)e.NewValue;
            if (source == null)
            {
                BindingOperations.ClearBinding(dataGrid, ItemsControl.ItemsSourceProperty);
                return;
            }

            var view = Lists2DView.CreateTransposed(source);
            dataGrid.Bind(ItemsControl.ItemsSourceProperty)
                    .OneWayTo(view);
            dataGrid.RaiseEvent(new RoutedEventArgs(Events.ColumnsChanged));
        }
    }
}
