namespace Gu.Wpf.DataGrid2D
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    public class Array2DView : IList
    {
        private readonly Array source;
        private readonly Array2DRowView[] rows;

        public Array2DView(Array source)
        {
            this.source = source;
            this.rows = new Array2DRowView[source.GetLength(0)];
            for (int i = 0; i < source.GetLength(0); i++)
            {
                this.rows[i] = new Array2DRowView(source, i);
            }
        }

        public int Count => this.rows.Length;

        public bool IsReadOnly => false;

        public bool IsFixedSize => true;

        object ICollection.SyncRoot => this.source.SyncRoot;

        bool ICollection.IsSynchronized => this.source.IsSynchronized;

        public Array2DRowView this[int index] => this.rows[index];

        object IList.this[int index]
        {
            get { return this[index]; }
            set { ThrowNotSupported(); }
        }

        public IEnumerator<Array2DRowView> GetEnumerator() => ((IList<Array2DRowView>)this.rows).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

        public void CopyTo(Array array, int index) => this.rows.CopyTo(array,index);

        int IList.Add(object value) => ThrowNotSupported<int>();

        bool IList.Contains(object value) => this.rows.Contains(value);

        void IList.Clear() => ThrowNotSupported();

        int IList.IndexOf(object value) => Array.IndexOf(this.rows, value);

        public void Insert(int index, object value) => ThrowNotSupported();

        public void Remove(object value) => ThrowNotSupported();

        public void RemoveAt(int index) => ThrowNotSupported();

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