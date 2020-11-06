namespace Gu.Wpf.DataGrid2D
{
    using System.Collections;

    /// <summary>
    /// A bindable view.
    /// </summary>
    public interface IView2D
    {
        /// <summary>Gets the source collection.</summary>
        IEnumerable? Source { get; }

        /// <summary>Gets a value indicating whether the source collection is transposed.</summary>
        bool IsTransposed { get; }
    }
}
