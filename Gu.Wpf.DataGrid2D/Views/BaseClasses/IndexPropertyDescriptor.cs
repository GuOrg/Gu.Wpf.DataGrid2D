namespace Gu.Wpf.DataGrid2D
{
    using System;
    using System.ComponentModel;

    internal abstract class IndexPropertyDescriptor : PropertyDescriptor
    {
        private readonly Type elementType;

        protected IndexPropertyDescriptor(Type elementType, int index, bool isReadOnly)
            : base($"C{index}", null)
        {
            this.IsReadOnly = isReadOnly;
            this.elementType = elementType;
            this.Index = index;
        }

        public override Type ComponentType => this.elementType;

        public override bool IsReadOnly { get; }

        public override Type PropertyType => this.elementType;

        protected int Index { get; }

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