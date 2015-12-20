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
            var oldList2DView = e.OldValue as Lists2DView;
            if (oldList2DView != null)
            {
                oldList2DView.Dispose();
                oldList2DView.ColumnsChanged -= OnViewColumnsChanged;
            }

            var newLists2DView = e.NewValue as Lists2DView;
            if (newLists2DView != null)
            {
                newLists2DView.ColumnsChanged += OnViewColumnsChanged;
                newLists2DView.DataGrid = (DataGrid)d;
            }
        }

        private static void OnViewColumnsChanged(object sender, EventArgs e)
        {
            var view = (Lists2DView)sender;
            if (ReferenceEquals(view.DataGrid.GetRowsSource(), view.Source))
            {
                UpdateItemsSource(view.DataGrid);
                return;
            }

            if (ReferenceEquals(view.DataGrid.GetColumnsSource(), view.Source))
            {
                UpdateItemsSource(view.DataGrid);
                return;
            }

            throw new ArgumentOutOfRangeException();
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
                view = new TransformedItemsSource(transposedSource);
            }

            dataGrid.Bind(ItemsControl.ItemsSourceProperty)
                    .OneWayTo(view);
            dataGrid.RaiseEvent(new RoutedEventArgs(Events.ColumnsChanged));
        }
    }
}
