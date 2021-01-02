namespace Gu.Wpf.DataGrid2D
{
    using System;
    using System.Collections.Concurrent;
    using System.ComponentModel;

    internal class Array2DIndexPropertyDescriptor : IndexPropertyDescriptor
    {
        private static readonly ConcurrentDictionary<Key, PropertyDescriptorCollection> RowDescriptorCache = new ConcurrentDictionary<Key, PropertyDescriptorCollection>();
        private static readonly ConcurrentDictionary<Key, PropertyDescriptorCollection> ColumnDescriptorCache = new ConcurrentDictionary<Key, PropertyDescriptorCollection>();

        private Array2DIndexPropertyDescriptor(Type elementType, int index)
            : base(elementType, index, isReadOnly: false)
        {
        }

        /// <inheritdoc/>
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

        /// <inheritdoc/>
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
            if (source.Source is Array array)
            {
                return RowDescriptorCache.GetOrAdd(new Key(array.GetElementType(), array.GetLength(1)), x => Create(x));
            }

            return RowDescriptorCache.GetOrAdd(Key.Empty, _ => new PropertyDescriptorCollection(new PropertyDescriptor[0], readOnly: true));

            static PropertyDescriptorCollection Create(Key key)
            {
                var descriptors = new Array2DIndexPropertyDescriptor[key.Length];
                for (int i = 0; i < descriptors.Length; i++)
                {
                    descriptors[i] = new Array2DIndexPropertyDescriptor(key.ElementType, i);
                }

                // ReSharper disable once CoVariantArrayConversion
                return new PropertyDescriptorCollection(descriptors);
            }
        }

        internal static PropertyDescriptorCollection GetColumnPropertyDescriptorCollection(Array2DView source)
        {
            if (source.Source is Array array)
            {
                return ColumnDescriptorCache.GetOrAdd(new Key(array.GetElementType(), array.GetLength(0)), x => Create(x));
            }

            return ColumnDescriptorCache.GetOrAdd(Key.Empty, _ => new PropertyDescriptorCollection(new PropertyDescriptor[0], readOnly: true));

            static PropertyDescriptorCollection Create(Key key)
            {
                var descriptors = new Array2DIndexPropertyDescriptor[key.Length];
                for (int i = 0; i < descriptors.Length; i++)
                {
                    descriptors[i] = new Array2DIndexPropertyDescriptor(key.ElementType, i);
                }

                // ReSharper disable once CoVariantArrayConversion
                return new PropertyDescriptorCollection(descriptors);
            }
        }

        private struct Key : IEquatable<Key>
        {
            internal static readonly Key Empty = new Key(typeof(int), 0);

            internal readonly Type ElementType;
            internal readonly int Length;

            internal Key(Type elementType, int length)
            {
                this.ElementType = elementType;
                this.Length = length;
            }

            public static bool operator ==(Key left, Key right)
            {
                return left.Equals(right);
            }

            public static bool operator !=(Key left, Key right)
            {
                return !left.Equals(right);
            }

            public bool Equals(Key other)
            {
                return this.ElementType == other.ElementType &&
                       this.Length == other.Length;
            }

            public override bool Equals(object? obj)
            {
                return obj is Key other && this.Equals(other);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    return (this.ElementType.GetHashCode() * 397) ^ this.Length;
                }
            }
        }
    }
}
