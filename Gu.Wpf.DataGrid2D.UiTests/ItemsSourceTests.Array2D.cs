namespace Gu.Wpf.DataGrid2D.UiTests
{
    using Gu.Wpf.UiAutomation;
    using NUnit.Framework;

    public static partial class ItemsSourceTests
    {
        public static class Array2D
        {
            [Test]
            public static void AutoColumns()
            {
                using var app = Application.Launch(Info.ExeFileName, "Array2DWindow");
                var window = app.MainWindow;
                var dataGrid = window.FindDataGrid("AutoColumns");

                Assert.AreEqual(3, dataGrid.RowCount);
                Assert.AreEqual(2, dataGrid.ColumnCount);

                var columnHeaders = dataGrid.ColumnHeaders;
                Assert.AreEqual(2, columnHeaders.Count);
                Assert.AreEqual("C0", columnHeaders[0].Text);
                Assert.AreEqual("C1", columnHeaders[1].Text);

                Assert.AreEqual("1", dataGrid[0, 0].Value);
                Assert.AreEqual("2", dataGrid[0, 1].Value);
                Assert.AreEqual("3", dataGrid[1, 0].Value);
                Assert.AreEqual("4", dataGrid[1, 1].Value);
                Assert.AreEqual("5", dataGrid[2, 0].Value);
                Assert.AreEqual("6", dataGrid[2, 1].Value);
            }

            [Test]
            public static void ExplicitColumns()
            {
                using var app = Application.Launch(Info.ExeFileName, "Array2DWindow");
                var window = app.MainWindow;
                var dataGrid = window.FindDataGrid("ExplicitColumns");
                Assert.AreEqual(3, dataGrid.RowCount);
                Assert.AreEqual(2, dataGrid.ColumnCount);

                var columnHeaders = dataGrid.ColumnHeaders;
                Assert.AreEqual(2, columnHeaders.Count);
                Assert.AreEqual("Col 1", columnHeaders[0].Text);
                Assert.AreEqual("Col 2", columnHeaders[1].Text);

                Assert.AreEqual("1", dataGrid[0, 0].Value);
                Assert.AreEqual("2", dataGrid[0, 1].Value);
                Assert.AreEqual("3", dataGrid[1, 0].Value);
                Assert.AreEqual("4", dataGrid[1, 1].Value);
                Assert.AreEqual("5", dataGrid[2, 0].Value);
                Assert.AreEqual("6", dataGrid[2, 1].Value);
            }

            [Test]
            public static void WithHeaders()
            {
                using var app = Application.Launch(Info.ExeFileName, "Array2DWindow");
                var window = app.MainWindow;
                var dataGrid = window.FindDataGrid("WithHeaders");

                var columnHeaders = dataGrid.ColumnHeaders;
                Assert.AreEqual(2, columnHeaders.Count);
                Assert.AreEqual("A", columnHeaders[0].Text);
                Assert.AreEqual("B", columnHeaders[1].Text);

                var rows = dataGrid.Rows;
                Assert.AreEqual(3, rows.Count);
                Assert.AreEqual("1", rows[0].Header.Text);
                Assert.AreEqual("2", rows[1].Header.Text);
                Assert.AreEqual("3", rows[2].Header.Text);

                Assert.AreEqual(3, dataGrid.RowCount);
                Assert.AreEqual("1", dataGrid[0, 0].Value);
                Assert.AreEqual("2", dataGrid[0, 1].Value);
                Assert.AreEqual("3", dataGrid[1, 0].Value);
                Assert.AreEqual("4", dataGrid[1, 1].Value);
                Assert.AreEqual("5", dataGrid[2, 0].Value);
                Assert.AreEqual("6", dataGrid[2, 1].Value);
            }

            [Test]
            public static void ViewUpdatesSource()
            {
                using var app = Application.Launch(Info.ExeFileName, "Array2DWindow");
                var window = app.MainWindow;
                var dataGrid = window.FindDataGrid("AutoColumns");
                var update = window.FindButton("UpdateDataButton");
                var data = window.FindTextBlock("DataTextBox");

                dataGrid[0, 0].Value = "10";
                update.Click();
                Assert.AreEqual("{{10, 2}, {3, 4}, {5, 6}}", data.Text);

                dataGrid[2, 1].Value = "11";
                update.Click();
                Assert.AreEqual("{{10, 2}, {3, 4}, {5, 11}}", data.Text);
            }
        }
    }
}
