namespace Gu.Wpf.DataGrid2D
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    public class Array2DView : IList, IView2D
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
                for (int i = 0; i < source.GetLength(1); i++)
                {
                    this.rows[i] = Array2DRowView.CreateForColumn(this, i);
                }
            }
            else
            {
                this.rows = new Array2DRowView[source.GetLength(0)];
                for (int i = 0; i < source.GetLength(0); i++)
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

        object ICollection.SyncRoot => this.Array.SyncRoot;

        bool ICollection.IsSynchronized => this.Array.IsSynchronized;

        /// <inheritdoc />
        public IEnumerable Source => this.Array;

        public bool IsTransposed { get; }

        private Array Array => (Array)this.source.Target;

        public Array2DRowView this[int index] => this.rows[index];

        object IList.this[int index]
        {
            get => this[index];
            //// ReSharper disable once ValueParameterNotUsed
            set => ThrowNotSupported();
        }

        public static Array2DView Create(Array source)
        {
            return new Array2DView(source, isTransposed: false);
        }

        public static Array2DView CreateTransposed(Array source)
        {
            return new Array2DView(source, isTransposed: true);
        }

        public IEnumerator<Array2DRowView> GetEnumerator() => ((IList<Array2DRowView>)this.rows).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

        void ICollection.CopyTo(Array array, int index) => this.rows.CopyTo(array, index);

        int IList.Add(object value) => ThrowNotSupported<int>();

        bool IList.Contains(object value) => this.rows.Contains(value);

        void IList.Clear() => ThrowNotSupported();

        // ReSharper disable once CoVariantArrayConversion
        int IList.IndexOf(object value) => Array.IndexOf(this.rows, value);

        void IList.Insert(int index, object value) => ThrowNotSupported();

        void IList.Remove(object value) => ThrowNotSupported();

        void IList.RemoveAt(int index) => ThrowNotSupported();

        private static void ThrowNotSupported()
        {
            throw new NotSupportedException();
        }

        private static T ThrowNotSupported<T>()
        {
            throw new NotSupportedException();
        }
    }
}