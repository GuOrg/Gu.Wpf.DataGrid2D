namespace Gu.Wpf.DataGrid2D
{
    using System;
    using System.Collections;
    using System.Windows.Controls;

    internal interface IView2D : IDisposable
    {
        event EventHandler ColumnsChanged;

        IEnumerable Source { get; }

        DataGrid DataGrid { get; set; }
    }
}