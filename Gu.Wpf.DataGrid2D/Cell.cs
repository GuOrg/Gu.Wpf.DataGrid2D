namespace Gu.Wpf.DataGrid2D
{
    using System;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;

    public static class Cell
    {
        public static readonly DependencyProperty TemplateProperty = DependencyProperty.RegisterAttached(
            "Template",
            typeof(DataTemplate),
            typeof(Cell),
            new PropertyMetadata(
                null,
                OnTemplateChanged));

        public static readonly DependencyProperty TemplateSelectorProperty = DependencyProperty.RegisterAttached(
            "TemplateSelector",
            typeof(DataTemplateSelector),
            typeof(Cell),
            new PropertyMetadata(
                null,
                OnTemplateSelectorChanged));

        public static readonly DependencyProperty EditingTemplateProperty = DependencyProperty.RegisterAttached(
            "EditingTemplate",
            typeof(DataTemplate),
            typeof(Cell),
            new PropertyMetadata(
                null,
                OnEditingTemplateChanged));

        public static readonly DependencyProperty EditingTemplateSelectorProperty = DependencyProperty.RegisterAttached(
            "EditingTemplateSelector",
            typeof(DataTemplateSelector),
            typeof(Cell),
            new PropertyMetadata(
                null,
                OnEditingTemplateSelectorChanged));

        private static readonly DependencyProperty ListenerProperty = DependencyProperty.RegisterAttached(
            "Listener",
            typeof(AutogenerateColumnListener),
            typeof(Cell));

        public static void SetTemplate(this DataGrid element, DataTemplate value)
        {
            element.SetValue(TemplateProperty, value);
        }

        [AttachedPropertyBrowsableForChildren(IncludeDescendants = false)]
        [AttachedPropertyBrowsableForType(typeof(DataGrid))]
        public static DataTemplate GetTemplate(this DataGrid element)
        {
            return (DataTemplate)element.GetValue(TemplateProperty);
        }

        public static void SetTemplateSelector(DataGrid element, DataTemplateSelector value)
        {
            element.SetValue(TemplateSelectorProperty, value);
        }

        [AttachedPropertyBrowsableForChildren(IncludeDescendants = false)]
        [AttachedPropertyBrowsableForType(typeof(DataGrid))]
        public static DataTemplateSelector GetTemplateSelector(DataGrid element)
        {
            return (DataTemplateSelector)element.GetValue(TemplateSelectorProperty);
        }

        public static void SetEditingTemplate(this DataGrid element, DataTemplate value)
        {
            element.SetValue(EditingTemplateProperty, value);
        }

        [AttachedPropertyBrowsableForChildren(IncludeDescendants = false)]
        [AttachedPropertyBrowsableForType(typeof(DataGrid))]
        public static DataTemplate GetEditingTemplate(this DataGrid element)
        {
            return (DataTemplate)element.GetValue(EditingTemplateProperty);
        }

        public static void SetEditingTemplateSelector(DependencyObject element, DataTemplateSelector value)
        {
            element.SetValue(EditingTemplateSelectorProperty, value);
        }

        public static DataTemplateSelector GetEditingTemplateSelector(DependencyObject element)
        {
            return (DataTemplateSelector)element.GetValue(EditingTemplateSelectorProperty);
        }

        private static void OnTemplateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var dataGrid = (DataGrid)d;
            if (e.OldValue != null && e.NewValue == null)
            {
                for (int i = 0; i < dataGrid.Columns.Count; ++i)
                {
                    CellTemplateColumn col = dataGrid.Columns[i] as CellTemplateColumn;
                    DataGridTextColumn tcol = new DataGridTextColumn();
                    tcol.Binding = col.Binding;
                    dataGrid.Columns[i] = tcol;
                }
            }
            else if (e.OldValue == null && e.NewValue != null)
            {
                for (int i = 0; i < dataGrid.Columns.Count; ++i)
                {
                    DataGridTextColumn tcol = dataGrid.Columns[i] as DataGridTextColumn;
                    CellTemplateColumn col = new CellTemplateColumn();
                    col.Binding = tcol.Binding;
                    col.CellTemplate = (DataTemplate)e.NewValue;
                    dataGrid.Columns[i] = col;
                }
            }
            else
            {
                foreach (var column in dataGrid.Columns.OfType<CellTemplateColumn>())
                {
                    column.SetCurrentValue(DataGridTemplateColumn.CellTemplateProperty, (DataTemplate)e.NewValue);
                }
            }

            ListenToColumnAutoGeneration(dataGrid);
        }

        private static void OnTemplateSelectorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var dataGrid = (DataGrid)d;
            foreach (var column in dataGrid.Columns.OfType<CellTemplateColumn>())
            {
                column.SetCurrentValue(DataGridTemplateColumn.CellTemplateSelectorProperty, (DataTemplateSelector)e.NewValue);
            }

            ListenToColumnAutoGeneration(dataGrid);
        }

        private static void OnEditingTemplateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var dataGrid = (DataGrid)d;
            foreach (var column in dataGrid.Columns.OfType<CellTemplateColumn>())
            {
                column.SetCurrentValue(DataGridTemplateColumn.CellEditingTemplateProperty, (DataTemplate)e.NewValue);
            }

            ListenToColumnAutoGeneration(dataGrid);
        }

        private static void OnEditingTemplateSelectorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var dataGrid = (DataGrid)d;
            foreach (var column in dataGrid.Columns.OfType<CellTemplateColumn>())
            {
                column.SetCurrentValue(DataGridTemplateColumn.CellEditingTemplateSelectorProperty, (DataTemplateSelector)e.NewValue);
            }

            ListenToColumnAutoGeneration(dataGrid);
        }

        private static void ListenToColumnAutoGeneration(DataGrid dataGrid)
        {
            if (dataGrid.GetValue(ListenerProperty) == null)
            {
                dataGrid.SetCurrentValue(ListenerProperty, new AutogenerateColumnListener(dataGrid));
            }
        }

        private sealed class AutogenerateColumnListener : IDisposable
        {
            private readonly DataGrid dataGrid;

            public AutogenerateColumnListener(DataGrid dataGrid)
            {
                this.dataGrid = dataGrid;
                dataGrid.AutoGeneratingColumn += OnDataGridAutoGeneratingColumn;
            }

            public void Dispose()
            {
                this.dataGrid.AutoGeneratingColumn -= OnDataGridAutoGeneratingColumn;
            }

            private static void OnDataGridAutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
            {
                var col = new CellTemplateColumn
                {
                    CellTemplate = ((DataGrid)sender).GetTemplate(),
                    CellEditingTemplate = ((DataGrid)sender).GetEditingTemplate()
                };

                DataGridTextColumn tc = e.Column as DataGridTextColumn;
                if (tc?.Binding != null)
                {
                    col.Binding = tc.Binding;
                    e.Column = col;
                }
            }
        }
    }
}
