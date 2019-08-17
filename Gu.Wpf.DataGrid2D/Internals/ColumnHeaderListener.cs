namespace Gu.Wpf.DataGrid2D
{
    using System;
    using System.Collections.Specialized;
    using System.Windows;
    using System.Windows.Controls;

    internal class ColumnHeaderListener : IDisposable
    {
        private static readonly RoutedEventArgs ColumnsChangedEventArgs = new RoutedEventArgs(Events.ColumnsChangedEvent);

        private readonly DataGrid dataGrid;
        private bool disposed;

        internal ColumnHeaderListener(DataGrid dataGrid)
        {
            this.dataGrid = dataGrid;
            dataGrid.Columns.CollectionChanged += this.OnCollectionChanged;
            var headers = dataGrid.GetColumnHeadersSource() as INotifyCollectionChanged;
            if (headers != null)
            {
                headers.CollectionChanged += this.OnCollectionChanged;
            }
        }

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
            this.dataGrid.RaiseEvent(ColumnsChangedEventArgs);
        }
    }
}
