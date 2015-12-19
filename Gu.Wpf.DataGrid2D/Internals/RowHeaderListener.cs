namespace Gu.Wpf.DataGrid2D
{
    using System;
    using System.Collections.Specialized;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;

    internal class RowHeaderListener : IDisposable
    {
        private static readonly RoutedEventArgs RowsChangedEventArgs = new RoutedEventArgs(Events.RowsChanged);

        private readonly DataGrid dataGrid;
        private bool disposed;

        public RowHeaderListener(DataGrid dataGrid)
        {
            this.dataGrid = dataGrid;
            dataGrid.ItemContainerGenerator.ItemsChanged += this.OnItemsChanged;
            dataGrid.ItemContainerGenerator.StatusChanged += this.OnStatusChanged;
            var headers = dataGrid.GetRowHeadersSource() as INotifyCollectionChanged;
            if (headers != null)
            {
                headers.CollectionChanged += this.OnHeadersChanged;
            }
        }

        public void Dispose()
        {
            if (this.disposed)
            {
                return;
            }

            this.disposed = true;
            this.dataGrid.ItemContainerGenerator.ItemsChanged -= this.OnItemsChanged;
            this.dataGrid.ItemContainerGenerator.StatusChanged -= this.OnStatusChanged;
            var headers = this.dataGrid.GetRowHeadersSource() as INotifyCollectionChanged;
            if (headers != null)
            {
                headers.CollectionChanged += this.OnHeadersChanged;
            }
        }

        private void OnItemsChanged(object o, ItemsChangedEventArgs e)
        {
            this.dataGrid.RaiseEvent(RowsChangedEventArgs);
        }

        private void OnHeadersChanged(object o, NotifyCollectionChangedEventArgs e)
        {
            this.dataGrid.RaiseEvent(RowsChangedEventArgs);
        }

        private void OnStatusChanged(object o, EventArgs e)
        {
            var generator = (ItemContainerGenerator)o;
            if (generator.Status == GeneratorStatus.ContainersGenerated)
            {
                this.dataGrid.RaiseEvent(RowsChangedEventArgs);
            }
        }
    }
}