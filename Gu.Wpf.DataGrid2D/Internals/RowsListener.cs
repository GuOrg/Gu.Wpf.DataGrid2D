namespace Gu.Wpf.DataGrid2D;

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

internal sealed class RowsListener : IDisposable
{
    private readonly DataGrid dataGrid;
    private bool disposed;

    internal RowsListener(DataGrid dataGrid)
    {
        this.dataGrid = dataGrid;
        dataGrid.ItemContainerGenerator.StatusChanged += this.OnStatusChanged;
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        if (this.disposed)
        {
            return;
        }

        this.disposed = true;
        this.dataGrid.ItemContainerGenerator.StatusChanged -= this.OnStatusChanged;
    }

    private void OnStatusChanged(object? o, EventArgs e)
    {
        if (o is ItemContainerGenerator { Status: GeneratorStatus.ContainersGenerated })
        {
            this.dataGrid.RaiseEvent(new RoutedEventArgs(Events.RowsChangedEvent));
        }
    }
}
