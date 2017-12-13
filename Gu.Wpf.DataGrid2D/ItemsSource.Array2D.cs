namespace Gu.Wpf.DataGrid2D
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;

    /// <summary>
    /// Attached properties for setting ItemsSource to 2D collections of different shapes.
    /// </summary>
    public static partial class ItemsSource
    {
        /// <summary>
        /// For setting the <see cref="ItemsControl.ItemsSourceProperty"/> to a two-dimensional array.
        /// </summary>
        public static readonly DependencyProperty Array2DProperty = DependencyProperty.RegisterAttached(
            "Array2D",
            typeof(Array),
            typeof(ItemsSource),
            new PropertyMetadata(
                default(Array),
                OnArray2DChanged),
            ValidateArray2D);

        /// <summary>
        /// Helper for setting Array2D property on a DataGrid.
        /// </summary>
        /// <param name="element">DataGrid to set Array2D property on.</param>
        /// <param name="value">Array2D property value.</param>
        public static void SetArray2D(this DataGrid element, Array value)
        {
            element.SetValue(Array2DProperty, value);
        }

        /// <summary>
        /// Helper for reading Array2D property from a DataGrid.
        /// </summary>
        /// <param name="element">DataGrid to read Array2D property from.</param>
        /// <returns>Array2D property value.</returns>
        [AttachedPropertyBrowsableForChildren(IncludeDescendants = false)]
        [AttachedPropertyBrowsableForType(typeof(DataGrid))]
        public static Array GetArray2D(this DataGrid element)
        {
            return (Array)element.GetValue(Array2DProperty);
        }

        private static void OnArray2DChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var dataGrid = (DataGrid)d;
            var array = (Array)e.NewValue;
            if (array == null)
            {
                BindingOperations.ClearBinding(dataGrid, ItemsControl.ItemsSourceProperty);
                return;
            }

            var array2DView = Array2DView.Create(array);
            dataGrid.Bind(ItemsControl.ItemsSourceProperty)
                    .OneWayTo(array2DView);
            dataGrid.RaiseEvent(new RoutedEventArgs(Events.ColumnsChangedEvent));
        }

        private static bool ValidateArray2D(object value)
        {
            if (value is Array array)
            {
                return array.Rank == 2;
            }

            return true;
        }
    }
}
