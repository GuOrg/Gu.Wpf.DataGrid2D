namespace Gu.Wpf.DataGrid2D
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;

    public class RowView : IReadOnlyList<object>, ICustomTypeDescriptor
    {
        private readonly Array _source;
        private readonly int _rowIndex;

        public RowView(Array source, int rowIndex)
        {
            _source = source;
            _rowIndex = rowIndex;
        }

        public int Count => _source.GetLength(0);

        public object this[int index] => _source.GetValue(_rowIndex, index);

        public IEnumerator<object> GetEnumerator()
        {
            for (int j = 0; j < _source.GetLength(1); j++)
            {
                yield return _source.GetValue(_rowIndex, j);
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

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
            var properties = Enumerable.Range(0, _source.GetLength(0) - 1)
                                       .Select(x => new IndexPropertyDescriptor($"[{x}]", null))
                                       .ToArray();
            return new PropertyDescriptorCollection(properties);
        }

        PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties(Attribute[] attributes)
        {
            var properties = Enumerable.Range(0, _source.GetLength(0) - 1)
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
