namespace Gu.Wpf.DataGrid2D;

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
        typeof(AutoGenerateColumnListener),
        typeof(Cell));

    /// <summary>Helper for setting <see cref="TemplateProperty"/> on <paramref name="element"/>.</summary>
    /// <param name="element"><see cref="DataGrid"/> to set <see cref="TemplateProperty"/> on.</param>
    /// <param name="value">Template property value.</param>
    public static void SetTemplate(this DataGrid element, DataTemplate? value)
    {
        if (element is null)
        {
            throw new ArgumentNullException(nameof(element));
        }

        element.SetValue(TemplateProperty, value);
    }

    /// <summary>Helper for getting <see cref="TemplateProperty"/> from <paramref name="element"/>.</summary>
    /// <param name="element"><see cref="DataGrid"/> to read <see cref="TemplateProperty"/> from.</param>
    /// <returns>Template property value.</returns>
    [AttachedPropertyBrowsableForChildren(IncludeDescendants = false)]
    [AttachedPropertyBrowsableForType(typeof(DataGrid))]
    public static DataTemplate? GetTemplate(this DataGrid element)
    {
        if (element is null)
        {
            throw new ArgumentNullException(nameof(element));
        }

        return (DataTemplate)element.GetValue(TemplateProperty);
    }

    /// <summary>Helper for setting <see cref="TemplateSelectorProperty"/> on <paramref name="element"/>.</summary>
    /// <param name="element"><see cref="DataGrid"/> to set <see cref="TemplateSelectorProperty"/> on.</param>
    /// <param name="value">TemplateSelector property value.</param>
    public static void SetTemplateSelector(DataGrid element, DataTemplateSelector? value)
    {
        if (element is null)
        {
            throw new ArgumentNullException(nameof(element));
        }

        element.SetValue(TemplateSelectorProperty, value);
    }

    /// <summary>Helper for getting <see cref="TemplateSelectorProperty"/> from <paramref name="element"/>.</summary>
    /// <param name="element"><see cref="DataGrid"/> to read <see cref="TemplateSelectorProperty"/> from.</param>
    /// <returns>TemplateSelector property value.</returns>
    [AttachedPropertyBrowsableForChildren(IncludeDescendants = false)]
    [AttachedPropertyBrowsableForType(typeof(DataGrid))]
    public static DataTemplateSelector? GetTemplateSelector(this DataGrid element)
    {
        if (element is null)
        {
            throw new ArgumentNullException(nameof(element));
        }

        return (DataTemplateSelector)element.GetValue(TemplateSelectorProperty);
    }

    /// <summary>Helper for setting <see cref="EditingTemplateProperty"/> on <paramref name="element"/>.</summary>
    /// <param name="element"><see cref="DataGrid"/> to set <see cref="EditingTemplateProperty"/> on.</param>
    /// <param name="value">EditingTemplate property value.</param>
    public static void SetEditingTemplate(this DataGrid element, DataTemplate? value)
    {
        if (element is null)
        {
            throw new ArgumentNullException(nameof(element));
        }

        element.SetValue(EditingTemplateProperty, value);
    }

    /// <summary>Helper for getting <see cref="EditingTemplateProperty"/> from <paramref name="element"/>.</summary>
    /// <param name="element"><see cref="DataGrid"/> to read <see cref="EditingTemplateProperty"/> from.</param>
    /// <returns>EditingTemplate property value.</returns>
    [AttachedPropertyBrowsableForChildren(IncludeDescendants = false)]
    [AttachedPropertyBrowsableForType(typeof(DataGrid))]
    public static DataTemplate? GetEditingTemplate(this DataGrid element)
    {
        if (element is null)
        {
            throw new ArgumentNullException(nameof(element));
        }

        return (DataTemplate)element.GetValue(EditingTemplateProperty);
    }

    /// <summary>Helper for setting <see cref="EditingTemplateSelectorProperty"/> on <paramref name="element"/>.</summary>
    /// <param name="element"><see cref="DependencyObject"/> to set <see cref="EditingTemplateSelectorProperty"/> on.</param>
    /// <param name="value">EditingTemplateSelector property value.</param>
    public static void SetEditingTemplateSelector(DependencyObject element, DataTemplateSelector? value)
    {
        if (element is null)
        {
            throw new ArgumentNullException(nameof(element));
        }

        element.SetValue(EditingTemplateSelectorProperty, value);
    }

    /// <summary>Helper for getting <see cref="EditingTemplateSelectorProperty"/> from <paramref name="element"/>.</summary>
    /// <param name="element"><see cref="DependencyObject"/> to read <see cref="EditingTemplateSelectorProperty"/> from.</param>
    /// <returns>EditingTemplateSelector property value.</returns>
    [AttachedPropertyBrowsableForType(typeof(DataGrid))]
    public static DataTemplateSelector? GetEditingTemplateSelector(this DependencyObject element)
    {
        if (element is null)
        {
            throw new ArgumentNullException(nameof(element));
        }

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
        if (dataGrid.GetValue(ListenerProperty) is null)
        {
            // ReSharper disable once HeapView.ObjectAllocation.Evident
#pragma warning disable IDISP004, CA2000  // Don't ignore return value of type IDisposable.
            dataGrid.SetCurrentValue(ListenerProperty, new AutoGenerateColumnListener(dataGrid));
#pragma warning restore IDISP004, CA2000  // Don't ignore return value of type IDisposable.
        }
    }

    private static void SetCellTemplateProperty(DataGrid dataGrid, DependencyPropertyChangedEventArgs e, bool editingTemplate)
    {
        if (e is { OldValue: { }, NewValue: null })
        {
            for (var i = 0; i < dataGrid.Columns.Count; ++i)
            {
                if (dataGrid.Columns[i] is CellTemplateColumn col)
                {
                    var temp = editingTemplate ?
                               col.GetValue(DataGridTemplateColumn.CellTemplateProperty) :
                               col.GetValue(DataGridTemplateColumn.CellEditingTemplateProperty);
                    if (temp is null)
                    {
                        var tcol = new DataGridTextColumn { Binding = col.Binding };
                        dataGrid.Columns[i] = tcol;
                    }
                    else
                    {
                        SetCellTemplateProperty(dataGrid, (DataTemplate?)e.NewValue, editingTemplate);
                    }
                }
            }
        }
        else if (e is { OldValue: null, NewValue: { } })
        {
            for (var i = 0; i < dataGrid.Columns.Count; ++i)
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
            SetCellTemplateProperty(dataGrid, (DataTemplate?)e.NewValue, editingTemplate);
        }
    }

    private static void SetCellTemplateProperty(DataGrid dataGrid, DataTemplate? t, bool editingTemplate)
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

    private sealed class AutoGenerateColumnListener : IDisposable
    {
        private readonly DataGrid dataGrid;

        internal AutoGenerateColumnListener(DataGrid dataGrid)
        {
            this.dataGrid = dataGrid;
            dataGrid.AutoGeneratingColumn += OnDataGridAutoGeneratingColumn;
        }

        public void Dispose()
        {
            this.dataGrid.AutoGeneratingColumn -= OnDataGridAutoGeneratingColumn;
        }

        private static void OnDataGridAutoGeneratingColumn(object? sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.Column is DataGridTextColumn { Binding: { } binding } &&
                sender is DataGrid dataGrid)
            {
                e.Column = new CellTemplateColumn
                {
                    Binding = binding,
                    CellTemplate = dataGrid.GetTemplate(),
                    CellEditingTemplate = dataGrid.GetEditingTemplate(),
                    CellTemplateSelector = dataGrid.GetTemplateSelector(),
                    CellEditingTemplateSelector = dataGrid.GetEditingTemplateSelector(),
                };
            }
        }
    }
}
