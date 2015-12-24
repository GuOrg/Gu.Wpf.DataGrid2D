namespace Gu.Wpf.DataGrid2D
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Windows;
    using System.Windows.Controls;
    using JetBrains.Annotations;

    public abstract class Lists2DViewBase : IList, INotifyCollectionChanged, INotifyPropertyChanged, IWeakEventListener, IDisposable, IView2D, IColumnsChanged
    {
        protected static readonly NotifyCollectionChangedEventArgs NotifyCollectionResetEventArgs = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset);
        protected static readonly PropertyChangedEventArgs CountPropertyChangedEventArgs = new PropertyChangedEventArgs(nameof(Count));
        protected static readonly PropertyChangedEventArgs IndexerPropertyChangedEventArgs = new PropertyChangedEventArgs("Item[]");

        [EditorBrowsable(EditorBrowsableState.Never)]
        private readonly WeakReference source = new WeakReference(null);

        protected Lists2DViewBase(IEnumerable<IEnumerable> source)
        {
            this.source.Target = source;

            var incc = source as INotifyCollectionChanged;
            if (incc != null)
            {
                CollectionChangedEventManager.AddListener(incc, this);
            }

            foreach (var row in source.OfType<INotifyCollectionChanged>())
            {
                CollectionChangedEventManager.AddListener(row, this);
            }
        }

        /// <summary>
        /// Not sure how to best handle the situation when the number of columns changes.
        /// Testing to raise this event and refresh the ItemsSource binding in the DataGrid.
        /// Just adding a column would not play nice with explicit columns.
        /// This way will not be ideal for performance if it changes frequently
        /// </summary>
        public event EventHandler ColumnsChanged;

        public event PropertyChangedEventHandler PropertyChanged;

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        public int Count => this.Rows.Count;

        bool IList.IsReadOnly => this.Source.IsReadOnly();

        bool IList.IsFixedSize => true;

        object ICollection.SyncRoot => (this.source.Target as ICollection)?.SyncRoot;

        bool ICollection.IsSynchronized => (this.source.Target as ICollection)?.IsSynchronized == true;

        IEnumerable IView2D.Source => this.Source;

        public abstract bool IsTransposed { get; }

        DataGrid IColumnsChanged.DataGrid { get; set; }

        internal IEnumerable<IEnumerable> Source => (IEnumerable<IEnumerable>)this.source.Target;

        protected List<ListRowView> Rows { get; } = new List<ListRowView>();

        public ListRowView this[int index] => this.Rows[index];

        object IList.this[int index]
        {
            get { return this[index]; }
            set { ThrowNotSupported(); }
        }

        public virtual bool ReceiveWeakEvent(Type managerType, object sender, EventArgs e)
        {
            var ccea = (NotifyCollectionChangedEventArgs) e;
            if (ReferenceEquals(sender, this.Source))
            {
                var oldItems = ccea.OldItems;
                if (oldItems != null)
                {
                    foreach (var incc in oldItems.OfType<INotifyCollectionChanged>())
                    {
                        CollectionChangedEventManager.RemoveListener(incc, this);
                    }
                }

                var newItems = ccea.NewItems;
                if (newItems != null)
                {
                    foreach (var incc in newItems.OfType<INotifyCollectionChanged>())
                    {
                        CollectionChangedEventManager.AddListener(incc, this);
                    }
                }
            }

            return true;
        }

        public void Dispose()
        {
            var incc = this.Source as INotifyCollectionChanged;

            if (incc != null)
            {
                CollectionChangedEventManager.RemoveListener(incc, this);
            }

            foreach (var row in this.Source.OfType<INotifyCollectionChanged>())
            {
                CollectionChangedEventManager.RemoveListener(row, this);
            }
        }

        public IEnumerator<ListRowView> GetEnumerator() => this.Rows.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

        void ICollection.CopyTo(Array array, int index) => ((IList)this.Rows).CopyTo(array, index);

        int IList.Add(object value) => ThrowNotSupported<int>();

        bool IList.Contains(object value) => this.Rows.Contains(value);

        void IList.Clear() => ThrowNotSupported();

        int IList.IndexOf(object value) => this.Rows.IndexOf((ListRowView)value);

        void IList.Insert(int index, object value) => ThrowNotSupported();

        void IList.Remove(object value) => ThrowNotSupported();

        void IList.RemoveAt(int index) => ThrowNotSupported();

        protected void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            this.CollectionChanged?.Invoke(this, e);
        }

        protected void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            this.PropertyChanged?.Invoke(this, e);
        }

        [NotifyPropertyChangedInvocator]
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected void OnColumnsChanged()
        {
            this.ColumnsChanged?.Invoke(this, EventArgs.Empty);
        }

        protected void AddRows(int newStartingIndex, int count)
        {
            var newItems = new List<ListRowView>();
            for (int index = newStartingIndex; index < newStartingIndex + count; index++)
            {
                var listRowView = this.CreateRow(index);
                this.Rows.Add(listRowView);
                newItems.Add(listRowView);
            }

            this.OnPropertyChanged(CountPropertyChangedEventArgs);
            this.OnPropertyChanged(IndexerPropertyChangedEventArgs);
            this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, newItems, newStartingIndex));
        }

        protected abstract ListRowView CreateRow(int index);

        protected void RemoveRows(int oldStartingIndex, int count)
        {
            var oldItems = new List<ListRowView>();
            for (int i = oldStartingIndex; i < oldStartingIndex + count; i++)
            {
                oldItems.Add(this.Rows[i]);
                this.Rows.RemoveAt(i);
            }

            this.OnPropertyChanged(CountPropertyChangedEventArgs);
            this.OnPropertyChanged(IndexerPropertyChangedEventArgs);
            this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, oldItems, oldStartingIndex));
        }

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