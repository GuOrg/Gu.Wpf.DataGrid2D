namespace Gu.Wpf.DataGrid2D.Tests.Views
{
    using System.Collections.ObjectModel;

    using NUnit.Framework;

    public partial class Lists2DTransposedViewTests
    {
        [Test]
        public void CreateFromArrays()
        {
            var ints = new[] { new[] { 1, 2 }, new[] { 3, 4 }, new[] { 5, 6 } };
            var view = new Lists2DTransposedView(ints);

            Assert.AreEqual(2, view.Count);

            Assert.AreEqual(0, view[0].Index);
            Assert.AreEqual(3, view[0].Count);
            Assert.AreEqual(1, view[0].GetProperties()[0].GetValue(view[0]));
            Assert.AreEqual(3, view[0].GetProperties()[1].GetValue(view[0]));
            Assert.AreEqual(5, view[0].GetProperties()[2].GetValue(view[0]));

            Assert.AreEqual(1, view[1].Index);
            Assert.AreEqual(3, view[1].Count);
            Assert.AreEqual(2, view[1].GetProperties()[0].GetValue(view[1]));
            Assert.AreEqual(4, view[1].GetProperties()[1].GetValue(view[1]));
            Assert.AreEqual(6, view[1].GetProperties()[2].GetValue(view[1]));
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
            var view = new Lists2DTransposedView(ints);

            Assert.AreEqual(2, view.Count);

            Assert.AreEqual(0, view[0].Index);
            Assert.AreEqual(3, view[0].Count);
            Assert.AreEqual(1, view[0].GetProperties()[0].GetValue(view[0]));
            Assert.AreEqual(3, view[0].GetProperties()[1].GetValue(view[0]));
            Assert.AreEqual(5, view[0].GetProperties()[2].GetValue(view[0]));

            Assert.AreEqual(1, view[1].Index);
            Assert.AreEqual(3, view[1].Count);
            Assert.AreEqual(2, view[1].GetProperties()[0].GetValue(view[1]));
            Assert.AreEqual(4, view[1].GetProperties()[1].GetValue(view[1]));
            Assert.AreEqual(6, view[1].GetProperties()[2].GetValue(view[1]));
        }

        [TestCase(0, 0, -10)]
        [TestCase(1, 0, -10)]
        [TestCase(0, 1, -10)]
        [TestCase(1, 1, -10)]
        [TestCase(0, 2, -10)]
        [TestCase(1, 2, -10)]
        public void EditTransposed(int r, int c, int value)
        {
            var ints = new[] { new[] { 1, 2 }, new[] { 3, 4 }, new[] { 5, 6 } };
            var view = new Lists2DTransposedView(ints);
            var row = view[r];
            var property = row.GetProperties()[c];
            property.SetValue(row, value);
            Assert.AreEqual(value, ints[c][r]);
            Assert.AreEqual(value, property.GetValue(row));
        }

        [Test]
        public void ColumnsReadOnly()
        {
            var ints = new[] { new[] { 1, 2 }, new[] { 3, 4 }, new[] { 5, 6 } };
            var view = new Lists2DTransposedView(ints);
            Assert.AreEqual(false, view[0].GetProperties()[0].IsReadOnly);
            Assert.AreEqual(false, view[0].GetProperties()[1].IsReadOnly);
            Assert.AreEqual(false, view[0].GetProperties()[2].IsReadOnly);
            Assert.AreEqual(false, view[1].GetProperties()[0].IsReadOnly);
            Assert.AreEqual(false, view[1].GetProperties()[1].IsReadOnly);
            Assert.AreEqual(false, view[1].GetProperties()[2].IsReadOnly);

            ints = new[] { new[] { 1 }, new[] { 3, 4 }, new[] { 5, 6 } };
            view = new Lists2DTransposedView(ints);
            Assert.AreEqual(true, view[0].GetProperties()[0].IsReadOnly);
            Assert.AreEqual(false, view[0].GetProperties()[1].IsReadOnly);
            Assert.AreEqual(false, view[0].GetProperties()[2].IsReadOnly);
            Assert.AreEqual(true, view[1].GetProperties()[0].IsReadOnly);
            Assert.AreEqual(false, view[1].GetProperties()[1].IsReadOnly);
            Assert.AreEqual(false, view[1].GetProperties()[2].IsReadOnly);

            ints = new[] { new[] { 1 }, new[] { 3 }, new[] { 5, 6 } };
            view = new Lists2DTransposedView(ints);
            Assert.AreEqual(true, view[0].GetProperties()[0].IsReadOnly);
            Assert.AreEqual(true, view[0].GetProperties()[1].IsReadOnly);
            Assert.AreEqual(false, view[0].GetProperties()[2].IsReadOnly);
            Assert.AreEqual(true, view[1].GetProperties()[0].IsReadOnly);
            Assert.AreEqual(true, view[1].GetProperties()[1].IsReadOnly);
            Assert.AreEqual(false, view[1].GetProperties()[2].IsReadOnly);
        }
    }
}