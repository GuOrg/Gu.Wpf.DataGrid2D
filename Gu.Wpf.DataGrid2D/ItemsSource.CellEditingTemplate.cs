namespace Gu.Wpf.DataGrid2D
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;

    public static partial class ItemsSource
    {
        public static readonly DependencyProperty CellEditingTemplateProperty = DependencyProperty.RegisterAttached(
            "CellEditingTemplate",
            typeof(DataTemplate),
            typeof(ItemsSource),
            new PropertyMetadata(null, OnCellEditingTemplateChanged),
            OnValidateCellEditingTemplate);

        public static void SetCellEditingTemplate(this DataGrid element, DataTemplate value)
        {
            element.SetValue(CellEditingTemplateProperty, value);
        }

        [AttachedPropertyBrowsableForChildren(IncludeDescendants = false)]
        [AttachedPropertyBrowsableForType(typeof(DataGrid))]
        public static DataTemplate GetCellEditingTemplate(this DataGrid element)
        {
            return (DataTemplate)element.GetValue(CellEditingTemplateProperty);
        }

        private static void OnCellEditingTemplateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            // TODO
        }

        private static bool OnValidateCellEditingTemplate(object value)
        {
            return true;
        }
    }
}
