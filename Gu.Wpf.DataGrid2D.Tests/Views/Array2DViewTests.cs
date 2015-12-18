namespace Gu.Wpf.DataGrid2D.Tests.Views
{
    using NUnit.Framework;

    public class Array2DViewTests
    {
        [Test]
        public void Create()
        {
            var ints = new[,] { { 1, 2 }, { 3, 4 }, { 5, 6 } };
            var view = Array2DView.Create(ints);

            Assert.AreEqual(3, view.Count);

            Assert.AreEqual(0, view[0].Index);
            Assert.AreEqual(2, view[0].Count);
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

        [TestCase(0, 0, -10)]
        [TestCase(1, 0, -10)]
        [TestCase(2, 0, -10)]
        [TestCase(0, 1, -10)]
        [TestCase(1, 1, -10)]
        [TestCase(2, 1, -10)]
        public void Edit(int r, int c, int value)
        {
            var ints = new[,] { { 1, 2 }, { 3, 4 }, { 5, 6 } };
            var view = Array2DView.Create(ints);
            var row = view[r];
            var property = row.GetProperties()[c];
            property.SetValue(row, value);
            Assert.AreEqual(value, ints[r, c]);
            Assert.AreEqual(value, property.GetValue(row));
        }

        [Test]
        public void CreateTransposed()
        {
            var ints = new[,] { { 1, 2 }, { 3, 4 }, { 5, 6 } };
            var view = Array2DView.CreateTransposed(ints);

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
            var ints = new[,] { { 1, 2 }, { 3, 4 }, { 5, 6 } };
            var view = Array2DView.CreateTransposed(ints);
            var row = view[r];
            var property = row.GetProperties()[c];
            property.SetValue(row, value);
            Assert.AreEqual(value, ints[c, r]);
            Assert.AreEqual(value, property.GetValue(row));
        }
    }
}
