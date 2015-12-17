namespace Gu.Wpf.DataGrid2D
{
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    internal class Array2DIndexPropertyDescriptor : PropertyDescriptor
    {
        private static readonly ConditionalWeakTable<Array, PropertyDescriptorCollection> Cache = new ConditionalWeakTable<Array, PropertyDescriptorCollection>();
        private readonly Type elementType;
        private readonly int columnIndex;

        private Array2DIndexPropertyDescriptor(Type elementType, int columnIndex)
            : base($"C{columnIndex}", null)
        {
            this.elementType = elementType;
            this.columnIndex = columnIndex;
        }

        public override Type ComponentType => this.elementType;

        public override bool IsReadOnly => false;

        public override Type PropertyType => this.elementType;

        public static PropertyDescriptorCollection GetPropertyDescriptorCollection(Array source)
        {
            return Cache.GetValue(source, CreatePropertyDescriptorCollection);
        }

        public override bool CanResetValue(object component) => false;

        public override object GetValue(object component)
        {
            var rowView = (Array2DRowView)component;
            var source = (Array)rowView.Source.Target;
            return source?.GetValue(rowView.RowIndex, this.columnIndex);
        }

        public override void ResetValue(object component)
        {
            // NOP
        }

        public override void SetValue(object component, object value)
        {
            var rowView = (Array2DRowView)component;
            var source = (Array)rowView.Source.Target;
            source?.SetValue(value, rowView.RowIndex, this.columnIndex);
        }

        public override bool ShouldSerializeValue(object component)
        {
            return true;
        }

        private static PropertyDescriptorCollection CreatePropertyDescriptorCollection(Array source)
        {
            var elementType = source.GetType().GetElementType();
            var descriptors = new Array2DIndexPropertyDescriptor[source.GetLength(0)];
            for (int i = 0; i < source.GetLength(1); i++)
            {
                descriptors[i] = new Array2DIndexPropertyDescriptor(elementType, i);
            }

            return new PropertyDescriptorCollection(descriptors);
        }
    }
}