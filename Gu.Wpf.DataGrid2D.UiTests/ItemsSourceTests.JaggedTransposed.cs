namespace Gu.Wpf.DataGrid2D.UiTests
{
    using Gu.Wpf.UiAutomation;
    using NUnit.Framework;

    public partial class ItemsSourceTests
    {
        public class JaggedTransposed
        {
            [Test]
            public void AutoColumns()
            {
                using (var app = Application.Launch(Info.ExeFileName, "JaggedWindow"))
                {
                    var window = app.MainWindow;
                    var dataGrid = window.FindDataGrid("AutoColumnsTransposed");

                    var columnHeaders = dataGrid.ColumnHeaders;
                    Assert.AreEqual(3, columnHeaders.Count);
                    Assert.AreEqual("C0", columnHeaders[0].Text);
                    Assert.AreEqual("C1", columnHeaders[1].Text);
                    Assert.AreEqual("C2", columnHeaders[2].Text);

                    Assert.AreEqual(2, dataGrid.Rows.Count);
                    Assert.AreEqual("1", dataGrid[0, 0].Value);
                    Assert.AreEqual("3", dataGrid[0, 1].Value);
                    Assert.AreEqual("5", dataGrid[0, 2].Value);
                    Assert.AreEqual("2", dataGrid[1, 0].Value);
                    Assert.AreEqual("4", dataGrid[1, 1].Value);
                    Assert.AreEqual("6", dataGrid[1, 2].Value);
                }
            }

            [Test]
            public void AutoColumnsDifferentLengths()
            {
                using (var app = Application.Launch(Info.ExeFileName, "JaggedWindow"))
                {
                    var window = app.MainWindow;
                    var tabItem = window.FindTabItem("int[][]");
                    tabItem.Click();
                    var dataGrid = tabItem.FindDataGrid("DifferentLengthsTransposed");

                    Assert.AreEqual(3, dataGrid.ColumnCount);
                    Assert.AreEqual(3, dataGrid.RowCount);

                    var columnHeaders = dataGrid.ColumnHeaders;
                    Assert.AreEqual(3, columnHeaders.Count);
                    Assert.AreEqual("C0", columnHeaders[0].Text);
                    Assert.AreEqual("C1", columnHeaders[1].Text);
                    Assert.AreEqual("C2", columnHeaders[2].Text);

                    Assert.AreEqual("1", dataGrid[0, 0].Value);
                    Assert.AreEqual("2", dataGrid[0, 1].Value);
                    Assert.AreEqual("4", dataGrid[0, 2].Value);
                    Assert.AreEqual(string.Empty, dataGrid[1, 0].Value);
                    Assert.AreEqual("3", dataGrid[1, 1].Value);
                    Assert.AreEqual("5", dataGrid[1, 2].Value);
                    Assert.AreEqual(string.Empty, dataGrid[2, 0].Value);
                    Assert.AreEqual(string.Empty, dataGrid[2, 1].Value);
                    Assert.AreEqual("6", dataGrid[2, 2].Value);
                }
            }

            [Test]
            public void ExplicitColumns()
            {
                using (var app = Application.Launch(Info.ExeFileName, "JaggedWindow"))
                {
                    var window = app.MainWindow;
                    var dataGrid = window.FindDataGrid("ExplicitColumnsTransposed");

                    var columnHeaders = dataGrid.ColumnHeaders;
                    Assert.AreEqual(3, columnHeaders.Count);
                    Assert.AreEqual("Col 1", columnHeaders[0].Text);
                    Assert.AreEqual("Col 2", columnHeaders[1].Text);
                    Assert.AreEqual("Col 3", columnHeaders[2].Text);

                    Assert.AreEqual(2, dataGrid.Rows.Count);
                    Assert.AreEqual("1", dataGrid[0, 0].Value);
                    Assert.AreEqual("3", dataGrid[0, 1].Value);
                    Assert.AreEqual("5", dataGrid[0, 2].Value);
                    Assert.AreEqual("2", dataGrid[1, 0].Value);
                    Assert.AreEqual("4", dataGrid[1, 1].Value);
                    Assert.AreEqual("6", dataGrid[1, 2].Value);
                }
            }

            [Test]
            public void WithHeaders()
            {
                using (var app = Application.Launch(Info.ExeFileName, "JaggedWindow"))
                {
                    var window = app.MainWindow;
                    var dataGrid = window.FindDataGrid("WithHeadersTransposed");

                    Assert.AreEqual(3, dataGrid.ColumnCount);
                    Assert.AreEqual(2, dataGrid.Rows.Count);

                    var columnHeaders = dataGrid.ColumnHeaders;
                    Assert.AreEqual(3, columnHeaders.Count);
                    Assert.AreEqual("A", columnHeaders[0].Text);
                    Assert.AreEqual("B", columnHeaders[1].Text);
                    Assert.AreEqual("C", columnHeaders[2].Text);

                    var rows = dataGrid.Rows;
                    Assert.AreEqual(2, rows.Count);
                    Assert.AreEqual("1", rows[0].Header.Text);
                    Assert.AreEqual("2", rows[1].Header.Text);

                    Assert.AreEqual(2, dataGrid.Rows.Count);
                    Assert.AreEqual("1", dataGrid[0, 0].Value);
                    Assert.AreEqual("3", dataGrid[0, 1].Value);
                    Assert.AreEqual("5", dataGrid[0, 2].Value);
                    Assert.AreEqual("2", dataGrid[1, 0].Value);
                    Assert.AreEqual("4", dataGrid[1, 1].Value);
                    Assert.AreEqual("6", dataGrid[1, 2].Value);
                }
            }

            [Test]
            public void ViewUpdatesSource()
            {
                using (var app = Application.Launch(Info.ExeFileName, "JaggedWindow"))
                {
                    var window = app.MainWindow;
                    var dataGrid = window.FindDataGrid("AutoColumnsTransposed");
                    var update = window.FindButton("UpdateDataButton");
                    var data = window.FindTextBlock("DataTextBox");

                    var cell = dataGrid.Rows[0].Cells[0];
                    cell.Click();
                    cell.Enter("10");

                    update.Click();
                    Assert.AreEqual("{{10, 2}, {3, 4}, {5, 6}}", data.Text);

                    cell = dataGrid.Rows[1].Cells[2];
                    cell.Click();
                    cell.Enter("11");

                    update.Click();
                    Assert.AreEqual("{{10, 2}, {3, 4}, {5, 11}}", data.Text);
                }
            }
        }
    }
}
