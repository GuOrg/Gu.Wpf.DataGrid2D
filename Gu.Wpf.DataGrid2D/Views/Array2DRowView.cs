namespace Gu.Wpf.DataGrid2D
{
    using System;
    using System.ComponentModel;

    public class Array2DRowView : RowView<Array>
    {
        private Array2DRowView(Array source, int index, PropertyDescriptorCollection properties, bool isTransposed)
            : base(source, index, properties, isTransposed)
        {
        }

        internal static Array2DRowView CreateForRow(Array source, int rowIndex)
        {
            var propertyDescriptors = Array2DIndexPropertyDescriptor.GetRowPropertyDescriptorCollection(source);
            return new Array2DRowView(source, rowIndex, propertyDescriptors, false);
        }

        internal static Array2DRowView CreateForColumn(Array source, int columnIndex)
        {
            var propertyDescriptors = Array2DIndexPropertyDescriptor.GetColumnPropertyDescriptorCollection(source);
            return new Array2DRowView(source, columnIndex, propertyDescriptors, true);
        }
    }
}