namespace Gu.Wpf.DataGrid2D
{
    using System;
    using System.ComponentModel;

    internal class NamePropertyDescriptor : PropertyDescriptor
    {
        private readonly PropertyDescriptor propertyDescriptor;

        internal NamePropertyDescriptor(PropertyDescriptor propertyDescriptor)
            : base(nameof(Name), null)
        {
            this.propertyDescriptor = propertyDescriptor;
        }

        /// <inheritdoc/>
        public override Type ComponentType => typeof(string);

        /// <inheritdoc/>
        public override bool IsReadOnly => true;

        /// <inheritdoc/>
        public override Type PropertyType => typeof(string);

        /// <inheritdoc/>
        public override bool CanResetValue(object component) => false;

        /// <inheritdoc/>
        public override object GetValue(object component)
        {
            return this.propertyDescriptor.Name;
        }

        /// <inheritdoc/>
        public override void ResetValue(object component)
        {
            // NOP
        }

        /// <inheritdoc/>
        public override void SetValue(object component, object value)
        {
            throw new NotSupportedException();
        }

        /// <inheritdoc/>
        public override bool ShouldSerializeValue(object component)
        {
            return true;
        }
    }
}
