namespace Gu.Wpf.DataGrid2D
{
    using System;
    using System.ComponentModel;
    using System.Linq;

    public class Array2DRowView : ICustomTypeDescriptor
    {
        private readonly Array source;
        private readonly int rowIndex;
        private readonly PropertyDescriptorCollection properties;

        public Array2DRowView(Array source, int rowIndex)
        {
            this.source = source;
            this.rowIndex = rowIndex;
            var indexPropertyDescriptors = Enumerable.Range(0, this.source.GetLength(0) - 1)
                                                     .Select(x => new IndexPropertyDescriptor($"[{x}]", null))
                                                     .ToArray();
            this.properties = new PropertyDescriptorCollection(indexPropertyDescriptors);
        }

        //int IReadOnlyCollection<object>.Count => this.source.GetLength(0);

        //object IReadOnlyList<object>.this[int index] => this.source.GetValue(this.rowIndex, index);

        //IEnumerator<object> IEnumerable<object>.GetEnumerator()
        //{
        //    for (int j = 0; j < this.source.GetLength(1); j++)
        //    {
        //        yield return this.source.GetValue(this.rowIndex, j);
        //    }
        //}

        //IEnumerator IEnumerable.GetEnumerator()
        //{
        //    return ((IReadOnlyList<object>)this).GetEnumerator();
        //}

        AttributeCollection ICustomTypeDescriptor.GetAttributes()
        {
            return AttributeCollection.Empty;
        }

        string ICustomTypeDescriptor.GetClassName()
        {
            return nameof(Array2DRowView);
        }

        string ICustomTypeDescriptor.GetComponentName()
        {
            throw new NotImplementedException();
        }

        TypeConverter ICustomTypeDescriptor.GetConverter()
        {
            throw new NotImplementedException();
        }

        EventDescriptor ICustomTypeDescriptor.GetDefaultEvent()
        {
            throw new NotImplementedException();
        }

        PropertyDescriptor ICustomTypeDescriptor.GetDefaultProperty()
        {
            throw new NotImplementedException();
        }

        object ICustomTypeDescriptor.GetEditor(Type editorBaseType)
        {
            throw new NotImplementedException();
        }

        EventDescriptorCollection ICustomTypeDescriptor.GetEvents()
        {
            return EventDescriptorCollection.Empty;
        }

        EventDescriptorCollection ICustomTypeDescriptor.GetEvents(Attribute[] attributes)
        {
            return EventDescriptorCollection.Empty;
        }

        PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties() => this.properties;

        PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties(Attribute[] attributes) => this.properties;

        object ICustomTypeDescriptor.GetPropertyOwner(PropertyDescriptor pd)
        {
            throw new NotImplementedException();
        }

        private static void ThrowNotSupported()
        {
            throw new NotSupportedException();
        }
    }
}