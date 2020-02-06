namespace Gu.Wpf.DataGrid2D
{
    using System.Collections;

    public interface IView2D
    {
        IEnumerable? Source { get; }

        bool IsTransposed { get; }
    }
}
