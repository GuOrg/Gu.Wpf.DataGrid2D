namespace Gu.Wpf.DataGrid2D
{
    using System;
    using System.ComponentModel;

    public class Array2DRowView : CustomTypeDescriptor
    {
        private readonly PropertyDescriptorCollection properties;

        private Array2DRowView(Array source, int index, PropertyDescriptorCollection properties, bool isTransposed)
        {
            this.Source.Target = source;
            this.Index = index;
            this.properties = properties;
            this.IsTransposed = isTransposed;
        }

        internal WeakReference Source { get; } = new WeakReference(null);

        public int Index { get; }

        public int Count => this.properties.Count;

        public bool IsTransposed { get; }

        public override string GetClassName() => this.GetType().FullName;

        public override PropertyDescriptor GetDefaultProperty()
        {
            return base.GetDefaultProperty();
        }

        public override PropertyDescriptorCollection GetProperties()
        {
            return this.properties;
        }

        public override PropertyDescriptorCollection GetProperties(Attribute[] attributes)
        {
            return this.properties;
        }

        public override object GetPropertyOwner(PropertyDescriptor pd)
        {
            return base.GetPropertyOwner(pd);
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