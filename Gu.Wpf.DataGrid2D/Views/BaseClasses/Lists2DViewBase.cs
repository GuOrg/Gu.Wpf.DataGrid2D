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

    /// <summary>
    /// Bindable representation of an <see cref="IEnumerable"/>.
    /// </summary>
#pragma warning disable CA1010 // Collections should implement generic interface WPF needs only IList
    public abstract class Lists2DViewBase : IList, INotifyCollectionChanged, INotifyPropertyChanged, IWeakEventListener, IDisposable, IView2D, IColumnsChanged
#pragma warning restore CA1010 // Collections should implement generic interface
    {
        /// <summary> Cached <see cref="NotifyCollectionChangedEventArgs"/> for <see cref="NotifyCollectionChangedAction.Reset"/>. </summary>
        protected static readonly NotifyCollectionChangedEventArgs NotifyCollectionResetEventArgs = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset);

        /// <summary> Cached <see cref="PropertyChangedEventArgs"/> for Count. </summary>
        protected static readonly PropertyChangedEventArgs CountPropertyChangedEventArgs = new PropertyChangedEventArgs(nameof(Count));

        /// <summary> Cached <see cref="PropertyChangedEventArgs"/> for Item[]. </summary>
        protected static readonly PropertyChangedEventArgs IndexerPropertyChangedEventArgs = new PropertyChangedEventArgs("Item[]");

        [EditorBrowsable(EditorBrowsableState.Never)]
        private readonly WeakReference source = new WeakReference(null);
        private bool disposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="Lists2DViewBase"/> class.
        /// </summary>
        /// <param name="source">The <see cref="IEnumerable{IEnumerable}"/>.</param>
        protected Lists2DViewBase(IEnumerable<IEnumerable> source)
        {
            this.source.Target = source;
            if (source is INotifyCollectionChanged incc)
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
        public event EventHandler? ColumnsChanged;

        /// <inheritdoc />
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <inheritdoc />
        public event NotifyCollectionChangedEventHandler? CollectionChanged;

        /// <inheritdoc />
        public int Count => this.Rows.Count;

#pragma warning disable CA1033 // Interface methods should be callable by child types

        /// <inheritdoc />
        bool IList.IsReadOnly => this.Source?.IsReadOnly() ?? true;

        /// <inheritdoc />
        bool IList.IsFixedSize => true;

        /// <inheritdoc />
        object ICollection.SyncRoot => (this.Source as ICollection)?.SyncRoot ?? new object();

        /// <inheritdoc />
        bool ICollection.IsSynchronized => (this.Source as ICollection)?.IsSynchronized == true;

        /// <inheritdoc />
        IEnumerable? IView2D.Source => this.Source;

#pragma warning restore CA1033 // Interface methods should be callable by child types

        /// <inheritdoc/>
        public abstract bool IsTransposed { get; }

        /// <inheritdoc/>
        DataGrid? IColumnsChanged.DataGrid { get; set; }

        internal IEnumerable<IEnumerable>? Source => (IEnumerable<IEnumerable>?)this.source.Target;

        /// <summary>
        /// Gets the rows.
        /// </summary>
#pragma warning disable CA1002 // Do not expose generic lists
        protected List<ListRowView> Rows { get; } = new List<ListRowView>();
#pragma warning restore CA1002 // Do not expose generic lists

        /// <summary>
        /// Gets the row at <paramref name="index"/>.
        /// </summary>
        /// <param name="index">The index.</param>
        public ListRowView this[int index] => this.Rows[index];

        /// <inheritdoc/>
        object? IList.this[int index]
        {
            get => this[index];
            //// ReSharper disable once ValueParameterNotUsed
            set => throw new NotSupportedException();
        }

        /// <inheritdoc />
        public virtual bool ReceiveWeakEvent(Type managerType, object sender, EventArgs e)
        {
            if (e is null)
            {
                throw new ArgumentNullException(nameof(e));
            }

            var ccea = (NotifyCollectionChangedEventArgs)e;
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

        /// <summary> Get the enumerator. </summary>
        /// <returns>A <see cref="IEnumerator{Array2DRowView}"/>.</returns>
        public IEnumerator<ListRowView> GetEnumerator() => this.Rows.GetEnumerator();

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

#pragma warning disable CA1033 // Interface methods should be callable by child types

        /// <inheritdoc/>
        void ICollection.CopyTo(Array array, int index) => ((IList)this.Rows).CopyTo(array, index);

        /// <inheritdoc/>
        int IList.Add(object? value) => throw new NotSupportedException();

        /// <inheritdoc/>
        bool IList.Contains(object? value) => this.Rows.Contains(value);

        /// <inheritdoc/>
        void IList.Clear() => throw new NotSupportedException();

        /// <inheritdoc/>
        int IList.IndexOf(object? value) => ((IList)this.Rows).IndexOf(value);

        /// <inheritdoc/>
        void IList.Insert(int index, object? value) => throw new NotSupportedException();

        /// <inheritdoc/>
        void IList.Remove(object? value) => throw new NotSupportedException();

        /// <inheritdoc/>
        void IList.RemoveAt(int index) => throw new NotSupportedException();

#pragma warning restore CA1033 // Interface methods should be callable by child types

        /// <inheritdoc/>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Raises a <see cref="CollectionChanged"/> event.
        /// </summary>
        /// <param name="e">The <see cref="NotifyCollectionChangedEventArgs"/>.</param>
        protected void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            this.CollectionChanged?.Invoke(this, e);
        }

        /// <summary>
        /// Raises a <see cref="PropertyChanged"/> event.
        /// </summary>
        /// <param name="e">The <see cref="PropertyChangedEventArgs"/>.</param>
        protected void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            this.PropertyChanged?.Invoke(this, e);
        }

        /// <summary>
        /// Raises a <see cref="PropertyChanged"/> event.
        /// </summary>
        /// <param name="propertyName">The property name.</param>
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Raises a <see cref="ColumnsChanged"/> event.
        /// </summary>
        protected void OnColumnsChanged()
        {
            this.ColumnsChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>Notifies that rows were added.</summary>
        /// <param name="newStartingIndex">The index of the first added row.</param>
        /// <param name="count">The number of added rows.</param>
        protected void AddRows(int newStartingIndex, int count)
        {
            this.ThrowIfDisposed();
            var newItems = new List<ListRowView>();
            for (var index = newStartingIndex; index < newStartingIndex + count; index++)
            {
                var listRowView = this.CreateRow(index);
                this.Rows.Add(listRowView);
                newItems.Add(listRowView);
            }

            this.OnPropertyChanged(CountPropertyChangedEventArgs);
            this.OnPropertyChanged(IndexerPropertyChangedEventArgs);
            this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, newItems, newStartingIndex));
        }

        /// <summary>Create a <see cref="ListRowView"/>.</summary>
        /// <param name="index">The index.</param>
        /// <returns>A <see cref="ListRowView"/>.</returns>
        protected abstract ListRowView CreateRow(int index);

        /// <summary>Notifies that rows were added.</summary>
        /// <param name="oldStartingIndex">The index of the first removed row.</param>
        /// <param name="count">The number of removed rows.</param>
        protected void RemoveRows(int oldStartingIndex, int count)
        {
            this.ThrowIfDisposed();
            var oldItems = new List<ListRowView>();
            for (var i = oldStartingIndex; i < oldStartingIndex + count; i++)
            {
                oldItems.Add(this.Rows[i]);
                this.Rows.RemoveAt(i);
            }

            this.OnPropertyChanged(CountPropertyChangedEventArgs);
            this.OnPropertyChanged(IndexerPropertyChangedEventArgs);
            this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, oldItems, oldStartingIndex));
        }

        /// <summary>
        /// Disposes of an <see cref="Lists2DViewBase"/>.
        /// </summary>
        /// <remarks>
        /// Called from Dispose() with disposing=true, and from the finalizer (~Lists2DViewBase) with disposing=false.
        /// Guidelines:
        /// 1. We may be called more than once: do nothing after the first call.
        /// 2. Avoid throwing exceptions if disposing is false, i.e. if we're being finalized.
        /// </remarks>
        /// <param name="disposing">True if called from Dispose(), false if called from the finalizer.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (this.disposed)
            {
                return;
            }

            this.disposed = true;
            if (disposing)
            {
                if (this.Source is INotifyCollectionChanged incc)
                {
                    CollectionChangedEventManager.RemoveListener(incc, this);
                }

                if (this.Source is IEnumerable source)
                {
                    foreach (var row in source.OfType<INotifyCollectionChanged>())
                    {
                        CollectionChangedEventManager.RemoveListener(row, this);
                    }
                }
            }
        }

        /// <summary>Throws <see cref="ObjectDisposedException"/> is this instance is disposed. </summary>
        protected virtual void ThrowIfDisposed()
        {
            if (this.disposed)
            {
                throw new ObjectDisposedException(this.GetType().FullName);
            }
        }
    }
}
