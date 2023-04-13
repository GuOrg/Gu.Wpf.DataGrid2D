namespace Gu.Wpf.DataGrid2D;

using System;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;

internal sealed class ColumnHeaderListener : IDisposable
{
    private readonly DataGrid dataGrid;
    private bool disposed;

    internal ColumnHeaderListener(DataGrid dataGrid)
    {
        this.dataGrid = dataGrid;
        dataGrid.Columns.CollectionChanged += this.OnCollectionChanged;
        if (dataGrid.GetColumnHeadersSource() is INotifyCollectionChanged headers)
        {
            headers.CollectionChanged += this.OnCollectionChanged;
        }
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        if (this.disposed)
        {
            return;
        }

        this.disposed = true;
        this.dataGrid.Columns.CollectionChanged -= this.OnCollectionChanged;
        if (this.dataGrid.GetColumnHeadersSource() is INotifyCollectionChanged headers)
        {
            headers.CollectionChanged -= this.OnCollectionChanged;
        }
    }

    private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
    {
        this.dataGrid.RaiseEvent(new RoutedEventArgs(Events.ColumnsChangedEvent));
    }
}
