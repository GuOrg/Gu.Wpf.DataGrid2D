namespace Gu.Wpf.DataGrid2D;

using System;
using System.Windows.Controls;

/// <summary>
/// For types signaling column changes.
/// </summary>
internal interface IColumnsChanged : IDisposable
{
    /// <summary>
    /// The change event.
    /// </summary>
    event EventHandler? ColumnsChanged;

    /// <summary>
    /// Gets or sets the tracked <see cref="DataGrid"/>.
    /// </summary>
    DataGrid? DataGrid { get; set; }
}
