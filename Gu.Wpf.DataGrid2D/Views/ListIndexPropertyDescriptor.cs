namespace Gu.Wpf.DataGrid2D
{
    using System;

    internal class ListIndexPropertyDescriptor : IndexPropertyDescriptor
    {
        private ListIndexPropertyDescriptor(Type elementType, int index)
            : base(elementType, index)
        {
        }

        //internal static PropertyDescriptorCollection GetRowPropertyDescriptorCollection(Array source)
        //{
        //    return RowDescriptorCache.GetValue(source, CreateRowPropertyDescriptorCollection);
        //}

        //internal static PropertyDescriptorCollection GetColumnPropertyDescriptorCollection(Array source)
        //{
        //    return ColumnDescriptorCache.GetValue(source, CreateColumnPropertyDescriptorCollection);
        //}

        public override object GetValue(object component)
        {
            throw new NotImplementedException();
        }

        public override void SetValue(object component, object value)
        {
            throw new NotImplementedException();
        }
    }
}