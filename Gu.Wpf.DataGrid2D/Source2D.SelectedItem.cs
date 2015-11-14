namespace Gu.Wpf.DataGrid2D
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;

    public static partial class Source2D
    {
        private static readonly object Unset = $"{nameof(Source2D)}.Unset";

        public static readonly DependencyProperty SelectedCellItemProperty = DependencyProperty.RegisterAttached(
            "SelectedCellItem",
            typeof(object),
            typeof(Source2D),
            new FrameworkPropertyMetadata(
                Unset,
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                OnSelectedItemChanged));

        private static readonly DependencyProperty CurrentCellProxyProperty = DependencyProperty.RegisterAttached(
            "CurrentCellProxy",
            typeof(object),
            typeof(Source2D),
            new PropertyMetadata(Unset, OnCurrentCellProxyChanged));

        private static void OnCurrentCellProxyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var dataGrid = (DataGrid)d;
            var cellInfo = (DataGridCellInfo)e.NewValue;

            if (!cellInfo.IsValid)
            {
                if (dataGrid.GetSelectedCellItem() != null)
                {
                    dataGrid.SetValue(SelectedCellItemProperty, null);
                }

                return;
            }

            var cell = cellInfo.GetCell();
            if (cell == null)
            {
                return;
            }

            var currentValue = cell.DataContext;

            if (dataGrid.GetSelectedCellItem() != currentValue)
            {
                dataGrid.SetValue(SelectedCellItemProperty, currentValue);
            }
        }

        private static void OnSelectedItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var dataGrid = (DataGrid)d;

            if (e.OldValue == Unset)
            {
                var binding = new Binding
                {
                    Source = dataGrid,
                    Path = BindingHelper.GetPath(DataGrid.CurrentCellProperty),
                    Mode = BindingMode.OneWay,
                    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                };
                BindingOperations.SetBinding(dataGrid, CurrentCellProxyProperty, binding);
            }

            SelectCellFor(dataGrid, e.NewValue);
        }

        public static void SetSelectedCellItem(this DataGrid element, object value)
        {
            element.SetValue(SelectedCellItemProperty, value);
        }

        [AttachedPropertyBrowsableForChildren(IncludeDescendants = false)]
        [AttachedPropertyBrowsableForType(typeof(DataGrid))]
        public static object GetSelectedCellItem(this DataGrid element)
        {
            return element.GetValue(SelectedCellItemProperty);
        }

        private static void SelectCellFor(DataGrid dataGrid, object item)
        {
            var selectedCells = dataGrid.SelectedCells;
            if (selectedCells.Count == 1)
            {
                var cell = selectedCells[0].GetCell();
                if (cell != null && cell.DataContext == item)
                {
                    return;
                }
            }

            dataGrid.UnselectAllCells();
            if (item == null)
            {
                return;
            }

            foreach (var row in dataGrid.Items)
            {
                foreach (var column in dataGrid.Columns)
                {
                    var content = column.GetCellContent(row);
                    if (content != null && content.DataContext == item)
                    {
                        var cell = (DataGridCell)content.Parent;
                        cell.IsSelected = true;
                    }
                }
            }
        }

        private static DataGridCell GetCell(this DataGridCellInfo info)
        {
            if (!info.IsValid)
            {
                return null;
            }

            var cellContent = info.Column.GetCellContent(info.Item);
            if (cellContent != null)
            {
                return cellContent.Parent as DataGridCell;
            }

            return null;
        }
    }
}
