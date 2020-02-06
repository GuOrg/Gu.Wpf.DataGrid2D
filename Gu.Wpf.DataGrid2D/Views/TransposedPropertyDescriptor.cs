namespace Gu.Wpf.DataGrid2D
{
    using System.ComponentModel;

    internal class TransposedPropertyDescriptor : IndexPropertyDescriptor
    {
        private readonly PropertyDescriptor propertyDescriptor;

        internal TransposedPropertyDescriptor(int index, PropertyDescriptor propertyDescriptor)
            : base(propertyDescriptor.PropertyType, index, propertyDescriptor.IsReadOnly)
        {
            this.propertyDescriptor = propertyDescriptor;
        }

        public override object GetValue(object component)
        {
            var row = (TransposedRow)component;
            var match = row.Source.Source?.ElementAtOrDefault(this.Index);
            return this.propertyDescriptor.GetValue(match);
        }

        public override void SetValue(object component, object value)
        {
            var row = (TransposedRow)component;
            var match = row.Source.Source?.ElementAtOrDefault(this.Index);
            this.propertyDescriptor.SetValue(match, value);
        }
    }
}
