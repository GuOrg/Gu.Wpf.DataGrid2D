namespace Gu.Wpf.DataGrid2D
{
    using System;
    using System.Collections.Specialized;
    using System.Windows;
    using System.Windows.Controls;

    internal class ColumnHeaderListener : IDisposable
    {
        private static readonly RoutedEventArgs ColumnsChangedEventArgs = new RoutedEventArgs(ItemsSource.ColumnsChangedEvent);

        private readonly DataGrid dataGrid;
        private bool disposed;

        public ColumnHeaderListener(DataGrid dataGrid)
        {
            this.dataGrid = dataGrid;
            dataGrid.Columns.CollectionChanged += OnCollectionChanged;
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
            this.dataGrid.Columns.CollectionChanged -= OnCollectionChanged;
            var headers = this.dataGrid.GetColumnHeadersSource() as INotifyCollectionChanged;
            if (headers != null)
            {
                headers.CollectionChanged += this.OnCollectionChanged;
            }
        }

        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
        {
            this.dataGrid.RaiseEvent(ColumnsChangedEventArgs);
        }
    }
}
