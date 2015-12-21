namespace Gu.Wpf.DataGrid2D
{
    using System;
    using System.Collections;
    using System.ComponentModel;

    public class RowView<TSource> : CustomTypeDescriptor
    {
        private readonly PropertyDescriptorCollection properties;
        private readonly WeakReference source;

        protected RowView(IEnumerable source, int index, PropertyDescriptorCollection properties, bool isTransposed)
        {
            this.source = new WeakReference(source);
            this.Index = index;
            this.properties = properties;
            this.IsTransposed = isTransposed;
        }

        internal TSource Source => (TSource)this.source.Target;

        public int Index { get; }

        public int Count => this.properties.Count;

        public bool IsTransposed { get; }

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