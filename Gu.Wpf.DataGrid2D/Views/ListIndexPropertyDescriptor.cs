namespace Gu.Wpf.DataGrid2D
{
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Linq;
    using System.Reflection;

    internal class ListIndexPropertyDescriptor : IndexPropertyDescriptor
    {
        private ListIndexPropertyDescriptor(Type elementType, int index)
            : base(elementType, index)
        {
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
                list.SetElementAt(listRowView.Index, value);
            }
            else
            {
                var list = listRowView.Source.ElementAtOrDefault<IEnumerable>(listRowView.Index);
                list.SetElementAt(this.index, value);
            }
        }

        internal static PropertyDescriptorCollection GetRowPropertyDescriptorCollection(Type elementType, int maxColumnCount)
        {
            var descriptors = Enumerable.Range(0, maxColumnCount)
                                        .Select(x => new ListIndexPropertyDescriptor(elementType, x))
                                        .ToArray();
            return new PropertyDescriptorCollection(descriptors);
        }
    }
}