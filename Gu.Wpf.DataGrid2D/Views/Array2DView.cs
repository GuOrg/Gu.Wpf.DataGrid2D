namespace Gu.Wpf.DataGrid2D
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Controls;

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

        public int Count => this.rows.Length;

        public bool IsReadOnly => false;

        public bool IsFixedSize => true;

        object ICollection.SyncRoot => ((Array)this.source.Target)?.SyncRoot;

        bool ICollection.IsSynchronized => ((Array)this.source.Target)?.IsSynchronized == true;

        public IEnumerable Source => (IEnumerable) this.source.Target;

        public bool IsTransposed { get; }

        public Array2DRowView this[int index] => this.rows[index];

        object IList.this[int index]
        {
            get { return this[index]; }
            set { ThrowNotSupported(); }
        }

        public static Array2DView Create(Array source)
        {
            return new Array2DView(source, false);
        }

        public static Array2DView CreateTransposed(Array source)
        {
            return new Array2DView(source, true);
        }

        public IEnumerator<Array2DRowView> GetEnumerator() => ((IList<Array2DRowView>)this.rows).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

        void ICollection.CopyTo(Array array, int index) => this.rows.CopyTo(array, index);

        int IList.Add(object value) => ThrowNotSupported<int>();

        bool IList.Contains(object value) => this.rows.Contains(value);

        void IList.Clear() => ThrowNotSupported();

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