namespace Gu.Wpf.DataGrid2D.Tests.Views
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using NUnit.Framework;

    public partial class Lists2DViewTests
    {
        [Test]
        public void CreateFromArrays()
        {
            var ints = new[] { new[] { 1, 2 }, new[] { 3, 4 }, new[] { 5, 6 } };
            var view = new Lists2DView(ints);

            Assert.AreEqual(3, view.Count);

            Assert.AreEqual(0, view[0].Index);
            Assert.AreEqual(2, view[0].Count);
            Assert.AreEqual(typeof(int), view[0].GetProperties()[0].ComponentType);
            Assert.AreEqual(false, view[0].GetProperties()[0].IsReadOnly);
            Assert.AreEqual(1, view[0].GetProperties()[0].GetValue(view[0]));
            Assert.AreEqual(false, view[0].GetProperties()[1].IsReadOnly);
            Assert.AreEqual(2, view[0].GetProperties()[1].GetValue(view[0]));

            Assert.AreEqual(1, view[1].Index);
            Assert.AreEqual(2, view[1].Count);
            Assert.AreEqual(false, view[1].GetProperties()[0].IsReadOnly);
            Assert.AreEqual(3, view[1].GetProperties()[0].GetValue(view[1]));
            Assert.AreEqual(false, view[1].GetProperties()[1].IsReadOnly);
            Assert.AreEqual(4, view[1].GetProperties()[1].GetValue(view[1]));

            Assert.AreEqual(2, view[2].Index);
            Assert.AreEqual(2, view[2].Count);
            Assert.AreEqual(false, view[2].GetProperties()[0].IsReadOnly);
            Assert.AreEqual(5, view[2].GetProperties()[0].GetValue(view[2]));
            Assert.AreEqual(false, view[2].GetProperties()[1].IsReadOnly);
            Assert.AreEqual(6, view[2].GetProperties()[1].GetValue(view[2]));
        }

        [Test]
        public void CreateFromObservableCollections()
        {
            var ints = new ObservableCollection<ObservableCollection<int>>
                           {
                               new ObservableCollection<int>(new[] { 1, 2 }),
                               new ObservableCollection<int>(new[] { 3, 4 }),
                               new ObservableCollection<int>(new[] { 5, 6 })
                           };
            var view = new Lists2DView(ints);

            Assert.AreEqual(3, view.Count);

            var row0 = view[0];
            Assert.AreEqual(0, row0.Index);
            Assert.AreEqual(2, row0.Count);
            Assert.AreEqual(typeof(int), row0.GetProperties()[0].ComponentType);
            Assert.AreEqual(1, row0.GetProperties()[0].GetValue(row0));
            Assert.AreEqual(typeof(int), row0.GetProperties()[0].ComponentType);
            Assert.AreEqual(2, row0.GetProperties()[1].GetValue(row0));

            Assert.AreEqual(1, view[1].Index);
            Assert.AreEqual(2, view[1].Count);
            Assert.AreEqual(3, view[1].GetProperties()[0].GetValue(view[1]));
            Assert.AreEqual(4, view[1].GetProperties()[1].GetValue(view[1]));

            Assert.AreEqual(2, view[2].Index);
            Assert.AreEqual(2, view[2].Count);
            Assert.AreEqual(5, view[2].GetProperties()[0].GetValue(view[2]));
            Assert.AreEqual(6, view[2].GetProperties()[1].GetValue(view[2]));
        }

        [TestCase(0, 0, -10)]
        [TestCase(1, 0, -10)]
        [TestCase(2, 0, -10)]
        [TestCase(0, 1, -10)]
        [TestCase(1, 1, -10)]
        [TestCase(2, 1, -10)]
        public void Edit(int r, int c, int value)
        {
            var ints = new[] { new[] { 1, 2 }, new[] { 3, 4 }, new[] { 5, 6 } };
            var view = new Lists2DView(ints);
            var row = view[r];
            var property = row.GetProperties()[c];
            property.SetValue(row, value);
            Assert.AreEqual(value, ints[r][c]);
            Assert.AreEqual(value, property.GetValue(row));
        }

        [Test]
        public void ObservesAndNotifies()
        {
            var ints = new ObservableCollection<ObservableCollection<int>>
                           {
                               new ObservableCollection<int>(new[] { 1, 2 }),
                               new ObservableCollection<int>(new[] { 3, 4 }),
                               new ObservableCollection<int>(new[] { 5, 6 })
                           };
            var view = new Lists2DView(ints);

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
        public void ColumnsReadOnly()
        {
            var ints = new[] { new[] { 1, 2 }, new[] { 3, 4 }, new[] { 5, 6 } };
            var view = new Lists2DView(ints);
            Assert.AreEqual(false, view[0].GetProperties()[0].IsReadOnly);
            Assert.AreEqual(false, view[0].GetProperties()[1].IsReadOnly);
            Assert.AreEqual(false, view[1].GetProperties()[0].IsReadOnly);
            Assert.AreEqual(false, view[1].GetProperties()[1].IsReadOnly);
            Assert.AreEqual(false, view[2].GetProperties()[0].IsReadOnly);
            Assert.AreEqual(false, view[2].GetProperties()[1].IsReadOnly);

            ints = new[] { new[] { 1 }, new[] { 3, 4 }, new[] { 5, 6 } };
            view = new Lists2DView(ints);
            Assert.AreEqual(false, view[0].GetProperties()[0].IsReadOnly);
            Assert.AreEqual(true, view[0].GetProperties()[1].IsReadOnly);
            Assert.AreEqual(false, view[1].GetProperties()[0].IsReadOnly);
            Assert.AreEqual(true, view[1].GetProperties()[1].IsReadOnly);
            Assert.AreEqual(false, view[2].GetProperties()[0].IsReadOnly);
            Assert.AreEqual(true, view[2].GetProperties()[1].IsReadOnly);

            ints = new[] { new[] { 1 }, new[] { 3 }, new[] { 5, 6 } };
            view = new Lists2DView(ints);
            Assert.AreEqual(false, view[0].GetProperties()[0].IsReadOnly);
            Assert.AreEqual(true, view[0].GetProperties()[1].IsReadOnly);
            Assert.AreEqual(false, view[1].GetProperties()[0].IsReadOnly);
            Assert.AreEqual(true, view[1].GetProperties()[1].IsReadOnly);
            Assert.AreEqual(false, view[2].GetProperties()[0].IsReadOnly);
            Assert.AreEqual(true, view[2].GetProperties()[1].IsReadOnly);
        }
    }
}