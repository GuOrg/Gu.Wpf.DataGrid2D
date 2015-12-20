namespace Gu.Wpf.DataGrid2D
{
    using System;
    using System.ComponentModel;

    internal class TransformedPropertyDescriptor : IndexPropertyDescriptor
    {
        private readonly PropertyDescriptor propertyDescriptor;

        internal TransformedPropertyDescriptor(int index, PropertyDescriptor propertyDescriptor)
            : base(propertyDescriptor.PropertyType, index, propertyDescriptor.IsReadOnly)
        {
            this.propertyDescriptor = propertyDescriptor;
        }

        public override object GetValue(object component)
        {
            var row = (TransformedRow)component;
            var match = row.Source.Source.ElementAtOrDefault(this.Index);
            return this.propertyDescriptor.GetValue(match);
        }

        public override void SetValue(object component, object value)
        {
            var row = (TransformedRow)component;
            var match = row.Source.Source.ElementAtOrDefault(this.Index);
            this.propertyDescriptor.SetValue(match, value);
        }
    }
}