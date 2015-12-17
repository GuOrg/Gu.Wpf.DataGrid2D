namespace Gu.Wpf.DataGrid2D
{
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    internal class Array2DIndexPropertyDescriptor : PropertyDescriptor
    {
        private static readonly ConditionalWeakTable<Array, PropertyDescriptorCollection> RowDescriptorCache = new ConditionalWeakTable<Array, PropertyDescriptorCollection>();
        private static readonly ConditionalWeakTable<Array, PropertyDescriptorCollection> ColumnDescriptorCache = new ConditionalWeakTable<Array, PropertyDescriptorCollection>();
        private readonly Type elementType;
        private readonly int index;

        private Array2DIndexPropertyDescriptor(Type elementType, int index)
            : base($"C{index}", null)
        {
            this.elementType = elementType;
            this.index = index;
        }

        public override Type ComponentType => this.elementType;

        public override bool IsReadOnly => false;

        public override Type PropertyType => this.elementType;

        internal static PropertyDescriptorCollection GetRowPropertyDescriptorCollection(Array source)
        {
            return RowDescriptorCache.GetValue(source, CreateRowPropertyDescriptorCollection);
        }

        internal static PropertyDescriptorCollection GetColumnPropertyDescriptorCollection(Array source)
        {
            return ColumnDescriptorCache.GetValue(source, CreateColumnPropertyDescriptorCollection);
        }

        public override bool CanResetValue(object component) => false;

        public override object GetValue(object component)
        {
            var rowView = (Array2DRowView)component;
            var source = (Array)rowView.Source.Target;
            if (rowView.IsTransposed)
            {
                return source?.GetValue(this.index, rowView.Index);
            }

            return source?.GetValue(rowView.Index, this.index);
        }

        public override void ResetValue(object component)
        {
            // NOP
        }

        public override void SetValue(object component, object value)
        {
            var rowView = (Array2DRowView)component;
            var source = (Array)rowView.Source.Target;
            if (rowView.IsTransposed)
            {
                source?.SetValue(value, this.index, rowView.Index);
            }
            else
            {
                source?.SetValue(value, rowView.Index, this.index);
            }
        }

        public override bool ShouldSerializeValue(object component)
        {
            return true;
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