namespace Gu.Wpf.DataGrid2D
{
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    internal class Array2DIndexPropertyDescriptor : IndexPropertyDescriptor
    {
        private static readonly ConditionalWeakTable<Array, PropertyDescriptorCollection> RowDescriptorCache = new ConditionalWeakTable<Array, PropertyDescriptorCollection>();
        private static readonly ConditionalWeakTable<Array, PropertyDescriptorCollection> ColumnDescriptorCache = new ConditionalWeakTable<Array, PropertyDescriptorCollection>();

        private Array2DIndexPropertyDescriptor(Type elementType, int index)
            : base(elementType, index, false)
        {
        }

        internal static PropertyDescriptorCollection GetRowPropertyDescriptorCollection(Array source)
        {
            return RowDescriptorCache.GetValue(source, CreateRowPropertyDescriptorCollection);
        }

        internal static PropertyDescriptorCollection GetColumnPropertyDescriptorCollection(Array source)
        {
            return ColumnDescriptorCache.GetValue(source, CreateColumnPropertyDescriptorCollection);
        }

        public override object GetValue(object component)
        {
            var rowView = (Array2DRowView)component;
            var source = rowView.Source;
            if (rowView.IsTransposed)
            {
                return source?.GetValue(this.Index, rowView.Index);
            }

            return source?.GetValue(rowView.Index, this.Index);
        }

        public override void SetValue(object component, object value)
        {
            var rowView = (Array2DRowView)component;
            var source = (Array)rowView.Source;
            if (rowView.IsTransposed)
            {
                source?.SetValue(value, this.Index, rowView.Index);
            }
            else
            {
                source?.SetValue(value, rowView.Index, this.Index);
            }
        }

        private static PropertyDescriptorCollection CreateRowPropertyDescriptorCollection(Array source)
        {
            var elementType = source.GetType().GetElementType();
            var n = source.GetLength(1);
            var descriptors = new Array2DIndexPropertyDescriptor[n];
            for (int i = 0; i < n; i++)
            {
                descriptors[i] = new Array2DIndexPropertyDescriptor(elementType, i);
            }

            return new PropertyDescriptorCollection(descriptors);
        }

        private static PropertyDescriptorCollection CreateColumnPropertyDescriptorCollection(Array source)
        {
            var elementType = source.GetType().GetElementType();
            var n = source.GetLength(0);
            var descriptors = new Array2DIndexPropertyDescriptor[n];
            for (int i = 0; i < n; i++)
            {
                descriptors[i] = new Array2DIndexPropertyDescriptor(elementType, i);
            }

            return new PropertyDescriptorCollection(descriptors);
        }
    }
}