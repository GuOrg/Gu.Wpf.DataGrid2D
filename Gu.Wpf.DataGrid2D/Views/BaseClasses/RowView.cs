namespace Gu.Wpf.DataGrid2D
{
    using System;
    using System.ComponentModel;

    public class RowView<TSource> : CustomTypeDescriptor
    {
        private readonly PropertyDescriptorCollection properties;

        protected RowView(TSource source, int index, PropertyDescriptorCollection properties)
        {
            this.Source = source;
            this.Index = index;
            this.properties = properties;
        }

        internal TSource Source { get; }

        public int Index { get; }

        public int Count => this.properties.Count;

        public override string GetClassName() => this.GetType().FullName;

        public override PropertyDescriptorCollection GetProperties()
        {
            return this.properties;
        }

        public override PropertyDescriptorCollection GetProperties(Attribute[] attributes)
        {
            return this.properties;
        }
    }
}