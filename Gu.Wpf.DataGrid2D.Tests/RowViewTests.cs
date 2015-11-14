namespace Gu.Wpf.DataGrid2D.Tests
{
    using NUnit.Framework;

    public class RowViewTests
    {
        [Test]
        public void TestName()
        {
            var ints = new[,] { { 1, 2 }, { 3, 4 } };
            var row0 = new RowView(ints, 0);
            CollectionAssert.AreEqual(new[] { 1, 2 }, row0);

            var row1 = new RowView(ints, 1);
            CollectionAssert.AreEqual(new[] { 3, 4 }, row1);
        }
    }
}
