namespace Gu.Wpf.DataGrid2D
{
    using System;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Exposes attached properties for <see cref="DataGrid"/> cells.
    /// </summary>
    public static class Cell
    {
        /// <summary>
        /// Gets or sets the <see cref="DataTemplate"/> to use when rendering cell contents.
        /// </summary>
        public static readonly DependencyProperty TemplateProperty = DependencyProperty.RegisterAttached(
            "Template",
            typeof(DataTemplate),
            typeof(Cell),
            new PropertyMetadata(
                null,
                OnTemplateChanged));

        /// <summary>
        /// Gets or sets the <see cref="DataTemplateSelector"/> to use when rendering cell contents.
        /// </summary>
        public static readonly DependencyProperty TemplateSelectorProperty = DependencyProperty.RegisterAttached(
            "TemplateSelector",
            typeof(DataTemplateSelector),
            typeof(Cell),
            new PropertyMetadata(
                null,
                OnTemplateSelectorChanged));

        /// <summary>
        /// Gets or sets the <see cref="DataTemplate"/> to use when editing cell contents.
        /// </summary>
        public static readonly DependencyProperty EditingTemplateProperty = DependencyProperty.RegisterAttached(
            "EditingTemplate",
            typeof(DataTemplate),
            typeof(Cell),
            new PropertyMetadata(
                null,
                OnEditingTemplateChanged));

        /// <summary>
        /// Gets or sets the <see cref="DataTemplateSelector"/> to use when editing cell contents.
        /// </summary>
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

        /// <summary>
        /// Helper for setting Template property on a DataGrid.
        /// </summary>
        /// <param name="element">DataGrid to set Template property on.</param>
        /// <param name="value">Template property value.</param>
        public static void SetTemplate(this DataGrid element, DataTemplate value)
        {
            element.SetValue(TemplateProperty, value);
        }

        /// <summary>
        /// Helper for reading Template property from a DataGrid.
        /// </summary>
        /// <param name="element">DataGrid to read Template property from.</param>
        /// <returns>Template property value.</returns>
        [AttachedPropertyBrowsableForChildren(IncludeDescendants = false)]
        [AttachedPropertyBrowsableForType(typeof(DataGrid))]
        public static DataTemplate GetTemplate(this DataGrid element)
        {
            return (DataTemplate)element.GetValue(TemplateProperty);
        }

        /// <summary>
        /// Helper for setting TemplateSelector property on a DataGrid.
        /// </summary>
        /// <param name="element">DataGrid to set TemplateSelector property on.</param>
        /// <param name="value">TemplateSelector property value.</param>
        public static void SetTemplateSelector(DataGrid element, DataTemplateSelector value)
        {
            element.SetValue(TemplateSelectorProperty, value);
        }

        /// <summary>
        /// Helper for reading TemplateSelector property from a DataGrid.
        /// </summary>
        /// <param name="element">DataGrid to read TemplateSelector property from.</param>
        /// <returns>TemplateSelector property value.</returns>
        [AttachedPropertyBrowsableForChildren(IncludeDescendants = false)]
        [AttachedPropertyBrowsableForType(typeof(DataGrid))]
        public static DataTemplateSelector GetTemplateSelector(this DataGrid element)
        {
            return (DataTemplateSelector)element.GetValue(TemplateSelectorProperty);
        }

        /// <summary>
        /// Helper for setting EditingTemplate property on a DataGrid.
        /// </summary>
        /// <param name="element">DataGrid to set EditingTemplate property on.</param>
        /// <param name="value">EditingTemplate property value.</param>
        public static void SetEditingTemplate(this DataGrid element, DataTemplate value)
        {
            element.SetValue(EditingTemplateProperty, value);
        }

        /// <summary>
        /// Helper for reading EditingTemplate property from a DataGrid.
        /// </summary>
        /// <param name="element">DataGrid to read EditingTemplate property from.</param>
        /// <returns>EditingTemplate property value.</returns>
        [AttachedPropertyBrowsableForChildren(IncludeDescendants = false)]
        [AttachedPropertyBrowsableForType(typeof(DataGrid))]
        public static DataTemplate GetEditingTemplate(this DataGrid element)
        {
            return (DataTemplate)element.GetValue(EditingTemplateProperty);
        }

        /// <summary>
        /// Helper for setting EditingTemplateSelector property on a DependencyObject.
        /// </summary>
        /// <param name="element">DependencyObject to set EditingTemplateSelector property on.</param>
        /// <param name="value">EditingTemplateSelector property value.</param>
        public static void SetEditingTemplateSelector(DependencyObject element, DataTemplateSelector value)
        {
            element.SetValue(EditingTemplateSelectorProperty, value);
        }

        /// <summary>
        /// Helper for reading EditingTemplateSelector property from a DependencyObject.
        /// </summary>
        /// <param name="element">DependencyObject to read EditingTemplateSelector property from.</param>
        /// <returns>EditingTemplateSelector property value.</returns>
        [AttachedPropertyBrowsableForType(typeof(DataGrid))]
        public static DataTemplateSelector GetEditingTemplateSelector(this DependencyObject element)
        {
            return (DataTemplateSelector)element.GetValue(EditingTemplateSelectorProperty);
        }

        private static void OnTemplateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var dataGrid = (DataGrid)d;
            SetCellTemplateProperty(dataGrid, e, editingTemplate: false);
            ListenToColumnAutoGeneration(dataGrid);
        }

        private static void OnTemplateSelectorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var dataGrid = (DataGrid)d;
            foreach (var column in dataGrid.Columns.OfType<CellTemplateColumn>())
            {
                column.SetCurrentValue(DataGridTemplateColumn.CellTemplateSelectorProperty, e.NewValue);
            }

            ListenToColumnAutoGeneration(dataGrid);
        }

        private static void OnEditingTemplateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var dataGrid = (DataGrid)d;
            SetCellTemplateProperty(dataGrid, e, editingTemplate: true);
            ListenToColumnAutoGeneration(dataGrid);
        }

        private static void OnEditingTemplateSelectorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var dataGrid = (DataGrid)d;
            foreach (var column in dataGrid.Columns.OfType<CellTemplateColumn>())
            {
                column.SetCurrentValue(DataGridTemplateColumn.CellEditingTemplateSelectorProperty, e.NewValue);
            }

            ListenToColumnAutoGeneration(dataGrid);
        }

        private static void ListenToColumnAutoGeneration(DataGrid dataGrid)
        {
            if (dataGrid.GetValue(ListenerProperty) == null)
            {
                // ReSharper disable once HeapView.ObjectAllocation.Evident
#pragma warning disable IDISP004 // Don't ignore return value of type IDisposable.
                dataGrid.SetCurrentValue(ListenerProperty, new AutogenerateColumnListener(dataGrid));
#pragma warning restore IDISP004 // Don't ignore return value of type IDisposable.
            }
        }

        private static void SetCellTemplateProperty(DataGrid dataGrid, DependencyPropertyChangedEventArgs e, bool editingTemplate)
        {
            if (e.OldValue != null && e.NewValue == null)
            {
                for (int i = 0; i < dataGrid.Columns.Count; ++i)
                {
                    if (dataGrid.Columns[i] is CellTemplateColumn col)
                    {
                        var temp = editingTemplate ?
                                   col.GetValue(DataGridTemplateColumn.CellTemplateProperty) :
                                   col.GetValue(DataGridTemplateColumn.CellEditingTemplateProperty);
                        if (temp == null)
                        {
                            var tcol = new DataGridTextColumn { Binding = col.Binding };
                            dataGrid.Columns[i] = tcol;
                        }
                        else
                        {
                            SetCellTemplateProperty(dataGrid, (DataTemplate)e.NewValue, editingTemplate);
                        }
                    }
                }
            }
            else if (e.OldValue == null && e.NewValue != null)
            {
                for (int i = 0; i < dataGrid.Columns.Count; ++i)
                {
                    if (dataGrid.Columns[i] is DataGridTextColumn tcol)
                    {
                        var col = new CellTemplateColumn { Binding = tcol.Binding };
                        if (!editingTemplate)
                        {
                            col.CellTemplate = (DataTemplate)e.NewValue;
                        }
                        else
                        {
                            col.CellEditingTemplate = (DataTemplate)e.NewValue;
                        }

                        dataGrid.Columns[i] = col;
                    }
                }
            }
            else
            {
                SetCellTemplateProperty(dataGrid, (DataTemplate)e.NewValue, editingTemplate);
            }
        }

        private static void SetCellTemplateProperty(DataGrid dataGrid, DataTemplate t, bool editingTemplate)
        {
            foreach (var column in dataGrid.Columns.OfType<CellTemplateColumn>())
            {
                if (!editingTemplate)
                {
                    column.SetCurrentValue(DataGridTemplateColumn.CellTemplateProperty, t);
                }
                else
                {
                    column.SetCurrentValue(DataGridTemplateColumn.CellEditingTemplateProperty, t);
                }
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
                    CellEditingTemplate = ((DataGrid)sender).GetEditingTemplate(),
                    CellTemplateSelector = ((DataGrid)sender).GetTemplateSelector(),
                    CellEditingTemplateSelector = ((DataGrid)sender).GetEditingTemplateSelector()
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
