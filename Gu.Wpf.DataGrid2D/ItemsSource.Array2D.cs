namespace Gu.Wpf.DataGrid2D
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;

    public static partial class ItemsSource
    {
        public static readonly DependencyProperty Array2DProperty = DependencyProperty.RegisterAttached(
            "Array2D",
            typeof(Array),
            typeof(ItemsSource),
            new PropertyMetadata(
                default(Array),
                OnArray2DChanged),
            Array2DValidateValue);

        public static void SetArray2D(this DataGrid element, Array value)
        {
            element.SetValue(Array2DProperty, value);
        }

        [AttachedPropertyBrowsableForChildren(IncludeDescendants = false)]
        [AttachedPropertyBrowsableForType(typeof(DataGrid))]
        public static Array GetArray2D(this DataGrid element)
        {
            return (Array)element.GetValue(Array2DProperty);
        }

        private static void OnArray2DChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var dataGrid = (DataGrid)d;
            dataGrid.AutoGeneratingColumn -= OnDataGridAutoGeneratingColumn;
            var array = (Array)e.NewValue;
            if (array == null)
            {
                BindingOperations.ClearBinding(dataGrid, ItemsControl.ItemsSourceProperty);
                return;
            }

            var array2DView = Array2DView.Create(array);
            dataGrid.Bind(ItemsControl.ItemsSourceProperty)
                    .OneWayTo(array2DView);
            dataGrid.AutoGeneratingColumn += OnDataGridAutoGeneratingColumn;
            dataGrid.RaiseEvent(new RoutedEventArgs(Events.ColumnsChanged));
        }

        private static void OnDataGridAutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            CustomDataGridTemplateColumn col = new CustomDataGridTemplateColumn();
            col.CellTemplate = ((DataGrid)sender).GetCellTemplate();
            col.CellEditingTemplate = ((DataGrid)sender).GetCellEditingTemplate();
            if (col.CellTemplate != null && col.CellEditingTemplate != null)
            {
                DataGridTextColumn tc = e.Column as DataGridTextColumn;
                if (tc?.Binding != null)
                {
                    col.Binding = tc.Binding;
                    e.Column = col;
                }
            }
        }

        private static bool Array2DValidateValue(object value)
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
