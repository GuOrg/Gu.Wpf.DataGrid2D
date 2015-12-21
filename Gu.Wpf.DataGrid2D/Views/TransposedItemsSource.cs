namespace Gu.Wpf.DataGrid2D
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;

    public class TransposedItemsSource : IList, IDisposable, IWeakEventListener, IView2D
    {
        private readonly WeakReference source;
        private readonly IReadOnlyList<TransposedRow> rows;
        private bool disposed;

        public TransposedItemsSource(IEnumerable source)
        {
            this.source = new WeakReference(source);
            var type = source.GetElementType();
            this.Properties = TypeDescriptor.GetProperties(type);
            this.rows = this.Properties.OfType<PropertyDescriptor>()
                            .Select(x => new TransposedRow(this, x))
                            .ToList();

            this.IsReadOnly = this.Properties.OfType<PropertyDescriptor>()
                                           .Any(x => !x.IsReadOnly);
            var incc = source as INotifyCollectionChanged;
            if (incc != null)
            {
                CollectionChangedEventManager.AddListener(incc, this);
            }

            foreach (var inpc in this.Source.OfType<INotifyPropertyChanged>())
            {
                PropertyChangedEventManager.AddListener(inpc, this, string.Empty);
            }
        }

        /// <summary>
        /// Not sure how to best handle the situation when the number of columns changes.
        /// Testing to raise this event and refresh the ItemsSource binding in the DataGrid.
        /// Just adding a column would not play nice with explicit columns.
        /// This way will not be ideal for performance if it changes frequently
        /// </summary>
        public event EventHandler ColumnsChanged;

        public int Count => this.rows.Count;

        public bool IsReadOnly { get; }

        bool IList.IsFixedSize => true;

        object ICollection.SyncRoot => ((ICollection)this.rows).SyncRoot;

        bool ICollection.IsSynchronized => ((ICollection)this.rows).IsSynchronized;

        IEnumerable IView2D.Source => Source;

        DataGrid IView2D.DataGrid { get; set; }

        public IEnumerable<object> Source => (IEnumerable<object>)this.source.Target;

        internal PropertyDescriptorCollection Properties { get; }

        public TransposedRow this[int index] => this.rows[index];

        object IList.this[int index]
        {
            get { return this.rows[index]; }
            set { ThrowNotSupported(); }
        }

        bool IWeakEventListener.ReceiveWeakEvent(Type managerType, object sender, EventArgs e)
        {
            if (managerType == typeof(CollectionChangedEventManager))
            {
                this.OnColumnsChanged();
                return true;
            }

            if (managerType == typeof(PropertyChangedEventManager))
            {
                var propertyChangedEventArgs = (PropertyChangedEventArgs)e;
                var row = this.rows.Single(x => x.Property.Name == propertyChangedEventArgs.PropertyName);
                row.RaiseColumnPropertyChanged(sender);
                return true;
            }

            return false;
        }

        public IEnumerator<TransposedRow> GetEnumerator() => this.rows.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => this.rows.GetEnumerator();

        void ICollection.CopyTo(Array array, int index) => ((IList)this.rows).CopyTo(array, index);

        int IList.Add(object value) => ThrowNotSupported<int>();

        bool IList.Contains(object value) => ((IList)this.rows).Contains(value);

        void IList.Clear() => ThrowNotSupported();

        int IList.IndexOf(object value) => ((IList)this.rows).IndexOf(value);

        void IList.Insert(int index, object value) => ThrowNotSupported();

        void IList.Remove(object value) => ThrowNotSupported();

        void IList.RemoveAt(int index) => ThrowNotSupported();

        public void Dispose()
        {
            if (this.disposed)
            {
                return;
            }

            this.disposed = true;
            var incc = this.Source as INotifyCollectionChanged;
            if (incc != null)
            {
                CollectionChangedEventManager.RemoveListener(incc, this);
            }

            foreach (var inpc in this.Source.OfType<INotifyPropertyChanged>())
            {
                PropertyChangedEventManager.RemoveListener(inpc, this, string.Empty);
            }
        }

        private static void ThrowNotSupported()
        {
            throw new NotSupportedException();
        }

        private static T ThrowNotSupported<T>()
        {
            throw new NotSupportedException();
        }

        private void OnColumnsChanged()
        {
            this.ColumnsChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
