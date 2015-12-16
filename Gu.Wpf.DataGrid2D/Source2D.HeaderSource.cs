namespace Gu.Wpf.DataGrid2D
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Controls;

    public static partial class Source2D
    {
        public static readonly DependencyProperty ColumnHeadersSourceProperty = DependencyProperty.RegisterAttached(
            "ColumnHeadersSource",
            typeof (IEnumerable),
            typeof (Source2D),
            new PropertyMetadata(null, OnColumnHeadersChanged),
            OnValidateHeaders);


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
    }
}
