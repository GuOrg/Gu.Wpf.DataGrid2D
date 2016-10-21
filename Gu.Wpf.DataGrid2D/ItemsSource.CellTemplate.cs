namespace Gu.Wpf.DataGrid2D
{
    using System.Windows;
    using System.Windows.Controls;

    public static partial class ItemsSource
    {
        public static readonly DependencyProperty CellTemplateProperty = DependencyProperty.RegisterAttached(
            "CellTemplate",
            typeof(DataTemplate),
            typeof(ItemsSource),
            new PropertyMetadata(null, OnCellTemplateChanged),
            null);

        public static void SetCellTemplate(this DataGrid element, DataTemplate value)
        {
            element.SetValue(CellTemplateProperty, value);
        }

        [AttachedPropertyBrowsableForChildren(IncludeDescendants = false)]
        [AttachedPropertyBrowsableForType(typeof(DataGrid))]
        public static DataTemplate GetCellTemplate(this DataGrid element)
        {
            return (DataTemplate)element.GetValue(CellTemplateProperty);
        }

        private static void OnCellTemplateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var dataGrid = (DataGrid)d;
            var celltemplate = (DataTemplate)e.NewValue;

            var a = dataGrid.GetArray2D();
            dataGrid.SetArray2D(null);
            dataGrid.SetCellTemplate(celltemplate);
            dataGrid.SetArray2D(a);
        }
    }
}
