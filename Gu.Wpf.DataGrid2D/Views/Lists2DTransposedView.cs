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
            this.RowCount = source.Max(x => x.Count());
            this.ColumnIsReadOnlies = source.Select(x => x.Count() != this.RowCount)
                                       .ToList();
            this.ColumnElementTypes = this.Source.Select(x => x.GetElementType())
                                     .ToList();

            this.ResetRows();
        }

        internal IReadOnlyList<Type> ColumnElementTypes { get; }

        internal IReadOnlyList<bool> ColumnIsReadOnlies { get; }

        internal int MaxColumnCount { get; }

        internal int RowCount { get; }

        public override bool IsTransposed => true;

        public override bool ReceiveWeakEvent(Type managerType, object sender, EventArgs e)
        {
            if (managerType != typeof(CollectionChangedEventManager))
            {
                return false;
            }

            base.ReceiveWeakEvent(managerType, sender, e);
            if (ReferenceEquals(sender, this.Source))
            {
                this.OnColumnsChanged();
                return true;
            }

            var ccea = (NotifyCollectionChangedEventArgs)e;
            var changed = (IEnumerable) sender;
            switch (ccea.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    throw new NotImplementedException();
                    break;
                case NotifyCollectionChangedAction.Remove:
                    if (this.ColumnIsReadOnlies[changed.Count() - 1] == false)
                    {
                        this.OnColumnsChanged();
                        return true;
                    }


                    throw new NotImplementedException();
                    break;
                case NotifyCollectionChangedAction.Replace:
                    foreach (var listRowView in this.Rows)
                    {
                        this.Rows[ccea.NewStartingIndex].RaiseColumnsChanged(ccea.NewStartingIndex, ccea.NewItems.Count);
                    }

                    break;
                case NotifyCollectionChangedAction.Move:
                    foreach (var listRowView in this.Rows)
                    {
                        this.Rows[ccea.NewStartingIndex].RaiseColumnsChanged(ccea.OldStartingIndex, ccea.OldItems.Count);
                        this.Rows[ccea.NewStartingIndex].RaiseColumnsChanged(ccea.NewStartingIndex, ccea.NewItems.Count);
                    }

                    break;
                case NotifyCollectionChangedAction.Reset:
                    this.ResetRows();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
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

            var rowCount = source.Max(x => x.Count());
            for (int i = 0; i < rowCount; i++)
            {
                var listRowView = this.CreateRow(i);
                this.Rows.Add(listRowView);
            }

            this.OnPropertyChanged(CountPropertyChangedEventArgs);
            this.OnPropertyChanged(IndexerPropertyChangedEventArgs);
            this.OnCollectionChanged(NotifyCollectionResetEventArgs);
        }

        protected override ListRowView CreateRow(int index)
        {
            PropertyDescriptorCollection propertyDescriptors = null;
            if (this.Rows.Count == 0)
            {
                propertyDescriptors = ListIndexPropertyDescriptor.GetRowPropertyDescriptorCollection(this.ColumnElementTypes, this.ColumnIsReadOnlies, this.MaxColumnCount);
            }
            else
            {
                propertyDescriptors = this.Rows[0].GetProperties();
            }

            return new ListRowView(this, index, propertyDescriptors);
        }
    }
}