namespace Gu.Wpf.DataGrid2D
{
    using System.ComponentModel;

    /// <summary>
    /// A bindable representation for <see cref="Array2DView"/>.
    /// </summary>
    public class Array2DRowView : RowView<Array2DView>
    {
        private Array2DRowView(Array2DView source, int index, PropertyDescriptorCollection properties)
            : base(source, index, properties)
        {
        }

        internal static Array2DRowView CreateForRow(Array2DView source, int rowIndex)
        {
            var propertyDescriptors = Array2DIndexPropertyDescriptor.GetRowPropertyDescriptorCollection(source);
            return new Array2DRowView(source, rowIndex, propertyDescriptors);
        }

        internal static Array2DRowView CreateForColumn(Array2DView source, int columnIndex)
        {
            var propertyDescriptors = Array2DIndexPropertyDescriptor.GetColumnPropertyDescriptorCollection(source);
            return new Array2DRowView(source, columnIndex, propertyDescriptors);
        }
    }
}
