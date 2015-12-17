namespace Gu.Wpf.DataGrid2D
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;

    public static partial class Source2D
    {
        public static readonly DependencyProperty ItemsSource2DProperty = DependencyProperty.RegisterAttached(
            "ItemsSource2D",
            typeof(Array),
            typeof(Source2D),
            new PropertyMetadata(default(Array), OnItemsSource2DChanged),
            OnValidateItemsSource2D);

        public static void SetItemsSource2D(this DataGrid element, Array value)
        {
            element.SetValue(ItemsSource2DProperty, value);
        }

        [AttachedPropertyBrowsableForChildren(IncludeDescendants = false)]
        [AttachedPropertyBrowsableForType(typeof(DataGrid))]
        public static Array GetItemsSource2D(this DataGrid element)
        {
            return (Array)element.GetValue(ItemsSource2DProperty);
        }

        private static void OnItemsSource2DChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var dataGrid = (DataGrid)d;
            var array = (Array)e.NewValue;
            if (array == null)
            {
                BindingOperations.ClearBinding(dataGrid, ItemsControl.ItemsSourceProperty);
                return;
            }

            var array2DView = new Array2DView(array);
            dataGrid.Bind(ItemsControl.ItemsSourceProperty)
                    .OneWayTo(array2DView);
        }

        private static bool OnValidateItemsSource2D(object value)
        {
            var array = value as Array;
            if (array != null)
            {
                return array.Rank == 2;
            }

            return true;
        }
    }
}
