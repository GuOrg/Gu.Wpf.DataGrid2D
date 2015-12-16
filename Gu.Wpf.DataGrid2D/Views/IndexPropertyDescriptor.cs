namespace Gu.Wpf.DataGrid2D
{
    using System;
    using System.ComponentModel;

    public class IndexPropertyDescriptor : PropertyDescriptor
    {
        private readonly Type componentType;

        public IndexPropertyDescriptor(string name, Attribute[] attrs)
            : base(name, attrs)
        {
        }

        public IndexPropertyDescriptor(MemberDescriptor descr)
            : base(descr)
        {
        }

        public IndexPropertyDescriptor(MemberDescriptor descr, Attribute[] attrs)
            : base(descr, attrs)
        {
        }

        public override Type ComponentType => this.componentType;

        public override bool IsReadOnly => false;

        public override Type PropertyType => typeof(object);

        public override bool CanResetValue(object component)
        {
            throw new NotImplementedException();
        }

        public override object GetValue(object component)
        {
            throw new NotImplementedException();
        }

        public override void ResetValue(object component)
        {
            throw new NotImplementedException();
        }

        public override void SetValue(object component, object value)
        {
            throw new NotImplementedException();
        }

        public override bool ShouldSerializeValue(object component)
        {
            throw new NotImplementedException();
        }
    }
}
