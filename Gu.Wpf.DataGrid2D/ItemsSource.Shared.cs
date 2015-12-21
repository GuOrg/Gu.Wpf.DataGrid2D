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
            var oldView = e.OldValue as IView2D;
            if (oldView != null)
            {
                oldView.Dispose();
                oldView.ColumnsChanged -= OnViewColumnsChanged;
            }

            var newView = e.NewValue as IView2D;
            if (newView != null)
            {
                newView.ColumnsChanged += OnViewColumnsChanged;
                newView.DataGrid = (DataGrid)d;
            }
        }

        private static void OnViewColumnsChanged(object sender, EventArgs e)
        {
            var view = (IView2D)sender;
            UpdateItemsSource(view.DataGrid);
        }

        private static void UpdateItemsSource(DataGrid dataGrid)
        {
            IEnumerable view = null;
            var rowsSource = (IEnumerable<IEnumerable>)dataGrid.GetRowsSource();
            if (rowsSource != null)
            {
                view = Lists2DView.Create(rowsSource);
            }

            var colsSource = (IEnumerable<IEnumerable>)dataGrid.GetColumnsSource();
            if (colsSource != null)
            {
                view = Lists2DView.CreateTransposed(colsSource);
            }

            var transposedSource = dataGrid.GetTransposedSource();
            if (transposedSource != null)
            {
                view = new TransposedItemsSource(transposedSource);
            }

            dataGrid.Bind(ItemsControl.ItemsSourceProperty)
                    .OneWayTo(view);
            dataGrid.RaiseEvent(new RoutedEventArgs(Events.ColumnsChanged));
        }
    }
}
