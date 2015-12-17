namespace Gu.Wpf.DataGrid2D
{
    using System;
    using System.ComponentModel;
    using System.Linq;

    public class Array2DRowView : CustomTypeDescriptor
    {
        private readonly Array source;
        private readonly int rowIndex;
        private readonly PropertyDescriptorCollection properties;

        public Array2DRowView(Array source, int rowIndex)
        {
            this.source = source;
            this.rowIndex = rowIndex;
            var indexPropertyDescriptors = Enumerable.Range(0, this.source.GetLength(0) - 1)
                                                     .Select(x => new IndexPropertyDescriptor(
                                                                       source.GetType().GetElementType(),
                                                                      () => source.GetValue(rowIndex, x),
                                                                      o => source.SetValue(o, rowIndex, x),
                                                                      $"C{x}"))
                                                     .ToArray();
            this.properties = new PropertyDescriptorCollection(indexPropertyDescriptors);
        }

        public override string GetClassName()
        {
            return base.GetClassName();
        }

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