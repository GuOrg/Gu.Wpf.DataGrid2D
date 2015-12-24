namespace Gu.Wpf.DataGrid2D.UiTests
{
    using NUnit.Framework;
    using TestStack.White.UIItems;

    public static class AssertDataGrid
    {
        public static void AreEqual<T>(T[,] expected, ListView dataGrid)
        {
            var cols = expected.GetLength(1);
            Assert.AreEqual(cols, dataGrid.Rows[0].Cells.Count);
            var rows = expected.GetLength(0);
            Assert.AreEqual(rows, dataGrid.Rows.Count);

            for (var c = 0; c < cols; c++)
            {
                Assert.AreEqual($"C{c}", dataGrid.Header.Columns[c].Text);
            }

            for (var r = 0; r < rows; r++)
            {
                for (var c = 0; c < cols; c++)
                {
                    var col = dataGrid.Header.Columns[c].Text;
                    var expectedCellValue = expected[r, c].ToString();
                    Assert.AreEqual(expectedCellValue, dataGrid.Cell(col, r).Text);
                }
            }
        }
    }
}