namespace Gu.Wpf.DataGrid2D
{
    using System;
    using System.ComponentModel;

    internal abstract class IndexPropertyDescriptor : PropertyDescriptor
    {
        private readonly Type elementType;
        protected readonly int index;

        protected IndexPropertyDescriptor(Type elementType, int index)
            : base($"C{index}", null)
        {
            this.elementType = elementType;
            this.index = index;
        }

        public override Type ComponentType => this.elementType;

        public override bool IsReadOnly => false;

        public override Type PropertyType => this.elementType;

        public override bool CanResetValue(object component) => false;

        public override void ResetValue(object component)
        {
            // NOP
        }

        public override bool ShouldSerializeValue(object component)
        {
            return true;
        }
    }
}