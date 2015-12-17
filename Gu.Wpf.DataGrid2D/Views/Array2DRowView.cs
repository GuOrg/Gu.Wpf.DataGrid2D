namespace Gu.Wpf.DataGrid2D
{
    using System;
    using System.ComponentModel;

    public class Array2DRowView : CustomTypeDescriptor
    {
        private readonly PropertyDescriptorCollection properties;

        public Array2DRowView(Array source, int rowIndex)
        {
            this.Source.Target = source;
            this.RowIndex = rowIndex;
            this.properties = Array2DIndexPropertyDescriptor.GetPropertyDescriptorCollection(source);
        }

        internal WeakReference Source { get; } = new WeakReference(null);

        public int RowIndex { get; }

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
    }
}