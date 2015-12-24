namespace Gu.Wpf.DataGrid2D
{
    using System;
    using System.Windows.Controls;

    internal interface IColumnsChanged : IDisposable
    {
        event EventHandler ColumnsChanged;

        DataGrid DataGrid { get; set; }
    }
}