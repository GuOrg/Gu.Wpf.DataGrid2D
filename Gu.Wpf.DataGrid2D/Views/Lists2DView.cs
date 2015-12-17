namespace Gu.Wpf.DataGrid2D
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;

    public class Lists2DView : IList
    {
        private readonly WeakReference source = new WeakReference(null);
        private readonly List<ListRowView> rows = new List<ListRowView>();

        private Lists2DView(IEnumerable<IEnumerable> source, bool isTransposed)
        {
            this.IsTransposed = isTransposed;
            this.source.Target = source;
            this.ResetRows();
        }

        public bool IsTransposed { get; }

        public int Count => this.rows.Count;

        bool IList.IsReadOnly => false;

        bool IList.IsFixedSize => true;

        object ICollection.SyncRoot => (this.source.Target as ICollection)?.SyncRoot;

        bool ICollection.IsSynchronized => (this.source.Target as ICollection)?.IsSynchronized == true;

        public ListRowView this[int index] => this.rows[index];

        object IList.this[int index]
        {
            get { return this[index]; }
            set { ThrowNotSupported(); }
        }

        public IEnumerator<ListRowView> GetEnumerator() => this.rows.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

        void ICollection.CopyTo(Array array, int index) => ((IList)this.rows).CopyTo(array, index);

        int IList.Add(object value) => ThrowNotSupported<int>();

        bool IList.Contains(object value) => this.rows.Contains(value);

        void IList.Clear() => ThrowNotSupported();

        int IList.IndexOf(object value) => this.rows.IndexOf((ListRowView)value);

        void IList.Insert(int index, object value) => ThrowNotSupported();

        void IList.Remove(object value) => ThrowNotSupported();

        void IList.RemoveAt(int index) => ThrowNotSupported();

        internal static Lists2DView Create(IEnumerable<IEnumerable> source)
        {
            return new Lists2DView(source, false);
        }

        internal static Lists2DView CreateTransposed(IEnumerable<IEnumerable> source)
        {
            return new Lists2DView(source, true);
        }

        private static void ThrowNotSupported()
        {
            throw new NotSupportedException();
        }

        private static T ThrowNotSupported<T>()
        {
            throw new NotSupportedException();
        }

        private void ResetRows()
        {
            var source = (IEnumerable<IEnumerable>)this.source.Target;
            this.rows.Clear();
            if (source == null || !source.Any())
            {
                return;
            }

            if (this.IsTransposed)
            {
                var allElementTypes = source.Select(x => x.GetType()
                                                          .GetElementType())
                                       .Distinct()
                                       .ToList();
                var elementType = allElementTypes.Count > 1
                                      ? typeof(object)
                                      : allElementTypes[0];

                var maxColumnCount = source.Count();

                var propertyDescriptors = ListIndexPropertyDescriptor.GetRowPropertyDescriptorCollection(elementType, null, maxColumnCount);
                for (int i = 0; i < maxColumnCount; i++)
                {
                    var listRowView = new ListRowView(source, i, propertyDescriptors, true);
                    this.rows.Add(listRowView);
                }
            }
            else
            {
                var maxColumnCount = source.Max(x => x.Count());
                var index = 0;
                var tempCache = new List<Tuple<Type, PropertyDescriptorCollection>>();
                foreach (var row in source)
                {
                    var elementType = row.GetType().GetElementType();
                    var propertyDescriptors = tempCache.FirstOrDefault(x => x.Item1 == elementType)?.Item2;
                    if (propertyDescriptors == null)
                    {
                        var indexer = row.GetType()
                                         .GetProperties()
                                         .SingleOrDefault(x => x.GetIndexParameters()
                                                                .Length == 1);
                        propertyDescriptors = ListIndexPropertyDescriptor.GetRowPropertyDescriptorCollection(elementType, indexer, maxColumnCount);
                        tempCache.Add(Tuple.Create(elementType, propertyDescriptors));
                    }

                    var listRowView = new ListRowView(source, index, propertyDescriptors, false);
                    this.rows.Add(listRowView);
                    index++;
                }
            }
        }
    }
}