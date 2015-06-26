using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Gu.Wpf.DataGrid2D
{
    public class IndexColumn : DataGridTemplateColumn
    {
        private static readonly Dictionary<int, PropertyPath> IndexPaths = new Dictionary<int, PropertyPath>();
        private static readonly Dictionary<DependencyProperty, PropertyPath> PropertyPaths = new Dictionary<DependencyProperty, PropertyPath>();
        public IndexColumn(DataGrid dataGrid, object[] headers, int index)
        {
            Index = index;
            Bind(this, HeaderProperty, headers, GetPath(index));
            CellTemplate = dataGrid.ItemTemplate;
        }

        public int Index { get; private set; }

        protected override FrameworkElement GenerateElement(DataGridCell cell, object dataItem)
        {
            Bind(cell, FrameworkElement.DataContextProperty, dataItem, GetPath(Index));
            var frameworkElement = base.GenerateElement(cell, dataItem);
            return frameworkElement;
        }

        protected override void RefreshCellContent(FrameworkElement element, string propertyName)
        {
            base.RefreshCellContent(element, propertyName);
        }

        private static void Bind(DependencyObject target, DependencyProperty targetProperty, object source, PropertyPath path)
        {
            var binding = new Binding
            {
                Path = path,
                Source = source,
                Mode = BindingMode.OneWay,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            };
            BindingOperations.SetBinding(target, targetProperty, binding);
        }

        private static PropertyPath GetPath(int index)
        {
            PropertyPath path;
            if (!IndexPaths.TryGetValue(index, out path))
            {
                path = new PropertyPath(string.Format("[{0}]", index));
                IndexPaths[index] = path;
            }
            return path;
        }

        private static PropertyPath GetPath(DependencyProperty property)
        {
            PropertyPath path;
            if (!PropertyPaths.TryGetValue(property, out path))
            {
                path = new PropertyPath(property.Name);
                PropertyPaths[property] = path;
            }
            return path;
        }
    }
}