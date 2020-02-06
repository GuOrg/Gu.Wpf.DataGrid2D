#pragma warning disable 1591
namespace Gu.Wpf.DataGrid2D
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Controls;

    public static partial class ItemsSource
    {
        // Using this for disposing ListView2D
        private static readonly DependencyProperty ItemsSourceProxyProperty = DependencyProperty.RegisterAttached(
            "ItemsSourceProxy",
            typeof(IEnumerable),
            typeof(ItemsSource),
            new PropertyMetadata(default(IEnumerable), OnItemsSourceProxyChanged));

        private static void OnItemsSourceProxyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue is IColumnsChanged oldView)
            {
                oldView.ColumnsChanged -= OnViewColumnsChanged;
                oldView.Dispose();
            }

            if (e.NewValue is IColumnsChanged newView)
            {
                newView.ColumnsChanged += OnViewColumnsChanged;
                newView.DataGrid = (DataGrid)d;
            }
        }

        private static void OnViewColumnsChanged(object sender, EventArgs e)
        {
            var view = (IColumnsChanged)sender;
            UpdateItemsSource(view.DataGrid);
        }

        private static void UpdateItemsSource(DataGrid dataGrid)
        {
            IEnumerable view = null;
            if (dataGrid.GetRowsSource() is IEnumerable<IEnumerable> rowsSource)
            {
#pragma warning disable IDISP001 // Dispose created.
                view = new Lists2DView(rowsSource);
#pragma warning restore IDISP001 // Dispose created.
            }
            else if (dataGrid.GetColumnsSource() is IEnumerable<IEnumerable> colsSource)
            {
#pragma warning disable IDISP001 // Dispose created.
                view = new Lists2DTransposedView(colsSource);
#pragma warning restore IDISP001 // Dispose created.
            }
            else if (dataGrid.GetTransposedSource() is { } transposedSource)
            {
#pragma warning disable IDISP001 // Dispose created.
                view = new TransposedItemsSource(transposedSource);
#pragma warning restore IDISP001 // Dispose created.
            }

            _ = dataGrid.Bind(ItemsControl.ItemsSourceProperty)
                        .OneWayTo(view);
            dataGrid.RaiseEvent(new RoutedEventArgs(Events.ColumnsChangedEvent));
        }
    }
}
