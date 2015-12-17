namespace Gu.Wpf.DataGrid2D
{
    using System;
    using System.ComponentModel;

    public class IndexPropertyDescriptor : PropertyDescriptor
    {
        private readonly Type elementType;
        private readonly Func<object> getter;
        private readonly Action<object> setter;

        public IndexPropertyDescriptor(Type elementType, object[] source, int index)
            : this(elementType, () => source[index], x => source[index] = x, $"C{index}")
        {
        }

        public IndexPropertyDescriptor(Type elementType, Func<object> getter, Action<object> setter, string name)
            : base(name, null)
        {
            this.elementType = elementType;
            this.getter = getter;
            this.setter = setter;
        }

        public override Type ComponentType => this.elementType;

        public override bool IsReadOnly => false;

        public override Type PropertyType => this.elementType;

        public override bool CanResetValue(object component)
        {
            throw new NotImplementedException();
        }

        public override object GetValue(object component)
        {
            return this.getter();
        }

        public override void ResetValue(object component)
        {
            throw new NotImplementedException();
        }

        public override void SetValue(object component, object value)
        {
            this.setter(value);
        }

        public override bool ShouldSerializeValue(object component)
        {
            throw new NotImplementedException();
        }
    }
}
