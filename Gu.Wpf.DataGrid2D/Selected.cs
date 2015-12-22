namespace Gu.Wpf.DataGrid2D
{
    using System.Linq;
    using System.Threading;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;

    public static class Selected
    {
        private static readonly string Unset = "Unset";
        public static readonly DependencyProperty CellItemProperty = DependencyProperty.RegisterAttached(
            "CellItem",
            typeof(object),
            typeof(Selected),
            new FrameworkPropertyMetadata(
                Unset,
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                OnCellItemChanged,
                OnCellItemCoerce));

        public static readonly DependencyProperty IndexProperty = DependencyProperty.RegisterAttached(
            "Index",
            typeof(RowColumnIndex?),
            typeof(Selected),
            new FrameworkPropertyMetadata(
                RowColumnIndex.Unset,
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                OnIndexChanged,
                OnIndexCoerce));

        private static readonly DependencyProperty CurrentCellProxyProperty = DependencyProperty.RegisterAttached(
            "CurrentCellProxy",
            typeof(object),
            typeof(Selected),
            new PropertyMetadata(
                Unset,
                OnCurrentCellProxyChanged));

        ////private static readonly DependencyProperty IsUpdatingProperty = DependencyProperty.RegisterAttached(
        ////    "IsUpdating",
        ////    typeof(bool),
        ////    typeof(Selected),
        ////    new PropertyMetadata(
        ////        false,
        ////        OnCurrentCellProxyChanged));

        public static void SetCellItem(this DataGrid element, object value)
        {
            element.SetValue(CellItemProperty, value);
        }

        [AttachedPropertyBrowsableForChildren(IncludeDescendants = false)]
        [AttachedPropertyBrowsableForType(typeof(DataGrid))]
        public static object GetCellItem(this DataGrid element)
        {
            return element.GetValue(CellItemProperty);
        }

        public static void SetIndex(this DataGrid element, RowColumnIndex? value)
        {
            element.SetValue(IndexProperty, value);
        }

        [AttachedPropertyBrowsableForChildren(IncludeDescendants = false)]
        [AttachedPropertyBrowsableForType(typeof(DataGrid))]
        public static RowColumnIndex? GetIndex(this DataGrid element)
        {
            return (RowColumnIndex?)element.GetValue(IndexProperty);
        }

        private static void OnCurrentCellProxyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var dataGrid = (DataGrid)d;
            ////if (Equals(dataGrid.GetValue(IsUpdatingProperty), true))
            ////{
            ////    return;
            ////}

            var cellInfo = (DataGridCellInfo)e.NewValue;

            if (!cellInfo.IsValid)
            {
                if (dataGrid.GetCellItem() != null)
                {
                    dataGrid.SetValue(CellItemProperty, null);
                    dataGrid.SetValue(IndexProperty, dataGrid.SelectedIndex());
                }

                return;
            }

            var cell = cellInfo.GetCell();
            if (cell == null)
            {
                dataGrid.SetValue(CellItemProperty, null);
                dataGrid.SetValue(IndexProperty, null);
                return;
            }

            var containerFromItem = dataGrid.ItemContainerGenerator.ContainerFromItem(dataGrid.CurrentItem);
            var rowIndex = dataGrid.ItemContainerGenerator.IndexFromContainer(containerFromItem);
            var colIndex = dataGrid.CurrentColumn.DisplayIndex;
            var index = new RowColumnIndex(rowIndex, colIndex);
            dataGrid.SetValue(IndexProperty, index);
        }

        private static void OnCellItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var dataGrid = (DataGrid)d;
            if (e.NewValue == null)
            {
                dataGrid.UnselectAllCells();
                return;
            }

            ////dataGrid.SetValue(IsUpdatingProperty, true);
            var selectedCells = dataGrid.SelectedCells;
            if (selectedCells.Count == 1)
            {
                var cell = selectedCells[0].GetCell();
                if (cell != null && cell.DataContext == e.NewValue)
                {
                    return;
                }
            }

            dataGrid.UnselectAllCells();

            foreach (var row in dataGrid.Items)
            {
                foreach (var column in dataGrid.Columns)
                {
                    var content = column.GetCellContent(row);
                    if (content != null && content.DataContext == e.NewValue)
                    {
                        var cell = (DataGridCell)content.Parent;
                        cell.IsSelected = true;
                    }
                }
            }

            ////dataGrid.SetValue(IsUpdatingProperty, false);
        }

        private static object OnCellItemCoerce(DependencyObject d, object basevalue)
        {
            BindCurrentCell((DataGrid)d);
            if (Equals(basevalue, Unset))
            {
                return null;
            }

            return basevalue;
        }

        private static void OnIndexChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var dataGrid = (DataGrid)d;
            if (dataGrid.SelectedIndex() == (RowColumnIndex?)dataGrid.GetValue(IndexProperty))
            {
                return;
            }

            var rowColumnIndex = (RowColumnIndex?)e.NewValue;
            if (rowColumnIndex == null)
            {
                dataGrid.UnselectAllCells();
                return;
            }

            var dataGridColumn = dataGrid.Columns[rowColumnIndex.Value.Column];
            var row = (DataGridRow)dataGrid.ItemContainerGenerator.ContainerFromIndex(rowColumnIndex.Value.Row);
            var content = dataGridColumn.GetCellContent(row);
            var cell = content.Ancestors()
                                        .OfType<DataGridCell>()
                                        .FirstOrDefault();
            if (cell != null)
            {
                cell.IsSelected = true;
            }
        }

        private static object OnIndexCoerce(DependencyObject d, object basevalue)
        {
            BindCurrentCell((DataGrid)d);
            if (Equals(basevalue, RowColumnIndex.Unset))
            {
                return null;
            }

            return basevalue;
        }

        private static DataGridCell GetCell(this DataGridCellInfo info)
        {
            if (!info.IsValid)
            {
                return null;
            }

            var cellContent = info.Column.GetCellContent(info.Item);
            return cellContent?.Parent as DataGridCell;
        }

        private static void BindCurrentCell(DataGrid dataGrid)
        {
            if (BindingOperations.GetBinding(dataGrid, CurrentCellProxyProperty) == null)
            {
                dataGrid.Bind(CurrentCellProxyProperty)
                        .TwoWayTo(dataGrid, DataGrid.CurrentCellProperty);
            }
        }

        private static RowColumnIndex? SelectedIndex(this DataGrid dataGrid)
        {
            var selectedCells = dataGrid.SelectedCells;
            if (selectedCells.Count != 1)
            {
                return null;
            }

            var cell = selectedCells[0].GetCell();
            if (cell != null)
            {
                return new RowColumnIndex(dataGrid.SelectedIndex, cell.Column.DisplayIndex);
            }

            return null;
        }
    }
}
