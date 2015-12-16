namespace Gu.Wpf.DataGrid2D
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;

    public class RowView : IReadOnlyList<object>, ICustomTypeDescriptor
    {
        private readonly Array source;
        private readonly int rowIndex;

        public RowView(Array source, int rowIndex)
        {
            this.source = source;
            this.rowIndex = rowIndex;
        }

        public int Count => this.source.GetLength(0);

        public object this[int index] => this.source.GetValue(this.rowIndex, index);

        public IEnumerator<object> GetEnumerator()
        {
            for (int j = 0; j < this.source.GetLength(1); j++)
            {
                yield return this.source.GetValue(this.rowIndex, j);
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

        AttributeCollection ICustomTypeDescriptor.GetAttributes()
        {
            return AttributeCollection.Empty;
        }

        string ICustomTypeDescriptor.GetClassName()
        {
            return nameof(RowView);
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

        PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties()
        {
            var properties = Enumerable.Range(0, this.source.GetLength(0) - 1)
                                       .Select(x => new IndexPropertyDescriptor($"[{x}]", null))
                                       .ToArray();
            return new PropertyDescriptorCollection(properties);
        }

        PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties(Attribute[] attributes)
        {
            var properties = Enumerable.Range(0, this.source.GetLength(0) - 1)
                                      .Select(x => new IndexPropertyDescriptor($"[{x}]", null))
                                      .ToArray();
            return new PropertyDescriptorCollection(properties);
        }

        object ICustomTypeDescriptor.GetPropertyOwner(PropertyDescriptor pd)
        {
            throw new NotImplementedException();
        }
    }
}
