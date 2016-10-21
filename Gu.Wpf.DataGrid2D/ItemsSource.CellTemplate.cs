namespace Gu.Wpf.DataGrid2D
{
    using System.Windows;
    using System.Windows.Controls;

    public static partial class Cell
    {
        public static readonly DependencyProperty TemplateProperty = DependencyProperty.RegisterAttached(
            "Template",
            typeof(DataTemplate),
            typeof(Cell),
            new PropertyMetadata(null, OnTemplateChanged));

        public static readonly DependencyProperty EditingTemplateProperty = DependencyProperty.RegisterAttached(
            "EditingTemplate",
            typeof(DataTemplate),
            typeof(Cell),
            new PropertyMetadata(null, OnEditingTemplateChanged));

        public static void SetTemplate(this DataGrid element, DataTemplate value)
        {
            element.SetValue(TemplateProperty, value);
        }

        [AttachedPropertyBrowsableForChildren(IncludeDescendants = false)]
        [AttachedPropertyBrowsableForType(typeof(DataGrid))]
        public static DataTemplate GetTemplate(this DataGrid element)
        {
            return (DataTemplate)element.GetValue(TemplateProperty);
        }

        public static void SetEditingTemplate(this DataGrid element, DataTemplate value)
        {
            element.SetValue(EditingTemplateProperty, value);
        }

        [AttachedPropertyBrowsableForChildren(IncludeDescendants = false)]
        [AttachedPropertyBrowsableForType(typeof(DataGrid))]
        public static DataTemplate GetEditingTemplate(this DataGrid element)
        {
            return (DataTemplate)element.GetValue(EditingTemplateProperty);
        }

        private static void OnTemplateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var dataGrid = (DataGrid)d;
            var celltemplate = (DataTemplate)e.NewValue;

            var a = dataGrid.GetArray2D();
            dataGrid.SetArray2D(null);
            dataGrid.SetTemplate(celltemplate);
            dataGrid.SetArray2D(a);
        }

        private static void OnEditingTemplateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var dataGrid = (DataGrid)d;
            var celltemplate = (DataTemplate)e.NewValue;

            dataGrid.SetEditingTemplate(celltemplate);
            dataGrid.RaiseEvent(new RoutedEventArgs(Events.ColumnsChanged));
        }
    }
}
