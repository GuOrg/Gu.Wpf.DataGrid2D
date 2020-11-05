namespace Gu.Wpf.DataGrid2D
{
    using System;
    using System.Collections.Specialized;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;

    internal sealed class RowHeaderListener : IDisposable
    {
        private readonly DataGrid dataGrid;
        private bool disposed;

        internal RowHeaderListener(DataGrid dataGrid)
        {
            this.dataGrid = dataGrid;
            ////dataGrid.ItemContainerGenerator.ItemsChanged += this.OnItemsChanged;
            dataGrid.ItemContainerGenerator.StatusChanged += this.OnStatusChanged;
            if (dataGrid.GetRowHeadersSource() is INotifyCollectionChanged headers)
            {
                headers.CollectionChanged += this.OnHeadersChanged;
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
            ////this.dataGrid.ItemContainerGenerator.ItemsChanged -= this.OnItemsChanged;
            this.dataGrid.ItemContainerGenerator.StatusChanged -= this.OnStatusChanged;
            if (this.dataGrid.GetRowHeadersSource() is INotifyCollectionChanged headers)
            {
                headers.CollectionChanged += this.OnHeadersChanged;
            }
        }

        ////private void OnItemsChanged(object o, ItemsChangedEventArgs e)
        ////{
        ////    this.dataGrid.RaiseEvent(RowsChangedEventArgs);
        ////}

        private void OnHeadersChanged(object o, NotifyCollectionChangedEventArgs e)
        {
            this.dataGrid.RaiseEvent(new RoutedEventArgs(Events.RowsChangedEvent));
        }

        private void OnStatusChanged(object? o, EventArgs e)
        {
            if (o is ItemContainerGenerator { Status: GeneratorStatus.ContainersGenerated })
            {
                this.dataGrid.RaiseEvent(new RoutedEventArgs(Events.RowsChangedEvent));
            }
        }
    }
}
