namespace Gu.Wpf.DataGrid2D.Tests.Views
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using NUnit.Framework;

    public partial class Lists2DViewTests
    {
        public class ObservableCollections
        {
            [Test]
            public void Create()
            {
                var ints = new ObservableCollection<ObservableCollection<int>>
                           {
                               new ObservableCollection<int>(new[] {1, 2}),
                               new ObservableCollection<int>(new[] {3, 4}),
                               new ObservableCollection<int>(new[] {5, 6})
                           };
                var view = Lists2DView.Create(ints);

                Assert.AreEqual(3, view.Count);

                Assert.AreEqual(0, view[0].Index);
                Assert.AreEqual(2, view[0].Count);
                Assert.AreEqual(false, view[0].IsTransposed);
                Assert.AreEqual(typeof(int), view[0].GetProperties()[0].ComponentType);
                Assert.AreEqual(1, view[0].GetProperties()[0].GetValue(view[0]));
                Assert.AreEqual(2, view[0].GetProperties()[1].GetValue(view[0]));

                Assert.AreEqual(1, view[1].Index);
                Assert.AreEqual(2, view[1].Count);
                Assert.AreEqual(3, view[1].GetProperties()[0].GetValue(view[1]));
                Assert.AreEqual(4, view[1].GetProperties()[1].GetValue(view[1]));

                Assert.AreEqual(2, view[2].Index);
                Assert.AreEqual(2, view[2].Count);
                Assert.AreEqual(5, view[2].GetProperties()[0].GetValue(view[2]));
                Assert.AreEqual(6, view[2].GetProperties()[1].GetValue(view[2]));
            }

            [Test]
            public void ObservesAndNotifies()
            {
                var ints = new ObservableCollection<ObservableCollection<int>>
                           {
                               new ObservableCollection<int>(new[] {1, 2}),
                               new ObservableCollection<int>(new[] {3, 4}),
                               new ObservableCollection<int>(new[] {5, 6})
                           };
                var view = Lists2DView.Create(ints);

                var expectedPropertyChanges = new List<string>();
                ((INotifyPropertyChanged)ints).PropertyChanged += (_, e) => expectedPropertyChanges.Add(e.PropertyName);
                var actualPropertyChanges = new List<string>();
                view.PropertyChanged += (_, e) => actualPropertyChanges.Add(e.PropertyName);

                var expectedCollectionChanges = new List<NotifyCollectionChangedEventArgs>();
                ints.CollectionChanged += (_, e) => expectedCollectionChanges.Add(e);
                var actualCollectionChanges = new List<NotifyCollectionChangedEventArgs>();
                view.CollectionChanged += (_, e) => actualCollectionChanges.Add(e);
                ints.Add(new ObservableCollection<int>(new[] { 7, 8 }));

                CollectionAssert.AreEqual(expectedPropertyChanges, actualPropertyChanges);
                CollectionAssert.AreEqual(expectedCollectionChanges, actualCollectionChanges, CollectionChangedEventArgsComparer.Default);
                Assert.AreEqual(4, view.Count);

                Assert.AreEqual(0, view[0].Index);
                Assert.AreEqual(2, view[0].Count);
                Assert.AreEqual(false, view[0].IsTransposed);
                Assert.AreEqual(typeof(int), view[0].GetProperties()[0].ComponentType);
                Assert.AreEqual(1, view[0].GetProperties()[0].GetValue(view[0]));
                Assert.AreEqual(2, view[0].GetProperties()[1].GetValue(view[0]));

                Assert.AreEqual(1, view[1].Index);
                Assert.AreEqual(2, view[1].Count);
                Assert.AreEqual(3, view[1].GetProperties()[0].GetValue(view[1]));
                Assert.AreEqual(4, view[1].GetProperties()[1].GetValue(view[1]));

                Assert.AreEqual(2, view[2].Index);
                Assert.AreEqual(2, view[2].Count);
                Assert.AreEqual(5, view[2].GetProperties()[0].GetValue(view[2]));
                Assert.AreEqual(6, view[2].GetProperties()[1].GetValue(view[2]));

                Assert.AreEqual(3, view[3].Index);
                Assert.AreEqual(2, view[3].Count);
                Assert.AreEqual(7, view[3].GetProperties()[0].GetValue(view[3]));
                Assert.AreEqual(8, view[3].GetProperties()[1].GetValue(view[3]));
            }

            [Test]
            public void ObservesAndNotifiesTransposed()
            {
                var ints = new ObservableCollection<ObservableCollection<int>>
                           {
                               new ObservableCollection<int>(new[] {1, 2}),
                               new ObservableCollection<int>(new[] {3, 4}),
                               new ObservableCollection<int>(new[] {5, 6})
                           };
                var view = Lists2DView.Create(ints);
                var expectedPropertyChanges = new List<string>();
                ((INotifyPropertyChanged)ints).PropertyChanged += (_, e) => expectedPropertyChanges.Add(e.PropertyName);
                var actualPropertyChanges = new List<string>();
                view.PropertyChanged += (_, e) => actualPropertyChanges.Add(e.PropertyName);

                var expectedCollectionChanges = new List<NotifyCollectionChangedEventArgs>();
                ints.CollectionChanged += (_, e) => expectedCollectionChanges.Add(e);
                var actualCollectionChanges = new List<NotifyCollectionChangedEventArgs>();
                view.CollectionChanged += (_, e) => actualCollectionChanges.Add(e);
                ints.Add(new ObservableCollection<int>(new[] { 7, 8 }));
                ints[2].Add(7);
                CollectionAssert.AreEqual(expectedPropertyChanges, actualPropertyChanges);
                CollectionAssert.AreEqual(expectedCollectionChanges, actualCollectionChanges, CollectionChangedEventArgsComparer.Default);

                Assert.AreEqual(3, view.Count);

                Assert.AreEqual(0, view[0].Index);
                Assert.AreEqual(2, view[0].Count);
                Assert.AreEqual(false, view[0].IsTransposed);
                Assert.AreEqual(typeof(int), view[0].GetProperties()[0].ComponentType);
                Assert.AreEqual(1, view[0].GetProperties()[0].GetValue(view[0]));
                Assert.AreEqual(2, view[0].GetProperties()[1].GetValue(view[0]));

                Assert.AreEqual(1, view[1].Index);
                Assert.AreEqual(2, view[1].Count);
                Assert.AreEqual(3, view[1].GetProperties()[0].GetValue(view[1]));
                Assert.AreEqual(4, view[1].GetProperties()[1].GetValue(view[1]));

                Assert.AreEqual(2, view[2].Index);
                Assert.AreEqual(2, view[2].Count);
                Assert.AreEqual(5, view[2].GetProperties()[0].GetValue(view[2]));
                Assert.AreEqual(6, view[2].GetProperties()[1].GetValue(view[2]));

                Assert.AreEqual(3, view[2].Index);
                Assert.AreEqual(3, view[2].Count);
                Assert.AreEqual(7, view[2].GetProperties()[0].GetValue(view[2]));
                Assert.AreEqual(8, view[2].GetProperties()[1].GetValue(view[2]));

                ints[1].Add(-3);
                CollectionAssert.AreEqual(expectedPropertyChanges, actualPropertyChanges);
                CollectionAssert.AreEqual(expectedCollectionChanges, actualCollectionChanges, CollectionChangedEventArgsComparer.Default);
            }
        }
    }
}
