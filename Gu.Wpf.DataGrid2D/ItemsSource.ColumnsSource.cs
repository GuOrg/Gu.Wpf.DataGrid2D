namespace Gu.Wpf.DataGrid2D;

using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

/// <summary>
/// Attached properties for setting columns.
/// </summary>
public static partial class ItemsSource
{
    /// <summary>
    /// An <see cref="IEnumerable"/> of columns where each row is an <see cref="IEnumerable"/> with the values.
    /// </summary>
    public static readonly DependencyProperty ColumnsSourceProperty = DependencyProperty.RegisterAttached(
        "ColumnsSource",
        typeof(IEnumerable),
        typeof(ItemsSource),
        new PropertyMetadata(default(IEnumerable), OnColumnsSourceChanged),
        x => x is null || x is IEnumerable<IEnumerable>);

    /// <summary>Helper for setting <see cref="ColumnsSourceProperty"/> on <paramref name="element"/>.</summary>
    /// <param name="element"><see cref="DataGrid"/> to set <see cref="ColumnsSourceProperty"/> on.</param>
    /// <param name="value">ColumnsSource property value.</param>
    public static void SetColumnsSource(this DataGrid element, IEnumerable? value)
    {
        if (element is null)
        {
            throw new System.ArgumentNullException(nameof(element));
        }

        element.SetValue(ColumnsSourceProperty, value);
    }

    /// <summary>Helper for getting <see cref="ColumnsSourceProperty"/> from <paramref name="element"/>.</summary>
    /// <param name="element"><see cref="DataGrid"/> to read <see cref="ColumnsSourceProperty"/> from.</param>
    /// <returns>ColumnsSource property value.</returns>
    [AttachedPropertyBrowsableForChildren(IncludeDescendants = false)]
    [AttachedPropertyBrowsableForType(typeof(DataGrid))]
    public static IEnumerable? GetColumnsSource(this DataGrid element)
    {
        if (element is null)
        {
            throw new System.ArgumentNullException(nameof(element));
        }

        return (IEnumerable)element.GetValue(ColumnsSourceProperty);
    }

    private static void OnColumnsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var dataGrid = (DataGrid)d;
        if (e.NewValue is not IEnumerable<IEnumerable>)
        {
            BindingOperations.ClearBinding(dataGrid, ItemsControl.ItemsSourceProperty);
            BindingOperations.ClearBinding(dataGrid, ItemsSourceProxyProperty);
            return;
        }

        _ = dataGrid.Bind(ItemsSourceProxyProperty)
                    .OneWayTo(dataGrid, ItemsControl.ItemsSourceProperty);
        UpdateItemsSource(dataGrid);
    }
}
