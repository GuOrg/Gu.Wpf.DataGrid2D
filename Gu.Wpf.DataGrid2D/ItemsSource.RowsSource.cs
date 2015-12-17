namespace Gu.Wpf.DataGrid2D
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;

   public static partial class ItemsSource
    {
        public static readonly DependencyProperty RowsSourceProperty = DependencyProperty.RegisterAttached(
            "RowsSource",
            typeof(IEnumerable),
            typeof(ItemsSource),
            new PropertyMetadata(default(IEnumerable), OnRowsSourceChanged),
            OnValidateRowsSource);

        public static void SetRowsSource(this DataGrid element, IEnumerable value)
        {
            element.SetValue(RowsSourceProperty, value);
        }

        [AttachedPropertyBrowsableForChildren(IncludeDescendants = false)]
        [AttachedPropertyBrowsableForType(typeof(DataGrid))]
        public static IEnumerable GetRowsSource(this DataGrid element)
        {
            return (IEnumerable)element.GetValue(RowsSourceProperty);
        }

        private static void OnRowsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var dataGrid = (DataGrid)d;
            dataGrid.AutoGenerateColumns = true;

            dataGrid.CanUserAddRows = false;
            dataGrid.CanUserDeleteRows = false;

            // Better to use ItemsSource than adding items manually
            // Adding manually does not create an editable collectionview and probably more things.
            // Should not be a problem since binding both RowSource and ItemsSource makes very little sense.
            BindingOperations.ClearBinding(dataGrid, ItemsControl.ItemsSourceProperty);
            var rows = (IEnumerable)e.NewValue;
            if (rows != null)
            {
                dataGrid.Bind(ItemsControl.ItemsSourceProperty)
                        .OneWayTo(dataGrid, RowsSourceProperty);
            }

            UpdateColumns(rows, dataGrid);
        }

        private static void UpdateColumns(IEnumerable rows, DataGrid dataGrid)
        {
            var firstRow = (IEnumerable)rows?.First();
            if (firstRow == null)
            {
                return;
            }

            int i = 0;
            foreach (var _ in firstRow)
            {
                AddColumn(dataGrid, i);
                i++;
            }
        }

        private static void AddColumn(DataGrid dataGrid, int i)
        {
            if (dataGrid.Columns.Count > i)
            {
                var column = dataGrid.Columns[i] as IndexColumn;
                if (column == null)
                {
                    throw new InvalidOperationException();
                }
            }
            else
            {
                var templateColumn = new IndexColumn(dataGrid, i);
                dataGrid.Columns.Add(templateColumn);
            }
        }

        private static bool OnValidateRowsSource(object value)
        {
            if (value == null)
            {
                return true;
            }

            var rows = value as IEnumerable;
            if (rows == null)
            {
                return false;
            }

            int columnCount = -1;
            foreach (var row in rows)
            {
                var cells = row as IEnumerable;
                if (cells == null)
                {
                    return false;
                }

                if (columnCount == -1)
                {
                    columnCount = cells.Count();
                }
                else
                {
                    if (columnCount != cells.Count())
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}
