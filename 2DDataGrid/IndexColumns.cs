namespace _2DDataGrid
{
    using System;
    using System.Windows;
    using System.Windows.Controls;

    public static class IndexColumns
    {
        public static readonly DependencyProperty HeadersProperty = DependencyProperty.RegisterAttached(
            "Headers",
            typeof(object[]),
            typeof(IndexColumns),
            new PropertyMetadata(default(object[]), OnHeadersChanged));

        private static readonly RoutedEventHandler OnLoadedHandler = new RoutedEventHandler(OnLoaded);

        public static void SetHeaders(this DataGrid element, object[] value)
        {
            element.SetValue(HeadersProperty, value);
        }

        public static object[] GetHeaders(this DataGrid element)
        {
            return (object[])element.GetValue(HeadersProperty);
        }

        private static void OnHeadersChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var dataGrid = (DataGrid)d;
            if (dataGrid.IsLoaded)
            {
                ApplyHeades(dataGrid);
            }
            dataGrid.RemoveHandler(FrameworkElement.LoadedEvent, OnLoadedHandler);
            dataGrid.AddHandler(FrameworkElement.LoadedEvent, OnLoadedHandler);
        }

        private static void OnLoaded(object sender, RoutedEventArgs e)
        {
            var dataGrid = (DataGrid)sender;
            dataGrid.RemoveHandler(FrameworkElement.LoadedEvent, OnLoadedHandler);
            ApplyHeades(dataGrid);
        }

        private static void ApplyHeades(DataGrid dataGrid)
        {
            var objects = dataGrid.GetHeaders();
            if (objects == null)
            {
                dataGrid.Columns.Clear();
                return;
            }
            for (int i = 0; i < objects.Length; i++)
            {
                var templateColumn = new IndexColumn(dataGrid, objects, i);
                dataGrid.Columns.Add(templateColumn);
            }
        }
    }

    public class IndexColumn : DataGridTemplateColumn
    {
        public IndexColumn(DataGrid dataGrid, object[] headers, int index)
        {
            Index = index;
            Header = headers[index];
            CellTemplate = dataGrid.ItemTemplate;
        }

        public int Index { get; private set; }

        protected override FrameworkElement GenerateEditingElement(DataGridCell cell, object dataItem)
        {
            cell.DataContext = GetArrayItem(dataItem);
            return base.GenerateEditingElement(cell, GetArrayItem(dataItem));
        }

        protected override FrameworkElement GenerateElement(DataGridCell cell, object dataItem)
        {
            var frameworkElement = base.GenerateElement(cell, dataItem);
            frameworkElement.DataContext = GetArrayItem(dataItem);
            return frameworkElement;
        }

        protected override void RefreshCellContent(FrameworkElement element, string propertyName)
        {
            base.RefreshCellContent(element, propertyName);
        }

        private object GetArrayItem(object dataItem)
        {
            var objects = (Array)dataItem;
            return objects.GetValue(Index);
        }
    }
}
