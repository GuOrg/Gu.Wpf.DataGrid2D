namespace Gu.Wpf.DataGrid2D;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

/// <summary>
/// A transposed source.
/// </summary>
#pragma warning disable CA1010 // Collections should implement generic interface WPF needs only IList
public class TransposedItemsSource : IList, IDisposable, IWeakEventListener, IView2D, IColumnsChanged
#pragma warning restore CA1010 // Collections should implement generic interface
{
    private readonly WeakReference source;
    private readonly IReadOnlyList<TransposedRow> rows;
    private bool disposed;

    /// <summary>
    /// Initializes a new instance of the <see cref="TransposedItemsSource"/> class.
    /// </summary>
    /// <param name="source">The <see cref="IEnumerable"/>.</param>
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

    /// <inheritdoc/>
    public int Count => this.rows.Count;

    /// <inheritdoc/>
    public bool IsReadOnly { get; }

#pragma warning disable CA1033 // Interface methods should be callable by child types

    /// <inheritdoc/>
    bool IList.IsFixedSize => true;

    /// <inheritdoc/>
    object ICollection.SyncRoot => ((ICollection)this.rows).SyncRoot;

    /// <inheritdoc/>
    bool ICollection.IsSynchronized => ((ICollection)this.rows).IsSynchronized;

#pragma warning restore CA1033 // Interface methods should be callable by child types

    /// <inheritdoc/>
    IEnumerable? IView2D.Source => this.Source;

    /// <inheritdoc/>
    public bool IsTransposed => true;

    /// <inheritdoc/>
    DataGrid? IColumnsChanged.DataGrid { get; set; }

    /// <summary>
    /// Gets the source collection.
    /// </summary>
    public IEnumerable<object>? Source => (IEnumerable<object>?)this.source.Target;

    internal PropertyDescriptorCollection Properties { get; }

    /// <summary>
    /// Gets the row at the specified index.
    /// </summary>
    /// <param name="index">The index.</param>
    /// <returns>The <see cref="TransposedRow"/>.</returns>
    public TransposedRow this[int index] => this.rows[index];

    /// <inheritdoc/>
    object? IList.this[int index]
    {
        get => this.rows[index];
        //// ReSharper disable once ValueParameterNotUsed
        set => throw new NotSupportedException();
    }

#pragma warning disable CA1033 // Interface methods should be callable by child types

    /// <inheritdoc/>
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

    /// <summary>
    /// Returns an enumerator that iterates through the <see cref="TransposedItemsSource"/>.
    /// </summary>
    /// <returns>An enumerator for the <see cref="TransposedItemsSource"/>.</returns>
    public IEnumerator<TransposedRow> GetEnumerator() => this.rows.GetEnumerator();

    /// <inheritdoc/>
    IEnumerator IEnumerable.GetEnumerator() => this.rows.GetEnumerator();

    /// <inheritdoc/>
    void ICollection.CopyTo(Array array, int index) => ((IList)this.rows).CopyTo(array, index);

    /// <inheritdoc/>
    int IList.Add(object? value) => throw new NotSupportedException();

    /// <inheritdoc/>
    bool IList.Contains(object? value) => ((IList)this.rows).Contains(value);

    /// <inheritdoc/>
    void IList.Clear() => throw new NotSupportedException();

    /// <inheritdoc/>
    int IList.IndexOf(object? value) => ((IList)this.rows).IndexOf(value);

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
    /// Disposes of an <see cref="TransposedItemsSource"/>.
    /// </summary>
    /// <remarks>
    /// Called from Dispose() with disposing=true, and from the finalizer (~TransposedItemsSource) with disposing=false.
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
                foreach (var inpc in source.OfType<INotifyPropertyChanged>())
                {
                    PropertyChangedEventManager.RemoveListener(inpc, this, string.Empty);
                }
            }
        }
    }

    /// <summary>
    /// Throws <see cref="ObjectDisposedException"/> is this instance is disposed.
    /// </summary>
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
