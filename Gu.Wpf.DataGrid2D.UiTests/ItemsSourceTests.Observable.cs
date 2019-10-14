namespace Gu.Wpf.DataGrid2D.UiTests
{
    using Gu.Wpf.UiAutomation;
    using NUnit.Framework;

    public static partial class ItemsSourceTests
    {
        public static class Observable
        {
            [Test]
            public static void AutoColumns()
            {
                using (var app = Application.Launch(Info.ExeFileName, "ObservableWindow"))
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
            public static void ExplicitColumns()
            {
                using (var app = Application.Launch(Info.ExeFileName, "ObservableWindow"))
                {
                    var window = app.MainWindow;
                    var dataGrid = window.FindDataGrid("ExplicitColumns");
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
            }

            [Test]
            public static void WithHeaders()
            {
                using (var app = Application.Launch(Info.ExeFileName, "ObservableWindow"))
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

                    Assert.AreEqual(3, dataGrid.RowCount);
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
                using (var app = Application.Launch(Info.ExeFileName, "ObservableWindow"))
                {
                    var window = app.MainWindow;
                    var dataGrid = window.FindDataGrid("AutoColumns");
                    var readOnly = window.FindDataGrid("AutoColumnsReadOnly");
                    Assert.AreEqual("1", dataGrid[0, 0].Value);
                    Assert.AreEqual("2", dataGrid[0, 1].Value);
                    Assert.AreEqual("3", dataGrid[1, 0].Value);
                    Assert.AreEqual("4", dataGrid[1, 1].Value);
                    Assert.AreEqual("5", dataGrid[2, 0].Value);
                    Assert.AreEqual("6", dataGrid[2, 1].Value);
                    Assert.AreEqual("1", readOnly[0, 0].Value);
                    Assert.AreEqual("2", readOnly[0, 1].Value);
                    Assert.AreEqual("3", readOnly[1, 0].Value);
                    Assert.AreEqual("4", readOnly[1, 1].Value);
                    Assert.AreEqual("5", readOnly[2, 0].Value);
                    Assert.AreEqual("6", readOnly[2, 1].Value);

                    dataGrid[0, 0].Value = "10";
                    Keyboard.Type(Key.DOWN);
                    Assert.AreEqual("10", dataGrid[0, 0].Value);
                    Assert.AreEqual("2", dataGrid[0, 1].Value);
                    Assert.AreEqual("3", dataGrid[1, 0].Value);
                    Assert.AreEqual("4", dataGrid[1, 1].Value);
                    Assert.AreEqual("5", dataGrid[2, 0].Value);
                    Assert.AreEqual("6", dataGrid[2, 1].Value);

                    Assert.AreEqual("10", readOnly[0, 0].Value);
                    Assert.AreEqual("2", readOnly[0, 1].Value);
                    Assert.AreEqual("3", readOnly[1, 0].Value);
                    Assert.AreEqual("4", readOnly[1, 1].Value);
                    Assert.AreEqual("5", readOnly[2, 0].Value);
                    Assert.AreEqual("6", readOnly[2, 1].Value);

                    dataGrid[2, 1].Value = "11";
                    Keyboard.Type(Key.UP);
                    Assert.AreEqual("10", dataGrid[0, 0].Value);
                    Assert.AreEqual("2", dataGrid[0, 1].Value);
                    Assert.AreEqual("3", dataGrid[1, 0].Value);
                    Assert.AreEqual("4", dataGrid[1, 1].Value);
                    Assert.AreEqual("5", dataGrid[2, 0].Value);
                    Assert.AreEqual("11", dataGrid[2, 1].Value);

                    Assert.AreEqual("10", readOnly[0, 0].Value);
                    Assert.AreEqual("2", readOnly[0, 1].Value);
                    Assert.AreEqual("3", readOnly[1, 0].Value);
                    Assert.AreEqual("4", readOnly[1, 1].Value);
                    Assert.AreEqual("5", readOnly[2, 0].Value);
                    Assert.AreEqual("11", readOnly[2, 1].Value);
                }
            }
        }
    }
}
