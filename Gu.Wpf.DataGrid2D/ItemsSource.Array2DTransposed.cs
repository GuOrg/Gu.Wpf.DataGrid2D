namespace Gu.Wpf.DataGrid2D
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;

    public static partial class ItemsSource
    {
        public static readonly DependencyProperty Array2DTransposedProperty = DependencyProperty.RegisterAttached(
            "Array2DTransposed",
            typeof(Array),
            typeof(ItemsSource),
            new PropertyMetadata(
                default(Array),
                OnArray2DTransposedChanged),
#pragma warning disable WPF0007 // Name of ValidateValueCallback should match registered name.
            Array2DValidateValue);
#pragma warning restore WPF0007 // Name of ValidateValueCallback should match registered name.

        public static void SetArray2DTransposed(this DataGrid element, Array value)
        {
            element.SetValue(Array2DTransposedProperty, value);
        }

        [AttachedPropertyBrowsableForChildren(IncludeDescendants = false)]
        [AttachedPropertyBrowsableForType(typeof(DataGrid))]
        public static Array GetArray2DTransposed(this DataGrid element)
        {
            return (Array)element.GetValue(Array2DTransposedProperty);
        }

        private static void OnArray2DTransposedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var dataGrid = (DataGrid)d;
            dataGrid.AutoGeneratingColumn -= OnDataGridAutoGeneratingColumn;
            var array = (Array)e.NewValue;
            if (array == null)
            {
                BindingOperations.ClearBinding(dataGrid, ItemsControl.ItemsSourceProperty);
                return;
            }

            var array2DView = Array2DView.CreateTransposed(array);
            dataGrid.AutoGeneratingColumn += OnDataGridAutoGeneratingColumn;
            dataGrid.Bind(ItemsControl.ItemsSourceProperty)
                    .OneWayTo(array2DView);
            dataGrid.RaiseEvent(new RoutedEventArgs(Events.ColumnsChanged));
        }
    }
}
