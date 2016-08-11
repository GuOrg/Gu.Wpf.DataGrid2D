namespace Gu.Wpf.DataGrid2D
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;

    public static partial class ItemsSource
    {
        public static readonly DependencyProperty CellTemplateProperty = DependencyProperty.RegisterAttached(
            "CellTemplate",
            typeof(DataTemplate),
            typeof(ItemsSource),
            new PropertyMetadata(null, OnCellTemplateChanged),
            OnValidateCellTemplate);
        
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
            
            dataGrid.SetCellTemplate(celltemplate);
            dataGrid.RaiseEvent(new RoutedEventArgs(Events.ColumnsChanged));
        }
        
        private static bool OnValidateCellTemplate(object value)
        {
            return true;
        }
    }
}
