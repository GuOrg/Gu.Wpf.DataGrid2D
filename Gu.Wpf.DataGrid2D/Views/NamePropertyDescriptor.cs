namespace Gu.Wpf.DataGrid2D
{
    using System;
    using System.ComponentModel;

    internal class NamePropertyDescriptor : PropertyDescriptor
    {
        private readonly PropertyDescriptor propertyDescriptor;

        internal NamePropertyDescriptor(PropertyDescriptor propertyDescriptor)
            : base("Name", null)
        {
            this.propertyDescriptor = propertyDescriptor;
        }

        public override Type ComponentType => typeof(string);

        public override bool IsReadOnly => true;

        public override Type PropertyType => typeof(string);

        public override bool CanResetValue(object component) => false;

        public override object GetValue(object component)
        {
            return this.propertyDescriptor.Name;
        }

        public override void ResetValue(object component)
        {
            // NOP
        }

        public override void SetValue(object component, object value)
        {
            throw new NotSupportedException();
        }

        public override bool ShouldSerializeValue(object component)
        {
            return true;
        }
    }
}