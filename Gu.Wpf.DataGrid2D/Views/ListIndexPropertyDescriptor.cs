namespace Gu.Wpf.DataGrid2D
{
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Linq;
    using System.Reflection;

    internal class ListIndexPropertyDescriptor : IndexPropertyDescriptor
    {
        private readonly PropertyInfo indexerProperty;

        private ListIndexPropertyDescriptor(Type elementType,PropertyInfo indexerProperty, int index)
            : base(elementType, index)
        {
            this.indexerProperty = indexerProperty;
        }

        public override object GetValue(object component)
        {
            var listRowView = (ListRowView)component;
            if (listRowView.IsTransposed)
            {
                return listRowView.Source
                                  .ElementAtOrDefault<IEnumerable>(this.index)
                                  .ElementAtOrDefault(listRowView.Index);
            }
            else
            {
                return listRowView.Source
                                  .ElementAtOrDefault<IEnumerable>(listRowView.Index)
                                  .ElementAtOrDefault(this.index);
            }
        }

        public override void SetValue(object component, object value)
        {
            var listRowView = (ListRowView)component;
            if (listRowView.IsTransposed)
            {
                var list = listRowView.Source.ElementAtOrDefault<IEnumerable>(this.index);
                this.indexerProperty.SetValue(list, value, new object[] { listRowView.Index });
            }
            else
            {
                var list = listRowView.Source.ElementAtOrDefault<IEnumerable>(listRowView.Index);
                this.indexerProperty.SetValue(list, value, new object[] { this.index });
            }
        }

        internal static PropertyDescriptorCollection GetRowPropertyDescriptorCollection(Type elementType, PropertyInfo indexerProperty, int maxColumnCount)
        {
            var descriptors = Enumerable.Range(0, maxColumnCount)
                                        .Select(x => new ListIndexPropertyDescriptor(elementType, indexerProperty, x))
                                        .ToArray();
            return new PropertyDescriptorCollection(descriptors);
        }
    }
}