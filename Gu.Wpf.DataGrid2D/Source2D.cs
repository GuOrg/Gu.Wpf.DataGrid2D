namespace Gu.Wpf.DataGrid2D
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;

    public static class Source2D
    {
        public static readonly DependencyProperty HeadersProperty = DependencyProperty.RegisterAttached(
            "Headers",
            typeof(object[]),
            typeof(Source2D),
            new PropertyMetadata(default(object[]), OnHeadersChanged));

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

        public static readonly DependencyProperty HeaderStringFormatProperty = DependencyProperty.RegisterAttached(
           "HeaderStringFormat",
           typeof(String),
           typeof(Source2D),
           new FrameworkPropertyMetadata(default(String)));

        public static readonly DependencyProperty HeaderTemplateProperty = DependencyProperty.RegisterAttached(
           "HeaderTemplate",
           typeof(DataTemplate),
           typeof(Source2D),
           new FrameworkPropertyMetadata(default(DataTemplate)));

        public static readonly DependencyProperty HeaderTemplateSelectorProperty = DependencyProperty.RegisterAttached(
           "HeaderTemplateSelector",
           typeof(DataTemplateSelector),
           typeof(Source2D),
           new FrameworkPropertyMetadata(default(DataTemplateSelector)));

        public static readonly DependencyProperty WidthProperty = DependencyProperty.RegisterAttached(
           "Width",
           typeof(DataGridLength),
           typeof(Source2D),
           new FrameworkPropertyMetadata(DataGridLength.Auto));

        public static readonly DependencyProperty MinWidthProperty = DependencyProperty.RegisterAttached(
           "MinWidth",
           typeof(Double),
           typeof(Source2D),
           new FrameworkPropertyMetadata(20d));

        public static readonly DependencyProperty MaxWidthProperty = DependencyProperty.RegisterAttached(
           "MaxWidth",
           typeof(Double),
           typeof(Source2D),
           new FrameworkPropertyMetadata(double.PositiveInfinity));

        public static readonly DependencyProperty CellTemplateProperty = DependencyProperty.RegisterAttached(
           "CellTemplate",
           typeof(DataTemplate),
           typeof(Source2D),
           new FrameworkPropertyMetadata(default(DataTemplate)));

        public static readonly DependencyProperty CellTemplateSelectorProperty = DependencyProperty.RegisterAttached(
           "CellTemplateSelector",
           typeof(DataTemplateSelector),
           typeof(Source2D),
           new FrameworkPropertyMetadata(default(DataTemplateSelector)));

        public static readonly DependencyProperty CellEditingTemplateProperty = DependencyProperty.RegisterAttached(
           "CellEditingTemplate",
           typeof(DataTemplate),
           typeof(Source2D),
           new FrameworkPropertyMetadata(default(DataTemplate)));

        public static readonly DependencyProperty CellEditingTemplateSelectorProperty = DependencyProperty.RegisterAttached(
           "CellEditingTemplateSelector",
           typeof(DataTemplateSelector),
           typeof(Source2D),
           new FrameworkPropertyMetadata(default(DataTemplateSelector)));

        public static void SetHeaders(this DataGrid element, object[] value)
        {
            element.SetValue(HeadersProperty, value);
        }

        [AttachedPropertyBrowsableForChildren(IncludeDescendants = false)]
        [AttachedPropertyBrowsableForType(typeof(DataGrid))]
        public static object[] GetHeaders(this DataGrid element)
        {
            return (object[])element.GetValue(HeadersProperty);
        }

        public static void SetItemsSource2D(this DataGrid element, object[,] value)
        {
            element.SetValue(ItemsSource2DProperty, value);
        }

        [AttachedPropertyBrowsableForChildren(IncludeDescendants = false)]
        [AttachedPropertyBrowsableForType(typeof(DataGrid))]
        public static object[,] GetItemsSource2D(this DataGrid element)
        {
            return (object[,])element.GetValue(ItemsSource2DProperty);
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

        public static void SetHeaderStringFormat(this DataGrid element, String value)
        {
            element.SetValue(HeaderStringFormatProperty, value);
        }

        [AttachedPropertyBrowsableForChildren(IncludeDescendants = false)]
        [AttachedPropertyBrowsableForType(typeof(DataGrid))]
        public static String GetHeaderStringFormat(this DataGrid element)
        {
            return (String)element.GetValue(HeaderStringFormatProperty);
        }

        public static void SetHeaderTemplate(this DataGrid element, DataTemplate value)
        {
            element.SetValue(HeaderTemplateProperty, value);
        }

        [AttachedPropertyBrowsableForChildren(IncludeDescendants = false)]
        [AttachedPropertyBrowsableForType(typeof(DataGrid))]
        public static DataTemplate GetHeaderTemplate(this DataGrid element)
        {
            return (DataTemplate)element.GetValue(HeaderTemplateProperty);
        }

        public static void SetHeaderTemplateSelector(this DataGrid element, DataTemplateSelector value)
        {
            element.SetValue(HeaderTemplateSelectorProperty, value);
        }

        [AttachedPropertyBrowsableForChildren(IncludeDescendants = false)]
        [AttachedPropertyBrowsableForType(typeof(DataGrid))]
        public static DataTemplateSelector GetHeaderTemplateSelector(this DataGrid element)
        {
            return (DataTemplateSelector)element.GetValue(HeaderTemplateSelectorProperty);
        }

        public static void SetWidth(this DataGrid element, DataGridLength value)
        {
            element.SetValue(WidthProperty, value);
        }

        [AttachedPropertyBrowsableForChildren(IncludeDescendants = false)]
        [AttachedPropertyBrowsableForType(typeof(DataGrid))]
        public static DataGridLength GetWidth(this DataGrid element)
        {
            return (DataGridLength)element.GetValue(WidthProperty);
        }

        public static void SetMinWidth(this DataGrid element, Double value)
        {
            element.SetValue(MinWidthProperty, value);
        }

        [AttachedPropertyBrowsableForChildren(IncludeDescendants = false)]
        [AttachedPropertyBrowsableForType(typeof(DataGrid))]
        public static Double GetMinWidth(this DataGrid element)
        {
            return (Double)element.GetValue(MinWidthProperty);
        }

        public static void SetMaxWidth(this DataGrid element, Double value)
        {
            element.SetValue(MaxWidthProperty, value);
        }

        [AttachedPropertyBrowsableForChildren(IncludeDescendants = false)]
        [AttachedPropertyBrowsableForType(typeof(DataGrid))]
        public static Double GetMaxWidth(this DataGrid element)
        {
            return (Double)element.GetValue(MaxWidthProperty);
        }

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

        public static void SetCellTemplateSelector(this DataGrid element, DataTemplateSelector value)
        {
            element.SetValue(CellTemplateSelectorProperty, value);
        }

        [AttachedPropertyBrowsableForChildren(IncludeDescendants = false)]
        [AttachedPropertyBrowsableForType(typeof(DataGrid))]
        public static DataTemplateSelector GetCellTemplateSelector(this DataGrid element)
        {
            return (DataTemplateSelector)element.GetValue(CellTemplateSelectorProperty);
        }

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

        public static void SetCellEditingTemplateSelector(this DataGrid element, DataTemplateSelector value)
        {
            element.SetValue(CellEditingTemplateSelectorProperty, value);
        }

        [AttachedPropertyBrowsableForChildren(IncludeDescendants = false)]
        [AttachedPropertyBrowsableForType(typeof(DataGrid))]
        public static DataTemplateSelector GetCellEditingTemplateSelector(this DataGrid element)
        {
            return (DataTemplateSelector)element.GetValue(CellEditingTemplateSelectorProperty);
        }

        public static void SetCellEditingTemplate(this DependencyObject element, DataTemplate value)
        {
            element.SetValue(CellEditingTemplateProperty, value);
        }

        [AttachedPropertyBrowsableForChildren(IncludeDescendants = false)]
        [AttachedPropertyBrowsableForType(typeof(DataGrid))]
        public static DataTemplate GetCellEditingTemplate(this DependencyObject element)
        {
            return (DataTemplate)element.GetValue(CellEditingTemplateProperty);
        }

        private static void OnHeadersChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var dataGrid = (DataGrid)d;
            ApplyHeades(dataGrid);
            //if (dataGrid.IsLoaded)
            //{

            //}
            //dataGrid.RemoveHandler(FrameworkElement.LoadedEvent, OnLoadedHandler);
            //dataGrid.AddHandler(FrameworkElement.LoadedEvent, OnLoadedHandler);
        }

        private static void ApplyHeades(DataGrid dataGrid)
        {
            dataGrid.AutoGenerateColumns = false;
            dataGrid.CanUserAddRows = false;
            dataGrid.CanUserDeleteRows = false;
            //dataGrid.CanUserReorderColumns = false;
            //dataGrid.CanUserSortColumns = false;
            //dataGrid.CanUserSortColumns = false;

            var headers = dataGrid.GetHeaders();
            if (headers == null)
            {
                //dataGrid.Columns.Clear();
                return;
            }
            for (int i = 0; i < headers.Length; i++)
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
            UpdateColumns(rows, dataGrid);
        }

        private static void UpdateColumns(IEnumerable rows, DataGrid dataGrid)
        {
            if (rows != null)
            {
                Helpers.Bind(dataGrid, ItemsControl.ItemsSourceProperty, dataGrid, RowsSourceProperty);
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
