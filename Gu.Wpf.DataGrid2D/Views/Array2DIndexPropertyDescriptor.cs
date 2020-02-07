namespace Gu.Wpf.DataGrid2D
{
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    internal class Array2DIndexPropertyDescriptor : IndexPropertyDescriptor
    {
        private static readonly ConditionalWeakTable<Array, PropertyDescriptorCollection> RowDescriptorCache = new ConditionalWeakTable<Array, PropertyDescriptorCollection>();
        private static readonly ConditionalWeakTable<Array, PropertyDescriptorCollection> ColumnDescriptorCache = new ConditionalWeakTable<Array, PropertyDescriptorCollection>();

        private Array2DIndexPropertyDescriptor(Type elementType, int index)
            : base(elementType, index, isReadOnly: false)
        {
        }

        public override object? GetValue(object component)
        {
            return component switch
            {
                Array2DRowView { Source: { IsTransposed: true, Source: Array array }, Index: var index }
                => array.GetValue(this.Index, index),
                Array2DRowView { Source: { IsTransposed: false, Source: Array array }, Index: var index }
                => array.GetValue(index, this.Index),
                _ => throw new InvalidOperationException("Error getting value."),
            };
        }

        public override void SetValue(object component, object? value)
        {
            switch (component)
            {
                case Array2DRowView { Source: { IsTransposed: true, Source: Array array }, Index: var index }:
                    array.SetValue(value, this.Index, index);
                    break;
                case Array2DRowView { Source: { IsTransposed: false, Source: Array array }, Index: var index }:
                    array.SetValue(value, index, this.Index);
                    break;
                default:
                    throw new InvalidOperationException("Error setting value.");
            }
        }

        internal static PropertyDescriptorCollection GetRowPropertyDescriptorCollection(Array2DView source)
        {
            return RowDescriptorCache.GetValue((Array)source.Source, CreateRowPropertyDescriptorCollection);
        }

        internal static PropertyDescriptorCollection GetColumnPropertyDescriptorCollection(Array2DView source)
        {
            return ColumnDescriptorCache.GetValue((Array)source.Source, CreateColumnPropertyDescriptorCollection);
        }

        private static PropertyDescriptorCollection CreateRowPropertyDescriptorCollection(Array source)
        {
            var elementType = source.GetType().GetElementType()!;
            var n = source.GetLength(1);
            var descriptors = new Array2DIndexPropertyDescriptor[n];
            for (int i = 0; i < n; i++)
            {
                descriptors[i] = new Array2DIndexPropertyDescriptor(elementType, i);
            }

            // ReSharper disable once CoVariantArrayConversion
            return new PropertyDescriptorCollection(descriptors);
        }

        private static PropertyDescriptorCollection CreateColumnPropertyDescriptorCollection(Array source)
        {
            var elementType = source.GetType().GetElementType()!;
            var n = source.GetLength(0);
            var descriptors = new Array2DIndexPropertyDescriptor[n];
            for (int i = 0; i < n; i++)
            {
                descriptors[i] = new Array2DIndexPropertyDescriptor(elementType, i);
            }

            // ReSharper disable once CoVariantArrayConversion
            return new PropertyDescriptorCollection(descriptors);
        }
    }
}
