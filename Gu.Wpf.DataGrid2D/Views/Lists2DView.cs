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
    using JetBrains.Annotations;

    public class Lists2DView : IList, INotifyCollectionChanged, INotifyPropertyChanged, IWeakEventListener
    {
        private static readonly NotifyCollectionChangedEventArgs NotifyCollectionResetEventArgs = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset);
        private static readonly PropertyChangedEventArgs CountPropertyChangedEventArgs = new PropertyChangedEventArgs(nameof(Count));
        private static readonly PropertyChangedEventArgs IndexerPropertyChangedEventArgs = new PropertyChangedEventArgs("Item[]");

        [EditorBrowsable(EditorBrowsableState.Never)]
        private readonly WeakReference source = new WeakReference(null);
        private readonly List<ListRowView> rows = new List<ListRowView>();

        private Lists2DView(IEnumerable<IEnumerable> source, bool isTransposed)
        {
            this.IsTransposed = isTransposed;
            this.source.Target = source;
            this.ResetRows();
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

        public event PropertyChangedEventHandler PropertyChanged;

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        public bool IsTransposed { get; }

        public int Count => this.rows.Count;

        bool IList.IsReadOnly => this.Source.IsReadOnly();

        bool IList.IsFixedSize => true;

        object ICollection.SyncRoot => (this.source.Target as ICollection)?.SyncRoot;

        bool ICollection.IsSynchronized => (this.source.Target as ICollection)?.IsSynchronized == true;

        internal IEnumerable<IEnumerable> Source => (IEnumerable<IEnumerable>)this.source.Target;

        public ListRowView this[int index] => this.rows[index];

        object IList.this[int index]
        {
            get { return this[index]; }
            set { ThrowNotSupported(); }
        }

        public static Lists2DView Create(IEnumerable<IEnumerable> source)
        {
            return new Lists2DView(source, false);
        }

        public static Lists2DView CreateTransposed(IEnumerable<IEnumerable> source)
        {
            return new Lists2DView(source, true);
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

        bool IWeakEventListener.ReceiveWeakEvent(Type managerType, object sender, EventArgs e)
        {
            if (managerType != typeof(CollectionChangedEventManager))
            {
                return false;
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

                if (this.IsTransposed)
                {
                    throw new NotSupportedException($"{nameof(Lists2DView)} does not support changing number of columns dynamically yet.");
                }
                else
                {
                    switch (ccea.Action)
                    {
                        case NotifyCollectionChangedAction.Add:
                            this.AddRows(ccea.NewStartingIndex, ccea.NewItems.Count);
                            break;
                        case NotifyCollectionChangedAction.Remove:
                            this.RemoveRows(ccea.OldStartingIndex, ccea.OldItems.Count);
                            break;
                        case NotifyCollectionChangedAction.Replace:
                            this.rows[ccea.NewStartingIndex].RaiseAllChanged();
                            break;
                        case NotifyCollectionChangedAction.Move:
                            this.rows[ccea.OldStartingIndex].RaiseAllChanged();
                            this.rows[ccea.NewStartingIndex].RaiseAllChanged();
                            break;
                        case NotifyCollectionChangedAction.Reset:
                            this.ResetRows();
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }
            else
            {
                if (this.IsTransposed)
                {
                    switch (ccea.Action)
                    {
                        case NotifyCollectionChangedAction.Add:
                            throw new NotImplementedException();
                            break;
                        case NotifyCollectionChangedAction.Remove:
                            throw new NotImplementedException();
                            break;
                        case NotifyCollectionChangedAction.Replace:
                            foreach (var listRowView in this.rows)
                            {
                                this.rows[ccea.NewStartingIndex].RaiseColumnsChanged(ccea.NewStartingIndex, ccea.NewItems.Count);
                            }

                            break;
                        case NotifyCollectionChangedAction.Move:
                            foreach (var listRowView in this.rows)
                            {
                                this.rows[ccea.NewStartingIndex].RaiseColumnsChanged(ccea.OldStartingIndex, ccea.OldItems.Count);
                                this.rows[ccea.NewStartingIndex].RaiseColumnsChanged(ccea.NewStartingIndex, ccea.NewItems.Count);
                            }

                            break;
                        case NotifyCollectionChangedAction.Reset:
                            throw new NotImplementedException();
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    //this.UpdateRows(ccea);
                }
                else
                {
                    throw new NotSupportedException($"{nameof(Lists2DView)} does not support changing number of columns dynamically yet.");
                }
            }

            return true;
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
            var source = this.Source;
            this.rows.Clear();

            if (source == null || !source.Any())
            {
                this.OnCollectionChanged(NotifyCollectionResetEventArgs);
                return;
            }

            if (this.IsTransposed)
            {
                var maxColumnCount = source.Count();
                var rowCount = source.Max(x => x.Count());
                for (int i = 0; i < rowCount; i++)
                {
                    var listRowView = this.CreateRow(i, maxColumnCount);
                    this.rows.Add(listRowView);
                }
            }
            else
            {
                var maxColumnCount = source.Max(x => x.Count());
                for (int index = 0; index < source.Count(); index++)
                {
                    var listRowView = this.CreateRow(index, maxColumnCount);
                    this.rows.Add(listRowView);
                }
            }

            this.OnPropertyChanged(CountPropertyChangedEventArgs);
            this.OnPropertyChanged(IndexerPropertyChangedEventArgs);
            this.OnCollectionChanged(NotifyCollectionResetEventArgs);
        }

        private void AddRows(int newStartingIndex, int count)
        {
            var newItems = new List<ListRowView>();
            var maxColumnCount = this.IsTransposed
                                     ? this.Source.Count()
                                     : this.Source.Max(x => x.Count());

            for (int index = newStartingIndex; index < newStartingIndex + count; index++)
            {
                var listRowView = this.CreateRow(index, maxColumnCount);
                this.rows.Add(listRowView);
                newItems.Add(listRowView);
            }

            this.OnPropertyChanged(CountPropertyChangedEventArgs);
            this.OnPropertyChanged(IndexerPropertyChangedEventArgs);
            this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, newItems, newStartingIndex));
        }

        private void RemoveRows(int oldStartingIndex, int count)
        {
            var oldItems = new List<ListRowView>();
            for (int i = oldStartingIndex; i < oldStartingIndex + count; i++)
            {
                oldItems.Add(this.rows[i]);
                this.rows.RemoveAt(i);
            }

            this.OnPropertyChanged(CountPropertyChangedEventArgs);
            this.OnPropertyChanged(IndexerPropertyChangedEventArgs);
            this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, oldItems, oldStartingIndex));
        }

        private ListRowView CreateRow(int index, int maxColumnCount)
        {
            if (this.IsTransposed)
            {
                PropertyDescriptorCollection propertyDescriptors = null;
                Type elementType = null;
                if (this.rows.Count == 0)
                {
                    var allElementTypes = this.Source.Select(x => x.GetElementType())
                          .Distinct()
                          .ToList();
                    elementType = allElementTypes.Count > 1
                                          ? typeof(object)
                                          : allElementTypes[0];
                    propertyDescriptors = ListIndexPropertyDescriptor.GetRowPropertyDescriptorCollection(elementType, maxColumnCount);
                }
                else
                {
                    elementType = this.rows[0].ElementType;
                    propertyDescriptors = this.rows[0].GetProperties();
                }

                return new ListRowView(this.Source, index, elementType, propertyDescriptors, true);
            }
            else
            {
                var elementType = this.Source.ElementAtOrDefault<IEnumerable>(index).GetElementType();
                var propertyDescriptors = this.rows.FirstOrDefault(x => x.ElementType == elementType)
                                              ?.GetProperties() ??
                                          ListIndexPropertyDescriptor.GetRowPropertyDescriptorCollection(elementType, maxColumnCount);

                return new ListRowView(this.Source, index, elementType, propertyDescriptors, false);
            }
        }

        private void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            this.CollectionChanged?.Invoke(this, e);
        }

        private void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            this.PropertyChanged?.Invoke(this, e);
        }

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}