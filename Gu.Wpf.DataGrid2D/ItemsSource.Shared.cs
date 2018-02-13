#pragma warning disable 1591
namespace Gu.Wpf.DataGrid2D
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Controls;
    using Gu.Wpf.DataGrid2D.Internals;

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
                oldView.Dispose();
                oldView.ColumnsChanged -= OnViewColumnsChanged;
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
            var rowsSource = (IEnumerable<IEnumerable>)dataGrid.GetRowsSource();
            if (rowsSource != null)
            {
                view = new Lists2DView(rowsSource);
            }

            var colsSource = (IEnumerable<IEnumerable>)dataGrid.GetColumnsSource();
            if (colsSource != null)
            {
                view = new Lists2DTransposedView(colsSource);
            }

            var transposedSource = dataGrid.GetTransposedSource();
            if (transposedSource != null)
            {
                view = new TransposedItemsSource(transposedSource);
            }

            dataGrid.Bind(ItemsControl.ItemsSourceProperty)
                    .OneWayTo(view)
                    .IgnoreReturnValue();
            dataGrid.RaiseEvent(new RoutedEventArgs(Events.ColumnsChangedEvent));
        }
    }
}
