// ReSharper disable PossibleMultipleEnumeration
namespace Gu.Wpf.DataGrid2D;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

/// <summary>
/// A bindable view of a list.
/// </summary>
#pragma warning disable CA1010 // Collections should implement generic interface WPF needs only IList
public class Lists2DView : Lists2DViewBase
#pragma warning restore CA1010 // Collections should implement generic interface
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Lists2DView"/> class.
    /// </summary>
    /// <param name="source">The <see cref="IEnumerable{IEnumerable}"/>.</param>
    public Lists2DView(IEnumerable<IEnumerable> source)
        : base(source)
    {
        if (source is null)
        {
            throw new ArgumentNullException(nameof(source));
        }

        bool isEmpty = source.IsEmpty();
        this.MaxColumnCount = isEmpty ? 0 : source.Max(x => x.Count());
        var itemType = source.GetElementType().GetEnumerableItemType();
        this.ColumnElementTypes = Enumerable.Repeat(itemType, this.MaxColumnCount).ToList();
        var min = isEmpty ? 0 : this.Source.Min(x => x.Count());
        this.ColumnIsReadOnlies = Enumerable.Repeat(false, min)
                                            .Concat(Enumerable.Repeat(true, this.MaxColumnCount - min))
                                            .ToList();
        this.ResetRows();
    }

    /// <inheritdoc/>
    public override bool IsTransposed => false;

    internal IReadOnlyList<Type> ColumnElementTypes { get; }

    internal IReadOnlyList<bool> ColumnIsReadOnlies { get; }

    internal int MaxColumnCount { get; }

    /// <inheritdoc/>
    public override bool ReceiveWeakEvent(Type managerType, object sender, EventArgs e)
    {
        if (sender is null)
        {
            throw new ArgumentNullException(nameof(sender));
        }

        if (e is null)
        {
            throw new ArgumentNullException(nameof(e));
        }

        if (managerType != typeof(CollectionChangedEventManager))
        {
            return false;
        }

        var ccea = (NotifyCollectionChangedEventArgs)e;

        if (this.IsColumnsChange(ccea))
        {
            this.OnColumnsChanged();
            return true;
        }

        _ = base.ReceiveWeakEvent(managerType, sender, e);
        if (ReferenceEquals(sender, this.Source))
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
                    this.Rows[ccea.NewStartingIndex].RaiseAllChanged();
                    break;
                case NotifyCollectionChangedAction.Move:
                    this.Rows[ccea.OldStartingIndex].RaiseAllChanged();
                    this.Rows[ccea.NewStartingIndex].RaiseAllChanged();
                    break;
                case NotifyCollectionChangedAction.Reset:
                    this.ResetRows();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(e));
            }
        }
        else if (this.Source is { } source)
        {
            var changed = (IEnumerable)sender;
            var row = source.IndexOf(changed);
            switch (ccea.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    this.Rows[row].RaiseColumnsChanged(changed.Count() - ccea.NewItems.Count, ccea.NewItems.Count);
                    break;
                case NotifyCollectionChangedAction.Remove:
                    if (!changed.IsEmpty())
                    {
                        this.Rows[row].RaiseColumnsChanged(changed.Count() - ccea.OldItems.Count, ccea.OldItems.Count);
                    }

                    break;
                case NotifyCollectionChangedAction.Replace:
                    this.Rows[row].RaiseColumnsChanged(ccea.NewStartingIndex, 1);
                    break;
                case NotifyCollectionChangedAction.Move:
                    this.Rows[row].RaiseColumnsChanged(ccea.OldStartingIndex, 1);
                    this.Rows[row].RaiseColumnsChanged(ccea.NewStartingIndex, 1);
                    break;
                case NotifyCollectionChangedAction.Reset:
                    this.Rows[row].RaiseColumnsChanged(0, this.MaxColumnCount);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(e));
            }
        }

        return true;
    }

    /// <inheritdoc/>
    protected override ListRowView CreateRow(int index)
    {
        var propertyDescriptors = this.Rows.Count > 0
                                      ? this.Rows[0].GetProperties()
                                      : ListIndexPropertyDescriptor.GetRowPropertyDescriptorCollection(this.ColumnElementTypes, this.ColumnIsReadOnlies, this.MaxColumnCount);

        return new ListRowView(this, index, propertyDescriptors);
    }

    // ReSharper disable once UnusedParameter.Local
    private bool IsColumnsChange(NotifyCollectionChangedEventArgs e)
    {
        switch (e.Action)
        {
            case NotifyCollectionChangedAction.Add:
            case NotifyCollectionChangedAction.Remove:
            case NotifyCollectionChangedAction.Reset:
                break;
            case NotifyCollectionChangedAction.Replace:
            case NotifyCollectionChangedAction.Move:
                return false;
            default:
                throw new ArgumentOutOfRangeException(nameof(e));
        }

        if (this.Source is { } source)
        {
            var min = this.MaxColumnCount;
            var max = 0;
            foreach (var row in source)
            {
                var count = row.Count();
                if (count > max)
                {
                    max = count;
                }

                if (count < min)
                {
                    min = count;
                }
            }

            if (max != this.MaxColumnCount)
            {
                return true;
            }

            var readOnlies = this.ColumnIsReadOnlies.Count(x => x == false);
            if (min != readOnlies)
            {
                return true;
            }
        }

        return false;
    }

    private void ResetRows()
    {
        var source = this.Source;
        this.Rows.Clear();

        if (source is null || !source.Any())
        {
            this.OnCollectionChanged(NotifyCollectionResetEventArgs);
            return;
        }

        for (var index = 0; index < source.Count(); index++)
        {
            var listRowView = this.CreateRow(index);
            this.Rows.Add(listRowView);
        }

        this.OnPropertyChanged(CountPropertyChangedEventArgs);
        this.OnPropertyChanged(IndexerPropertyChangedEventArgs);
        this.OnCollectionChanged(NotifyCollectionResetEventArgs);
    }
}
