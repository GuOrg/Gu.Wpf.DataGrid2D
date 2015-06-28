namespace Gu.Wpf.DataGrid2D
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Data;

    public static partial class Source2D
    {
        public static readonly DependencyProperty ColumnHeadersSourceProperty = DependencyProperty.RegisterAttached(
            "ColumnHeadersSource",
            typeof(IEnumerable),
            typeof(Source2D),
            new PropertyMetadata(null, OnColumnHeadersChanged), OnValidateHeaders);

        public static readonly DependencyProperty ItemsSource2DProperty = DependencyProperty.RegisterAttached(
            "ItemsSource2D",
            typeof(Array),
            typeof(Source2D),
            new PropertyMetadata(default(Array), OnItemsSource2DChanged), OnValidateItemsSource2D);

        public static readonly DependencyProperty RowsSourceProperty = DependencyProperty.RegisterAttached(
            "RowsSource",
            typeof(IEnumerable),
            typeof(Source2D),
            new PropertyMetadata(default(IEnumerable), OnRowsSourceChanged), OnValidateRowsSource);

        public static void SetColumnHeadersSource(this DataGrid element, IEnumerable value)
        {
            element.SetValue(ColumnHeadersSourceProperty, value);
        }

        [AttachedPropertyBrowsableForChildren(IncludeDescendants = false)]
        [AttachedPropertyBrowsableForType(typeof(DataGrid))]
        public static IEnumerable GetColumnHeadersSource(this DataGrid element)
        {
            return (IEnumerable)element.GetValue(ColumnHeadersSourceProperty);
        }

        public static void SetItemsSource2D(this DataGrid element, Array value)
        {
            element.SetValue(ItemsSource2DProperty, value);
        }

        [AttachedPropertyBrowsableForChildren(IncludeDescendants = false)]
        [AttachedPropertyBrowsableForType(typeof(DataGrid))]
        public static Array GetItemsSource2D(this DataGrid element)
        {
            return (Array)element.GetValue(ItemsSource2DProperty);
        }

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

        private static void OnColumnHeadersChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var dataGrid = (DataGrid)d;
            dataGrid.AutoGenerateColumns = false;
            dataGrid.CanUserAddRows = false;
            dataGrid.CanUserDeleteRows = false;

            var headers = dataGrid.GetColumnHeadersSource();
            if (headers == null)
            {
                return;
            }
            var count = headers.Count();
            for (int i = 0; i < count; i++)
            {
                if (dataGrid.Columns.Count > i)
                {
                    var column = (IndexColumn)dataGrid.Columns[i];
                    column.BindHeader(headers, i);
                }
                else
                {
                    var templateColumn = new IndexColumn(dataGrid, headers, i);
                    dataGrid.Columns.Add(templateColumn);
                }
            }
        }

        private static void OnItemsSource2DChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var dataGrid = (DataGrid)d;
            dataGrid.AutoGenerateColumns = false;
            dataGrid.CanUserAddRows = false;
            dataGrid.CanUserDeleteRows = false;
            var array = (Array)e.NewValue;
            if (array != null)
            {
                var rows = new List<IList>();
                for (int i = 0; i < array.GetLength(0); i++)
                {
                    var row = new object[array.GetLength(1)];
                    for (int j = 0; j < array.GetLength(1); j++)
                    {
                        row[j] = array.GetValue(i, j);
                    }
                    rows.Add(row);
                }
                dataGrid.SetRowsSource(rows);
            }
        }

        private static void OnRowsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var dataGrid = (DataGrid)d;
            dataGrid.AutoGenerateColumns = false;
            dataGrid.CanUserAddRows = false;
            dataGrid.CanUserDeleteRows = false;
            // Better to use ItemsSource than adding items manually
            // Adding manually does not create an editable collectionview and probably more things.
            // Should not be a problem since binding both RowSource and ItemsSource makes very little sense.
            BindingOperations.ClearBinding(dataGrid, ItemsControl.ItemsSourceProperty);
            var rows = (IEnumerable)e.NewValue;
            if (rows != null)
            {
                Helpers.Bind(dataGrid, ItemsControl.ItemsSourceProperty, dataGrid, RowsSourceProperty);
            }
            UpdateColumns(rows, dataGrid);
        }

        private static void UpdateColumns(IEnumerable rows, DataGrid dataGrid)
        {
            if (rows != null)
            {
                var firstRow = (IEnumerable)rows.First();
                if (firstRow != null)
                {
                    int i = 0;
                    foreach (var cells in firstRow)
                    {
                        AddColumn(dataGrid, i);
                        i++;
                    }
                }
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

        private static bool OnValidateHeaders(object value)
        {
            if (value == null)
            {
                return true;
            }
            var list = value as IList;
            if (list != null)
            {
                return true;
            }
            var rol = value as IReadOnlyList<object>;
            if (rol != null)
            {
                return true;
            }
            return false;
        }

        private static bool OnValidateItemsSource2D(object value)
        {
            var array = value as Array;
            if (array != null)
            {
                return array.Rank == 2;
            }
            return true;
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
