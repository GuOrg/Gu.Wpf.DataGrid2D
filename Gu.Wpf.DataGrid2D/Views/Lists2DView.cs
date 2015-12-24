namespace Gu.Wpf.DataGrid2D
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Linq;

    public class Lists2DView : Lists2DViewBase
    {
        public Lists2DView(IEnumerable<IEnumerable> source)
            : base(source)
        {
            this.MaxColumnCount = source.Max(x => x.Count());
            this.ColumnElementTypes = Enumerable.Repeat(source.ElementAt(0).GetElementType(), this.MaxColumnCount).ToList();
            var min = source.Min(x => x.Count());
            this.ColumnIsReadOnlies = Enumerable.Repeat(false, min)
                                           .Concat(Enumerable.Repeat(true, this.MaxColumnCount - min))
                                           .ToList();
            this.ResetRows();
        }

        internal IReadOnlyList<Type> ColumnElementTypes { get; }

        internal IReadOnlyList<bool> ColumnIsReadOnlies { get; }

        internal int MaxColumnCount { get; }

        public override bool IsTransposed => false;

        public override bool ReceiveWeakEvent(Type managerType, object sender, EventArgs e)
        {
            if (managerType != typeof(CollectionChangedEventManager))
            {
                return false;
            }

            base.ReceiveWeakEvent(managerType, sender, e);
            var ccea = (NotifyCollectionChangedEventArgs)e;
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
                        throw new ArgumentOutOfRangeException();
                }
            }
            else
            {
                var changed = (IEnumerable)sender;

                var row = this.Source.IndexOf(changed);
                switch (ccea.Action)
                {
                    case NotifyCollectionChangedAction.Add:
                        {
                            var count = changed.Count();
                            if (changed.Count() > this.MaxColumnCount)
                            {
                                this.OnColumnsChanged();
                                return true;
                            }

                            this.Rows[row].RaiseColumnsChanged(count - ccea.NewItems.Count, ccea.NewItems.Count);
                            break;
                        }

                    case NotifyCollectionChangedAction.Remove:
                        {
                            var count = changed.Count();
                            if (this.ColumnIsReadOnlies[count - 1] == false)
                            {
                                this.OnColumnsChanged();
                                return true;
                            }

                            this.Rows[row].RaiseColumnsChanged(count - ccea.OldItems.Count, ccea.OldItems.Count);
                            break;
                        }
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
                        throw new ArgumentOutOfRangeException();
                }
            }

            return true;
        }

        private void ResetRows()
        {
            var source = this.Source;
            this.Rows.Clear();

            if (source == null || !source.Any())
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

        protected override ListRowView CreateRow(int index)
        {
            var propertyDescriptors = this.Rows.Count > 0
                                          ? this.Rows[0].GetProperties()
                                          : ListIndexPropertyDescriptor.GetRowPropertyDescriptorCollection(this.ColumnElementTypes, this.ColumnIsReadOnlies, this.MaxColumnCount);

            return new ListRowView(this, index, propertyDescriptors);
        }
    }
}