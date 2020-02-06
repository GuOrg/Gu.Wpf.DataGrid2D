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

#pragma warning disable CA1010 // Collections should implement generic interface WPF needs only IList
    public class TransposedItemsSource : IList, IDisposable, IWeakEventListener, IView2D, IColumnsChanged
#pragma warning restore CA1010 // Collections should implement generic interface
    {
        private readonly WeakReference source;
        private readonly IReadOnlyList<TransposedRow> rows;
        private bool disposed;

        public TransposedItemsSource(IEnumerable source)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            this.source = new WeakReference(source);
            var type = source.GetElementType();
            this.Properties = TypeDescriptor.GetProperties(type);
            this.rows = this.Properties.OfType<PropertyDescriptor>()
                            .Select(x => new TransposedRow(this, x))
                            .ToList();

            this.IsReadOnly = this.Properties.OfType<PropertyDescriptor>()
                                           .Any(x => !x.IsReadOnly);
            if (source is INotifyCollectionChanged incc)
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
        public event EventHandler? ColumnsChanged;

        public int Count => this.rows.Count;

        public bool IsReadOnly { get; }

        bool IList.IsFixedSize => true;

        object ICollection.SyncRoot => ((ICollection)this.rows).SyncRoot;

        bool ICollection.IsSynchronized => ((ICollection)this.rows).IsSynchronized;

        IEnumerable? IView2D.Source => this.Source;

        public bool IsTransposed => true;

        DataGrid? IColumnsChanged.DataGrid { get; set; }

        public IEnumerable<object>? Source => (IEnumerable<object>?)this.source.Target;

        internal PropertyDescriptorCollection Properties { get; }

        public TransposedRow this[int index] => this.rows[index];

        object? IList.this[int index]
        {
            get => this.rows[index];
            //// ReSharper disable once ValueParameterNotUsed
            set => throw new NotSupportedException();
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

        int IList.Add(object? value) => throw new NotSupportedException();

        bool IList.Contains(object? value) => ((IList)this.rows).Contains(value);

        void IList.Clear() => throw new NotSupportedException();

        int IList.IndexOf(object? value) => ((IList)this.rows).IndexOf(value);

        void IList.Insert(int index, object? value) => throw new NotSupportedException();

        void IList.Remove(object? value) => throw new NotSupportedException();

        void IList.RemoveAt(int index) => throw new NotSupportedException();

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

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
                    foreach (var inpc in source.OfType<INotifyPropertyChanged>())
                    {
                        PropertyChangedEventManager.RemoveListener(inpc, this, string.Empty);
                    }
                }
            }
        }

        protected virtual void ThrowIfDisposed()
        {
            if (this.disposed)
            {
                throw new ObjectDisposedException(this.GetType().FullName);
            }
        }

        private void OnColumnsChanged()
        {
            this.ColumnsChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
