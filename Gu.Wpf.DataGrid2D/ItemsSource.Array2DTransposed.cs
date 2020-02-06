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
            Validate2DArray);

        /// <summary>Helper for setting <see cref="Array2DTransposedProperty"/> on <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="DataGrid"/> to set <see cref="Array2DTransposedProperty"/> on.</param>
        /// <param name="value">Array2DTransposed property value.</param>
        public static void SetArray2DTransposed(this DataGrid element, Array? value)
        {
            if (element is null)
            {
                throw new ArgumentNullException(nameof(element));
            }

            element.SetValue(Array2DTransposedProperty, value);
        }

        /// <summary>Helper for getting <see cref="Array2DTransposedProperty"/> from <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="DataGrid"/> to read <see cref="Array2DTransposedProperty"/> from.</param>
        /// <returns>Array2DTransposed property value.</returns>
        [AttachedPropertyBrowsableForChildren(IncludeDescendants = false)]
        [AttachedPropertyBrowsableForType(typeof(DataGrid))]
        public static Array? GetArray2DTransposed(this DataGrid element)
        {
            if (element is null)
            {
                throw new ArgumentNullException(nameof(element));
            }

            return (Array)element.GetValue(Array2DTransposedProperty);
        }

        private static void OnArray2DTransposedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var dataGrid = (DataGrid)d;
            var array = (Array)e.NewValue;
            if (array is null)
            {
                BindingOperations.ClearBinding(dataGrid, ItemsControl.ItemsSourceProperty);
                return;
            }

            var array2DView = Array2DView.CreateTransposed(array);
            _ = dataGrid.Bind(ItemsControl.ItemsSourceProperty)
                        .OneWayTo(array2DView);
            dataGrid.RaiseEvent(new RoutedEventArgs(Events.ColumnsChangedEvent));
        }
    }
}
