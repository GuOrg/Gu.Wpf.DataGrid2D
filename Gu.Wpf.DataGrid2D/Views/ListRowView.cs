namespace Gu.Wpf.DataGrid2D
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;

    public class ListRowView : IReadOnlyList<object>, ICustomTypeDescriptor
    {
        private readonly Array source;
        private readonly int rowIndex;

        public ListRowView(Array source, int rowIndex)
        {
            this.source = source;
            this.rowIndex = rowIndex;
        }

        int IReadOnlyCollection<object>.Count => this.source.GetLength(0);

        object IReadOnlyList<object>.this[int index] => this.source.GetValue(this.rowIndex, index);

        IEnumerator<object> IEnumerable<object>.GetEnumerator()
        {
            for (int j = 0; j < this.source.GetLength(1); j++)
            {
                yield return this.source.GetValue(this.rowIndex, j);
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => ((IReadOnlyList<object>)this).GetEnumerator();

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

        PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties()
        {
            throw new NotFiniteNumberException();
            //var properties = Enumerable.Range(0, this.Source.GetLength(0) - 1)
            //                           .Select(x => new IndexPropertyDescriptor($"[{x}]", null))
            //                           .ToArray();
            //return new PropertyDescriptorCollection(properties);
        }

        PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties(Attribute[] attributes)
        {
            throw new NotImplementedException();
            //var properties = Enumerable.Range(0, this.Source.GetLength(0) - 1)
            //                          .Select(x => new IndexPropertyDescriptor($"[{x}]", null))
            //                          .ToArray();
            //return new PropertyDescriptorCollection(properties);
        }

        object ICustomTypeDescriptor.GetPropertyOwner(PropertyDescriptor pd)
        {
            throw new NotImplementedException();
        }
    }
}
