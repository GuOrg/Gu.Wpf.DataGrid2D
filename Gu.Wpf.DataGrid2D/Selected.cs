namespace Gu.Wpf.DataGrid2D
{
    using System.ComponentModel;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;

    public static class Selected
    {
        public static readonly DependencyProperty CellItemProperty = DependencyProperty.RegisterAttached(
            "CellItem",
            typeof(object),
            typeof(Selected),
            new FrameworkPropertyMetadata(
                null,
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                OnCellItemChanged,
                CoerceCellItem));

        public static readonly DependencyProperty IndexProperty = DependencyProperty.RegisterAttached(
            "Index",
            typeof(RowColumnIndex?),
            typeof(Selected),
            new FrameworkPropertyMetadata(
                null,
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                OnIndexChanged,
                CoerceIndex));

        private static readonly DependencyProperty IsSubscribingChangesProperty = DependencyProperty.RegisterAttached(
            "IsSubscribingChanges",
            typeof(bool),
            typeof(Selected),
            new PropertyMetadata(BooleanBoxes.False));

        private static readonly DependencyProperty IsUpdatingProperty = DependencyProperty.RegisterAttached(
            "IsUpdating",
            typeof(bool),
            typeof(Selected),
            new PropertyMetadata(BooleanBoxes.False));

        private static readonly DependencyProperty PropertyDescriptorProperty = DependencyProperty.RegisterAttached(
            "PropertyDescriptor",
            typeof(PropertyDescriptor),
            typeof(Selected),
            new PropertyMetadata(null));

        private static readonly RoutedEventHandler SelectedCellsChangedHandler = OnSelectedCellsChanged;

        /// <summary>Helper for setting <see cref="CellItemProperty"/> on <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="DataGrid"/> to set <see cref="CellItemProperty"/> on.</param>
        /// <param name="value">CellItem property value.</param>
        public static void SetCellItem(this DataGrid element, object? value)
        {
            if (element is null)
            {
                throw new System.ArgumentNullException(nameof(element));
            }

            element.SetValue(CellItemProperty, value);
        }

        /// <summary>Helper for getting <see cref="CellItemProperty"/> from <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="DataGrid"/> to read <see cref="CellItemProperty"/> from.</param>
        /// <returns>CellItem property value.</returns>
        [AttachedPropertyBrowsableForChildren(IncludeDescendants = false)]
        [AttachedPropertyBrowsableForType(typeof(DataGrid))]
        public static object? GetCellItem(this DataGrid element)
        {
            if (element is null)
            {
                throw new System.ArgumentNullException(nameof(element));
            }

            return element.GetValue(CellItemProperty);
        }

        /// <summary>Helper for setting <see cref="IndexProperty"/> on <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="DataGrid"/> to set <see cref="IndexProperty"/> on.</param>
        /// <param name="value">Index property value.</param>
        public static void SetIndex(this DataGrid element, RowColumnIndex? value)
        {
            if (element is null)
            {
                throw new System.ArgumentNullException(nameof(element));
            }

            element.SetValue(IndexProperty, value);
        }

        /// <summary>Helper for getting <see cref="IndexProperty"/> from <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="DataGrid"/> to read <see cref="IndexProperty"/> from.</param>
        /// <returns>Index property value.</returns>
        [AttachedPropertyBrowsableForChildren(IncludeDescendants = false)]
        [AttachedPropertyBrowsableForType(typeof(DataGrid))]
        public static RowColumnIndex? GetIndex(this DataGrid element)
        {
            if (element is null)
            {
                throw new System.ArgumentNullException(nameof(element));
            }

            return (RowColumnIndex?)element.GetValue(IndexProperty);
        }

        private static void OnSelectedCellsChanged(object sender, RoutedEventArgs routedEventArgs)
        {
            var dataGrid = (DataGrid)sender;
            if (Equals(dataGrid.GetValue(IsUpdatingProperty), BooleanBoxes.True))
            {
                return;
            }

            dataGrid.SetCurrentValue(IsUpdatingProperty, BooleanBoxes.True);
            try
            {
                if (dataGrid.SelectedCells.Count != 1)
                {
                    dataGrid.SetCurrentValue(IndexProperty, null);
                    return;
                }

                var index = dataGrid.SelectedRowColumnIndex();
                dataGrid.SetCurrentValue(IndexProperty, index);
                UpdateSelectedCellItemFromView(dataGrid);
            }
            finally
            {
                dataGrid.SetCurrentValue(IsUpdatingProperty, BooleanBoxes.False);
            }
        }

        private static void OnCellItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (Equals(d.GetValue(IsUpdatingProperty), BooleanBoxes.True))
            {
                return;
            }

            d.SetCurrentValue(IsUpdatingProperty, BooleanBoxes.True);
            try
            {
                var dataGrid = (DataGrid)d;
                dataGrid.UnselectAllCells();
                if (e.NewValue is null)
                {
                    dataGrid.SetIndex(null);
                    return;
                }

                for (var r = 0; r < dataGrid.Items.Count; r++)
                {
                    for (var c = 0; c < dataGrid.Columns.Count; c++)
                    {
                        var column = dataGrid.Columns[c];
                        var cellItem = GetCellItem(column, dataGrid.Items[r]);
                        if (Equals(cellItem, e.NewValue))
                        {
                            var index = new RowColumnIndex(r, c);
                            dataGrid.SetIndex(index);
                            var cell = dataGrid.GetCell(index);
                            if (cell != null)
                            {
                                cell.SetCurrentValue(DataGridCell.IsSelectedProperty, true);
                            }

                            return;
                        }
                    }
                }

                dataGrid.SetIndex(null);
            }
            finally
            {
                d.SetCurrentValue(IsUpdatingProperty, BooleanBoxes.False);
            }
        }

        private static object CoerceCellItem(DependencyObject d, object basevalue)
        {
            if (Equals(d.GetValue(IsSubscribingChangesProperty), BooleanBoxes.False))
            {
                SubscribeSelectionChanges((DataGrid)d);
            }

            return basevalue;
        }

        private static void OnIndexChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (Equals(d.GetValue(IsUpdatingProperty), BooleanBoxes.True))
            {
                return;
            }

            d.SetCurrentValue(IsUpdatingProperty, BooleanBoxes.True);
            try
            {
                var dataGrid = (DataGrid)d;
                dataGrid.UnselectAllCells();
                var rowColumnIndex = (RowColumnIndex?)e.NewValue;
                if (rowColumnIndex is null)
                {
                    return;
                }

                var cell = dataGrid.GetCell(rowColumnIndex.Value);
                cell?.SetCurrentValue(DataGridCell.IsSelectedProperty, true);

                UpdateSelectedCellItemFromView(dataGrid);
            }
            finally
            {
                d.SetCurrentValue(IsUpdatingProperty, BooleanBoxes.False);
            }
        }

        private static object CoerceIndex(DependencyObject d, object basevalue)
        {
            if (Equals(d.GetValue(IsSubscribingChangesProperty), BooleanBoxes.False))
            {
                SubscribeSelectionChanges((DataGrid)d);
            }

            return basevalue;
        }

        private static void SubscribeSelectionChanges(DataGrid dataGrid)
        {
            dataGrid.UpdateHandler(DataGridCell.SelectedEvent, SelectedCellsChangedHandler, handledEventsToo: true);
            dataGrid.UpdateHandler(DataGridCell.UnselectedEvent, SelectedCellsChangedHandler, handledEventsToo: true);
            dataGrid.SetCurrentValue(IsSubscribingChangesProperty, BooleanBoxes.True);
        }

        private static RowColumnIndex? SelectedRowColumnIndex(this DataGrid dataGrid)
        {
            var item = dataGrid.CurrentItem;
            if (item is null)
            {
                return null;
            }

            var row = dataGrid.ItemContainerGenerator.ContainerFromItem(item);
            if (row is null || dataGrid.CurrentColumn is null)
            {
                return null;
            }

            var rowIndex = dataGrid.ItemContainerGenerator.IndexFromContainer(row);
            return new RowColumnIndex(rowIndex, dataGrid.CurrentColumn.DisplayIndex);
        }

        private static DataGridCell? GetCell(this DataGrid dataGrid, RowColumnIndex index)
        {
            if (index.Column < 0 || index.Column >= dataGrid.Columns.Count)
            {
                return null;
            }

            if (index.Row < 0 || index.Row >= dataGrid.ItemContainerGenerator.Items.Count)
            {
                return null;
            }

            var dataGridColumn = dataGrid.Columns[index.Column];
            var row = (DataGridRow)dataGrid.ItemContainerGenerator.ContainerFromIndex(index.Row);
            if (row != null)
            {
                var content = dataGridColumn.GetCellContent(row);
                var cell = content.Ancestors()
                                  .OfType<DataGridCell>()
                                  .FirstOrDefault();
                return cell;
            }

            return null;
        }

        private static void UpdateSelectedCellItemFromView(DataGrid dataGrid)
        {
            var index = dataGrid.GetIndex();
            if (index is null || index.Value.Column < 0 || index.Value.Column >= dataGrid.Columns.Count)
            {
                dataGrid.SetCurrentValue(CellItemProperty, null);
                return;
            }

            var column = dataGrid.Columns.ElementAtOrDefault<DataGridColumn>(index.Value.Column);
            var item = dataGrid.Items.ElementAtOrDefault(index.Value.Row);

            var cellItem = GetCellItem(column, item);
            dataGrid.SetCurrentValue(CellItemProperty, cellItem);
        }

        private static object? GetCellItem(this DataGridColumn column, object item)
        {
            if (column is null || item is null)
            {
                return null;
            }

            var descriptor = (PropertyDescriptor)column.GetValue(PropertyDescriptorProperty);
            if (descriptor != null)
            {
                return descriptor.GetValue(item);
            }

            var binding = (column as DataGridBoundColumn)?.Binding as Binding ??
                          (column as CellTemplateColumn)?.Binding as Binding;
            if (binding is null)
            {
                return null;
            }

            descriptor = TypeDescriptor.GetProperties(item)
                                       .OfType<PropertyDescriptor>()
                                       .SingleOrDefault(x => x.Name == binding.Path.Path);
            column.SetCurrentValue(PropertyDescriptorProperty, descriptor);

            return descriptor?.GetValue(item);
        }
    }
}
