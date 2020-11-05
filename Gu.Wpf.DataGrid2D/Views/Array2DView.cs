namespace Gu.Wpf.DataGrid2D
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// A bindable representation for <see cref="Array2DView"/>.
    /// </summary>
#pragma warning disable CA1010 // Collections should implement generic interface WPF needs only IList
    public class Array2DView : IList, IView2D
#pragma warning restore CA1010 // Collections should implement generic interface
    {
        private readonly WeakReference source = new WeakReference(null);
        private readonly Array2DRowView[] rows;

        private Array2DView(Array source, bool isTransposed)
        {
            this.IsTransposed = isTransposed;
            this.source.Target = source;
            if (isTransposed)
            {
                this.rows = new Array2DRowView[source.GetLength(1)];
                for (var i = 0; i < source.GetLength(1); i++)
                {
                    this.rows[i] = Array2DRowView.CreateForColumn(this, i);
                }
            }
            else
            {
                this.rows = new Array2DRowView[source.GetLength(0)];
                for (var i = 0; i < source.GetLength(0); i++)
                {
                    this.rows[i] = Array2DRowView.CreateForRow(this, i);
                }
            }
        }

        /// <inheritdoc />
        public int Count => this.rows.Length;

        /// <inheritdoc />
        public bool IsReadOnly => false;

        /// <inheritdoc />
        public bool IsFixedSize => true;

#pragma warning disable CA1033 // Interface methods should be callable by child types

        /// <inheritdoc/>
        object ICollection.SyncRoot => this.Array?.SyncRoot ?? new object();

        /// <inheritdoc/>
        bool ICollection.IsSynchronized => this.Array?.IsSynchronized ?? false;

#pragma warning restore CA1033 // Interface methods should be callable by child types

        /// <inheritdoc />
        public IEnumerable? Source => this.Array;

        /// <inheritdoc/>
        public bool IsTransposed { get; }

        private Array? Array => (Array?)this.source.Target;

        /// <summary>
        /// Gets the row at <paramref name="index"/>.
        /// </summary>
        /// <param name="index">The index.</param>
        public Array2DRowView this[int index] => this.rows[index];

        /// <inheritdoc/>
        object? IList.this[int index]
        {
            get => this[index];
            //// ReSharper disable once ValueParameterNotUsed
            set => throw new NotSupportedException();
        }

        public static Array2DView Create(Array source)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return new Array2DView(source, isTransposed: false);
        }

        public static Array2DView CreateTransposed(Array source)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return new Array2DView(source, isTransposed: true);
        }

        public IEnumerator<Array2DRowView> GetEnumerator() => ((IList<Array2DRowView>)this.rows).GetEnumerator();

#pragma warning disable CA1033 // Interface methods should be callable by child types

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

        /// <inheritdoc/>
        void ICollection.CopyTo(Array array, int index) => this.rows.CopyTo(array, index);

        /// <inheritdoc/>
        int IList.Add(object? value) => throw new NotSupportedException();

        /// <inheritdoc/>
        bool IList.Contains(object? value) => this.rows.Contains(value);

        /// <inheritdoc/>
        void IList.Clear() => throw new NotSupportedException();

        // ReSharper disable once CoVariantArrayConversion
        /// <inheritdoc/>
        int IList.IndexOf(object? value) => Array.IndexOf(this.rows, value);

        /// <inheritdoc/>
        void IList.Insert(int index, object? value) => throw new NotSupportedException();

        /// <inheritdoc/>
        void IList.Remove(object? value) => throw new NotSupportedException();

        /// <inheritdoc/>
        void IList.RemoveAt(int index) => throw new NotSupportedException();

#pragma warning restore CA1033 // Interface methods should be callable by child types
    }
}
