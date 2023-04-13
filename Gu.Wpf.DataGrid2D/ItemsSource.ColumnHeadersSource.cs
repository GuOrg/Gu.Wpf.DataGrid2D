namespace Gu.Wpf.DataGrid2D;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

/// <summary>
/// Attached properties for column headers.
/// </summary>
public static partial class ItemsSource
{
    /// <summary>For setting column headers.</summary>
    public static readonly DependencyProperty ColumnHeadersSourceProperty = DependencyProperty.RegisterAttached(
        "ColumnHeadersSource",
        typeof(IEnumerable),
        typeof(ItemsSource),
        new PropertyMetadata(null, OnColumnHeadersSourceChanged),
        HeadersSourceValidateValue);

    private static readonly RoutedEventHandler OnColumnsChangedHandler = OnColumnsChanged;

    private static readonly DependencyProperty ColumnHeaderListenerProperty = DependencyProperty.RegisterAttached(
        "ColumnHeaderListener",
        typeof(ColumnHeaderListener),
        typeof(ItemsSource),
        new PropertyMetadata(default(ColumnHeaderListener)));

    /// <summary>Helper for setting <see cref="ColumnHeadersSourceProperty"/> on <paramref name="element"/>.</summary>
    /// <param name="element"><see cref="DataGrid"/> to set <see cref="ColumnHeadersSourceProperty"/> on.</param>
    /// <param name="value">ColumnHeadersSource property value.</param>
    public static void SetColumnHeadersSource(this DataGrid element, IEnumerable? value)
    {
        if (element is null)
        {
            throw new ArgumentNullException(nameof(element));
        }

        element.SetValue(ColumnHeadersSourceProperty, value);
    }

    /// <summary>Helper for getting <see cref="ColumnHeadersSourceProperty"/> from <paramref name="element"/>.</summary>
    /// <param name="element"><see cref="DataGrid"/> to read <see cref="ColumnHeadersSourceProperty"/> from.</param>
    /// <returns>ColumnHeadersSource property value.</returns>
    [AttachedPropertyBrowsableForChildren(IncludeDescendants = false)]
    [AttachedPropertyBrowsableForType(typeof(DataGrid))]
    public static IEnumerable? GetColumnHeadersSource(this DataGrid element)
    {
        if (element is null)
        {
            throw new ArgumentNullException(nameof(element));
        }

        return (IEnumerable)element.GetValue(ColumnHeadersSourceProperty);
    }

    private static void OnColumnHeadersSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var dataGrid = (DataGrid)d;
        var headers = (IEnumerable?)e.NewValue;
#pragma warning disable IDISP007 // Don't dispose injected.
        (dataGrid.GetValue(ColumnHeaderListenerProperty) as IDisposable)?.Dispose();
#pragma warning restore IDISP007 // Don't dispose injected.
        dataGrid.ClearValue(ColumnHeaderListenerProperty);

        if (headers is null)
        {
            foreach (var column in dataGrid.Columns)
            {
                BindingOperations.ClearBinding(column, DataGridColumn.HeaderProperty);
            }

            dataGrid.RemoveHandler(Events.ColumnsChangedEvent, OnColumnsChangedHandler);
            return;
        }

#pragma warning disable IDISP004, CA2000  // Don't ignore return value of type IDisposable.
        dataGrid.SetCurrentValue(ColumnHeaderListenerProperty, new ColumnHeaderListener(dataGrid));
#pragma warning restore IDISP004, CA2000  // Don't ignore return value of type IDisposable.
        dataGrid.UpdateHandler(Events.ColumnsChangedEvent, OnColumnsChangedHandler);
        OnColumnsChanged(dataGrid, null);
    }

    private static void OnColumnsChanged(object sender, RoutedEventArgs? routedEventArgs)
    {
        var dataGrid = (DataGrid)sender;
        var headers = dataGrid.GetColumnHeadersSource();
        if (headers is null)
        {
            return;
        }

        var count = headers.Count();
        for (int i = 0; i < Math.Min(count, dataGrid.Columns.Count); i++)
        {
            var column = dataGrid.Columns[i];
            _ = column.Bind(DataGridColumn.HeaderProperty)
                      .OneWayTo(headers, i);
        }
    }

    private static bool HeadersSourceValidateValue(object? value)
    {
        if (value is null ||
            value is IList ||
            value is IReadOnlyList<object>)
        {
            return true;
        }

        return false;
    }
}
