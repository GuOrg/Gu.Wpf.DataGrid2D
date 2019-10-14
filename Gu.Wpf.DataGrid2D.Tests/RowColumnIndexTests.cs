namespace Gu.Wpf.DataGrid2D.Tests
{
    using System;
    using NUnit.Framework;

    public static class RowColumnIndexTests
    {
        [TestCase("R1 C2", true, 1, 2)]
        [TestCase("R1C2", true, 1, 2)]
        [TestCase("  R1C2", true, 1, 2)]
        [TestCase("  R1  C2   ", true, 1, 2)]
        [TestCase("  R-1  C2   ", false, -1, -1)]
        [TestCase("  R-1  C-2   ", false, -1, -1)]
        [TestCase("advf", false, -1, -1)]
        public static void Parse(string text, bool expectedSuccess, int row, int col)
        {
            RowColumnIndex result;
            var tryParse = RowColumnIndex.TryParse(text, out result);
            Assert.AreEqual(tryParse, expectedSuccess);
            Assert.AreEqual(row, result.Row);
            Assert.AreEqual(col, result.Column);

            if (expectedSuccess)
            {
                result = RowColumnIndex.Parse(text);
                Assert.AreEqual(row, result.Row);
                Assert.AreEqual(col, result.Column);
            }
            else
            {
                Assert.Throws<FormatException>(() => RowColumnIndex.Parse(text));
            }
        }
    }
}
