namespace Gu.Wpf.DataGrid2D
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.Linq;

    public class Lists2DTransposedView : Lists2DViewBase
    {
        public Lists2DTransposedView(IEnumerable<IEnumerable> source)
            : base(source)
        {
            this.MaxColumnCount = source.Count();
            var rowCount = source.Max(x => x.Count());
            this.ColumnIsReadOnlies = source.Select(x => x.Count() != rowCount)
                                       .ToList();
            this.ColumnElementTypes = this.Source.Select(x => x.GetElementType())
                                     .ToList();

            this.ResetRows();
        }

        public override bool IsTransposed => true;

        internal IReadOnlyList<Type> ColumnElementTypes { get; }

        internal IReadOnlyList<bool> ColumnIsReadOnlies { get; }

        internal int MaxColumnCount { get; }

        public override bool ReceiveWeakEvent(Type managerType, object sender, EventArgs e)
        {
            if (managerType != typeof(CollectionChangedEventManager))
            {
                return false;
            }

            var ccea = (NotifyCollectionChangedEventArgs)e;
            if (this.IsColumnsChange(sender, ccea))
            {
                this.OnColumnsChanged();
                return true;
            }

            base.ReceiveWeakEvent(managerType, sender, e);
            if (ReferenceEquals(sender, this.Source))
            {
                switch (ccea.Action)
                {
                    case NotifyCollectionChangedAction.Add:
                    case NotifyCollectionChangedAction.Remove:
                        throw new InvalidOperationException("Should never get here, this is a columns change.");
                    case NotifyCollectionChangedAction.Replace:
                        foreach (var row in this.Rows)
                        {
                            row.RaiseColumnsChanged(ccea.NewStartingIndex, 1);
                        }

                        break;
                    case NotifyCollectionChangedAction.Move:
                        foreach (var row in this.Rows)
                        {
                            row.RaiseColumnsChanged(ccea.OldStartingIndex, 1);
                            row.RaiseColumnsChanged(ccea.NewStartingIndex, 1);
                        }

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
                var col = this.Source.IndexOf(changed);
                switch (ccea.Action)
                {
                    case NotifyCollectionChangedAction.Add:
                        for (var i = changed.Count() - ccea.NewItems.Count; i < this.Rows.Count; i++)
                        {
                            this.Rows[i].RaiseColumnsChanged(col, 1);
                        }

                        break;
                    case NotifyCollectionChangedAction.Remove:
                        for (var i = changed.Count() - ccea.OldItems.Count; i < this.Rows.Count; i++)
                        {
                            this.Rows[i].RaiseColumnsChanged(col, 1);
                        }

                        break;
                    case NotifyCollectionChangedAction.Replace:
                        this.Rows[ccea.NewStartingIndex].RaiseColumnsChanged(col, 1);
                        break;
                    case NotifyCollectionChangedAction.Move:
                        this.Rows[ccea.NewStartingIndex].RaiseColumnsChanged(col, 1);
                        this.Rows[ccea.OldStartingIndex].RaiseColumnsChanged(col, 1);
                        break;
                    case NotifyCollectionChangedAction.Reset:
                        foreach (var row in this.Rows)
                        {
                            row.RaiseColumnsChanged(col, 1);
                        }

                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            return true;
        }

        protected override ListRowView CreateRow(int index)
        {
            PropertyDescriptorCollection propertyDescriptors = this.Rows.Count == 0
                                      ? ListIndexPropertyDescriptor.GetRowPropertyDescriptorCollection(this.ColumnElementTypes, this.ColumnIsReadOnlies, this.MaxColumnCount)
                                      : this.Rows[0].GetProperties();

            return new ListRowView(this, index, propertyDescriptors);
        }

        private bool IsColumnsChange(object sender, NotifyCollectionChangedEventArgs e)
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
                    throw new ArgumentOutOfRangeException();
            }

            if (ReferenceEquals(sender, this.Source))
            {
                return true;
            }

            var index = 0;
            foreach (var row in this.Source)
            {
                var count = row.Count();
                if (count == this.Rows.Count && this.ColumnIsReadOnlies[index] == true)
                {
                    return true;
                }

                if (count != this.Rows.Count && this.ColumnIsReadOnlies[index] == false)
                {
                    return true;
                }

                index++;
            }

            return false;
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

            var rowCount = source.Max(x => x.Count());
            for (var i = 0; i < rowCount; i++)
            {
                var listRowView = this.CreateRow(i);
                this.Rows.Add(listRowView);
            }

            this.OnPropertyChanged(CountPropertyChangedEventArgs);
            this.OnPropertyChanged(IndexerPropertyChangedEventArgs);
            this.OnCollectionChanged(NotifyCollectionResetEventArgs);
        }
    }
}