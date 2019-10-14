namespace Gu.Wpf.DataGrid2D.UiTests
{
    using Gu.Wpf.UiAutomation;
    using NUnit.Framework;

    public static partial class ItemsSourceTests
    {
        public static class Jagged
        {
            [Test]
            public static void AutoColumns()
            {
                using (var app = Application.Launch(Info.ExeFileName, "JaggedWindow"))
                {
                    var window = app.MainWindow;
                    var dataGrid = window.FindDataGrid("AutoColumns");

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
            }

            [Test]
            public static void DifferentLengths()
            {
                using (var app = Application.Launch(Info.ExeFileName, "JaggedWindow"))
                {
                    var window = app.MainWindow;
                    var dataGrid = window.FindDataGrid("AutoColumnsDifferentLengths");

                    Assert.AreEqual(3, dataGrid.Rows[0].Cells.Count);
                    Assert.AreEqual(3, dataGrid.Rows.Count);

                    var columnHeaders = dataGrid.ColumnHeaders;
                    Assert.AreEqual(3, columnHeaders.Count);
                    Assert.AreEqual("C0", columnHeaders[0].Text);
                    Assert.AreEqual("C1", columnHeaders[1].Text);
                    Assert.AreEqual("C2", columnHeaders[2].Text);

                    Assert.AreEqual("1", dataGrid[0, 0].Value);
                    Assert.AreEqual(string.Empty, dataGrid[0, 1].Value);
                    Assert.AreEqual(string.Empty, dataGrid[0, 2].Value);
                    Assert.AreEqual("2", dataGrid[1, 0].Value);
                    Assert.AreEqual("3", dataGrid[1, 1].Value);
                    Assert.AreEqual(string.Empty, dataGrid[1, 2].Value);
                    Assert.AreEqual("4", dataGrid[2, 0].Value);
                    Assert.AreEqual("5", dataGrid[2, 1].Value);
                    Assert.AreEqual("6", dataGrid[2, 2].Value);
                }
            }

            [Test]
            public static void ExplicitColumns()
            {
                using (var app = Application.Launch(Info.ExeFileName, "JaggedWindow"))
                {
                    var window = app.MainWindow;
                    var dataGrid = window.FindDataGrid("ExplicitColumns");

                    var columnHeaders = dataGrid.ColumnHeaders;
                    Assert.AreEqual(2, columnHeaders.Count);
                    Assert.AreEqual("Col 1", columnHeaders[0].Text);
                    Assert.AreEqual("Col 2", columnHeaders[1].Text);

                    Assert.AreEqual(3, dataGrid.Rows.Count);
                    Assert.AreEqual("1", dataGrid[0, 0].Value);
                    Assert.AreEqual("2", dataGrid[0, 1].Value);
                    Assert.AreEqual("3", dataGrid[1, 0].Value);
                    Assert.AreEqual("4", dataGrid[1, 1].Value);
                    Assert.AreEqual("5", dataGrid[2, 0].Value);
                    Assert.AreEqual("6", dataGrid[2, 1].Value);
                }
            }

            [Test]
            public static void WithHeaders()
            {
                using (var app = Application.Launch(Info.ExeFileName, "JaggedWindow"))
                {
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

                    Assert.AreEqual("1", dataGrid[0, 0].Value);
                    Assert.AreEqual("2", dataGrid[0, 1].Value);
                    Assert.AreEqual("3", dataGrid[1, 0].Value);
                    Assert.AreEqual("4", dataGrid[1, 1].Value);
                    Assert.AreEqual("5", dataGrid[2, 0].Value);
                    Assert.AreEqual("6", dataGrid[2, 1].Value);
                }
            }

            [Test]
            public static void ViewUpdatesSource()
            {
                using (var app = Application.Launch(Info.ExeFileName, "JaggedWindow"))
                {
                    var window = app.MainWindow;
                    var dataGrid = window.FindDataGrid("AutoColumns");
                    var update = window.FindButton("UpdateDataButton");
                    var data = window.FindTextBlock("DataTextBox");

                    var cell = dataGrid.Rows[0].Cells[0];
                    cell.Click();
                    cell.Enter("10");

                    update.Click();
                    Assert.AreEqual("{{10, 2}, {3, 4}, {5, 6}}", data.Text);

                    cell = dataGrid.Rows[2].Cells[1];
                    cell.Click();
                    cell.Enter("11");

                    update.Click();
                    Assert.AreEqual("{{10, 2}, {3, 4}, {5, 11}}", data.Text);
                }
            }

            [Test]
            public static void ToggleRowHeaders()
            {
                using (var app = Application.Launch(Info.ExeFileName, "JaggedWindow"))
                {
                    var window = app.MainWindow;
                    var dataGrid = window.FindDataGrid("WithHeaders");
                    var useHeadersButton = window.FindButton("Use row headers");
                    var nullHeadersButton = window.FindButton("Null row headers");

                    var columnHeaders = dataGrid.ColumnHeaders;
                    Assert.AreEqual(2, columnHeaders.Count);
                    Assert.AreEqual("A", columnHeaders[0].Text);
                    Assert.AreEqual("B", columnHeaders[1].Text);

                    var rows = dataGrid.Rows;
                    Assert.AreEqual(3, rows.Count);
                    Assert.AreEqual("1", rows[0].Header.Text);
                    Assert.AreEqual("2", rows[1].Header.Text);
                    Assert.AreEqual("3", rows[2].Header.Text);

                    Assert.AreEqual("1", dataGrid[0, 0].Value);
                    Assert.AreEqual("2", dataGrid[0, 1].Value);
                    Assert.AreEqual("3", dataGrid[1, 0].Value);
                    Assert.AreEqual("4", dataGrid[1, 1].Value);
                    Assert.AreEqual("5", dataGrid[2, 0].Value);
                    Assert.AreEqual("6", dataGrid[2, 1].Value);

                    nullHeadersButton.Invoke();
                    columnHeaders = dataGrid.ColumnHeaders;
                    Assert.AreEqual(2, columnHeaders.Count);
                    Assert.AreEqual("A", columnHeaders[0].Text);
                    Assert.AreEqual("B", columnHeaders[1].Text);

                    rows = dataGrid.Rows;
                    Assert.AreEqual(3, rows.Count);
                    Assert.AreEqual(string.Empty, rows[0].Header.Text);
                    Assert.AreEqual(string.Empty, rows[1].Header.Text);
                    Assert.AreEqual(string.Empty, rows[2].Header.Text);

                    Assert.AreEqual("1", dataGrid[0, 0].Value);
                    Assert.AreEqual("2", dataGrid[0, 1].Value);
                    Assert.AreEqual("3", dataGrid[1, 0].Value);
                    Assert.AreEqual("4", dataGrid[1, 1].Value);
                    Assert.AreEqual("5", dataGrid[2, 0].Value);
                    Assert.AreEqual("6", dataGrid[2, 1].Value);

                    useHeadersButton.Invoke();
                    columnHeaders = dataGrid.ColumnHeaders;
                    Assert.AreEqual(2, columnHeaders.Count);
                    Assert.AreEqual("A", columnHeaders[0].Text);
                    Assert.AreEqual("B", columnHeaders[1].Text);

                    rows = dataGrid.Rows;
                    Assert.AreEqual(3, rows.Count);
                    Assert.AreEqual("1", rows[0].Header.Text);
                    Assert.AreEqual("2", rows[1].Header.Text);
                    Assert.AreEqual("3", rows[2].Header.Text);

                    Assert.AreEqual("1", dataGrid[0, 0].Value);
                    Assert.AreEqual("2", dataGrid[0, 1].Value);
                    Assert.AreEqual("3", dataGrid[1, 0].Value);
                    Assert.AreEqual("4", dataGrid[1, 1].Value);
                    Assert.AreEqual("5", dataGrid[2, 0].Value);
                    Assert.AreEqual("6", dataGrid[2, 1].Value);
                }
            }

            [Test]
            public static void ToggleColumnHeaders()
            {
                using (var app = Application.Launch(Info.ExeFileName, "JaggedWindow"))
                {
                    var window = app.MainWindow;
                    var dataGrid = window.FindDataGrid("WithHeaders");
                    var useHeadersButton = window.FindButton("Use column headers");
                    var nullHeadersButton = window.FindButton("Null column headers");

                    var columnHeaders = dataGrid.ColumnHeaders;
                    Assert.AreEqual(2, columnHeaders.Count);
                    Assert.AreEqual("A", columnHeaders[0].Text);
                    Assert.AreEqual("B", columnHeaders[1].Text);

                    var rows = dataGrid.Rows;
                    Assert.AreEqual(3, rows.Count);
                    Assert.AreEqual("1", rows[0].Header.Text);
                    Assert.AreEqual("2", rows[1].Header.Text);
                    Assert.AreEqual("3", rows[2].Header.Text);

                    Assert.AreEqual("1", dataGrid[0, 0].Value);
                    Assert.AreEqual("2", dataGrid[0, 1].Value);
                    Assert.AreEqual("3", dataGrid[1, 0].Value);
                    Assert.AreEqual("4", dataGrid[1, 1].Value);
                    Assert.AreEqual("5", dataGrid[2, 0].Value);
                    Assert.AreEqual("6", dataGrid[2, 1].Value);

                    nullHeadersButton.Invoke();
                    columnHeaders = dataGrid.ColumnHeaders;
                    Assert.AreEqual(2, columnHeaders.Count);
                    Assert.AreEqual(string.Empty, columnHeaders[0].Text);
                    Assert.AreEqual(string.Empty, columnHeaders[1].Text);

                    rows = dataGrid.Rows;
                    Assert.AreEqual(3, rows.Count);
                    Assert.AreEqual("1", rows[0].Header.Text);
                    Assert.AreEqual("2", rows[1].Header.Text);
                    Assert.AreEqual("3", rows[2].Header.Text);

                    Assert.AreEqual("1", dataGrid[0, 0].Value);
                    Assert.AreEqual("2", dataGrid[0, 1].Value);
                    Assert.AreEqual("3", dataGrid[1, 0].Value);
                    Assert.AreEqual("4", dataGrid[1, 1].Value);
                    Assert.AreEqual("5", dataGrid[2, 0].Value);
                    Assert.AreEqual("6", dataGrid[2, 1].Value);

                    useHeadersButton.Invoke();
                    columnHeaders = dataGrid.ColumnHeaders;
                    Assert.AreEqual(2, columnHeaders.Count);
                    Assert.AreEqual("A", columnHeaders[0].Text);
                    Assert.AreEqual("B", columnHeaders[1].Text);

                    rows = dataGrid.Rows;
                    Assert.AreEqual(3, rows.Count);
                    Assert.AreEqual("1", rows[0].Header.Text);
                    Assert.AreEqual("2", rows[1].Header.Text);
                    Assert.AreEqual("3", rows[2].Header.Text);

                    Assert.AreEqual("1", dataGrid[0, 0].Value);
                    Assert.AreEqual("2", dataGrid[0, 1].Value);
                    Assert.AreEqual("3", dataGrid[1, 0].Value);
                    Assert.AreEqual("4", dataGrid[1, 1].Value);
                    Assert.AreEqual("5", dataGrid[2, 0].Value);
                    Assert.AreEqual("6", dataGrid[2, 1].Value);
                }
            }
        }
    }
}
