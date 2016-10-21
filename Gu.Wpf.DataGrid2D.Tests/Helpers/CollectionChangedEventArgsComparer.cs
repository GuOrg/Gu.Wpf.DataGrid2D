namespace Gu.Wpf.DataGrid2D.Tests
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.Specialized;

    public class CollectionChangedEventArgsComparer : IComparer, IComparer<NotifyCollectionChangedEventArgs>
    {
        public static readonly CollectionChangedEventArgsComparer Default = new CollectionChangedEventArgsComparer();

        private CollectionChangedEventArgsComparer()
        {
        }

        public int Compare(NotifyCollectionChangedEventArgs x, NotifyCollectionChangedEventArgs y)
        {
            if (x.Action != y.Action)
            {
                return -1;
            }

            if (x.NewStartingIndex != y.NewStartingIndex)
            {
                return -1;
            }

            if (x.NewItems?.Count != y.NewItems?.Count)
            {
                return -1;
            }

            if (x.OldStartingIndex != y.OldStartingIndex)
            {
                return -1;
            }

            if (x.OldItems?.Count != y.OldItems?.Count)
            {
                return -1;
            }

            return 0;
        }

        int IComparer.Compare(object x, object y)
        {
            return this.Compare((NotifyCollectionChangedEventArgs)x, (NotifyCollectionChangedEventArgs)y);
        }
    }
}
