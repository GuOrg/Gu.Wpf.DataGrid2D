namespace Gu.Wpf.DataGrid2D
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Linq;

    internal class ListIndexPropertyDescriptor : IndexPropertyDescriptor
    {
        private ListIndexPropertyDescriptor(Type elementType, int index, bool isreadOnly)
            : base(elementType, index, isreadOnly)
        {
        }

        public override object GetValue(object component)
        {
            var listRowView = (ListRowView)component;
            var lists2DView = listRowView.Source as Lists2DView;
            if (lists2DView != null)
            {
                return lists2DView.Source
                  .ElementAtOrDefault<IEnumerable>(listRowView.Index)
                  .ElementAtOrDefault(this.Index);
            }

            var lists2DTransposedView = listRowView.Source as Lists2DTransposedView;
            if (lists2DTransposedView != null)
            {
                return lists2DTransposedView.Source
                  .ElementAtOrDefault<IEnumerable>(this.Index)
                  .ElementAtOrDefault(listRowView.Index);
            }

            throw new ArgumentOutOfRangeException($"Could not get value from {component.GetType()}");
        }

        public override void SetValue(object component, object value)
        {
            var listRowView = (ListRowView)component;
            var lists2DView = listRowView.Source as Lists2DView;
            if (lists2DView != null)
            {
                var list = lists2DView.Source.ElementAtOrDefault<IEnumerable>(listRowView.Index);
                list.SetElementAt(this.Index, value);
                return;
            }

            var lists2DTransposedView = listRowView.Source as Lists2DTransposedView;
            if (lists2DTransposedView != null)
            {
                var list = lists2DTransposedView.Source.ElementAtOrDefault<IEnumerable>(this.Index);
                list.SetElementAt(listRowView.Index, value);
                return;
            }

            throw new ArgumentOutOfRangeException($"Could not set value for {component.GetType()}");
        }

        internal static PropertyDescriptorCollection GetRowPropertyDescriptorCollection(IReadOnlyList<Type> elementTypes, IReadOnlyList<bool> readOnlies, int maxColumnCount)
        {
            Debug.Assert(elementTypes.Count == maxColumnCount, "elementTypes.Count != maxColumnCount");
            var descriptors = Enumerable.Range(0, maxColumnCount)
                                        .Select(x => new ListIndexPropertyDescriptor(elementTypes[x], x, readOnlies[x]))
                                        .ToArray();
            //// ReSharper disable once CoVariantArrayConversion
            return new PropertyDescriptorCollection(descriptors);
        }
    }
}