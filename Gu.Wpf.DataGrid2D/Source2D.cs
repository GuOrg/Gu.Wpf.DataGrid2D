namespace Gu.Wpf.DataGrid2D
{
    using System;
    using System.Collections;
    using System.Windows;
    using System.Windows.Controls;

    public static class Source2D
    {
        public static readonly DependencyProperty HeadersProperty = DependencyProperty.RegisterAttached(
            "Headers",
            typeof(object[]),
            typeof(Source2D),
            new PropertyMetadata(default(object[]), OnHeadersChanged));

        public static readonly DependencyProperty EvenSpacingProperty = DependencyProperty.RegisterAttached(
            "EvenSpacing",
            typeof(bool),
            typeof(Source2D),
            new PropertyMetadata(default(bool)));

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

        private static readonly SizeChangedEventHandler OnSizeChangedHandler = OnSizeChanged;

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

        public static void SetEvenSpacing(this DataGrid element, bool value)
        {
            element.SetValue(EvenSpacingProperty, value);
        }

        [AttachedPropertyBrowsableForChildren(IncludeDescendants = false)]
        [AttachedPropertyBrowsableForType(typeof(DataGrid))]
        public static bool GetEvenSpacing(this DataGrid element)
        {
            return (bool)element.GetValue(EvenSpacingProperty);
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
            if (dataGrid.GetEvenSpacing())
            {
                dataGrid.CanUserResizeColumns = false;
                dataGrid.UpdateHandler(FrameworkElement.SizeChangedEvent, OnSizeChangedHandler);
            }
            //dataGrid.CanUserReorderColumns = false;
            //dataGrid.CanUserSortColumns = false;
            //dataGrid.CanUserSortColumns = false;

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


        private static void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (!e.WidthChanged)
            {
                return;
            }
            var dataGrid = (DataGrid)sender;
            double totalWidth = dataGrid.ActualWidth - dataGrid.CellsPanelHorizontalOffset - dataGrid.RowHeaderWidth;
            var columnWidth = totalWidth / dataGrid.Columns.Count;
            foreach (var column in dataGrid.Columns)
            {
                column.Width = columnWidth;
            }
        }
    }
}
