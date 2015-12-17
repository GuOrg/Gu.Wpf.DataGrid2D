namespace Gu.Wpf.DataGrid2D
{
    using System;
    using System.Collections.Specialized;
    using System.Windows;
    using System.Windows.Controls;

    internal class RowHeaderListener : IDisposable
    {
        private static readonly RoutedEventArgs RowsChangedEventArgs = new RoutedEventArgs(ItemsSource.RowsChangedEvent);

        private readonly DataGrid dataGrid;
        private bool disposed;

        public RowHeaderListener(DataGrid dataGrid)
        {
            this.dataGrid = dataGrid;
            dataGrid.ItemContainerGenerator.ItemsChanged += OnCollectionChanged;
            dataGrid.ItemContainerGenerator.StatusChanged += OnCollectionChanged;
            var headers = dataGrid.GetRowHeadersSource() as INotifyCollectionChanged;
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
            this.dataGrid.ItemContainerGenerator.ItemsChanged -= OnCollectionChanged;
            this.dataGrid.ItemContainerGenerator.StatusChanged -= OnCollectionChanged;
            var headers = this.dataGrid.GetRowHeadersSource() as INotifyCollectionChanged;
            if (headers != null)
            {
                headers.CollectionChanged += this.OnCollectionChanged;
            }
        }

        private void OnCollectionChanged(object o, EventArgs e)
        {
            this.dataGrid.RaiseEvent(RowsChangedEventArgs);
        }
    }
}