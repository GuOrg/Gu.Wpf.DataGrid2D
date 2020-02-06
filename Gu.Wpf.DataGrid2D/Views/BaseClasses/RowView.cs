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

        public int Index { get; }

        public int Count => this.properties.Count;

        internal TSource Source { get; }

        /// <inheritdoc />
        public override string GetClassName() => this.GetType().FullName!;

        /// <inheritdoc />
        public override PropertyDescriptorCollection GetProperties() => this.properties;

        /// <inheritdoc />
        public override PropertyDescriptorCollection GetProperties(Attribute[] attributes) => this.properties;
    }
}
