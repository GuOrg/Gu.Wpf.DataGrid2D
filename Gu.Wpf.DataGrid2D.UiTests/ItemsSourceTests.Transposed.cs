namespace Gu.Wpf.DataGrid2D.UiTests
{
    using Gu.Wpf.UiAutomation;
    using NUnit.Framework;

    public static partial class ItemsSourceTests
    {
        public static class Transposed
        {
            [Test]
            public static void ExplicitColumns()
            {
                using (var app = Application.Launch(Info.ExeFileName, "TransposedWindow"))
                {
                    var window = app.MainWindow;
                    var dataGrid = window.FindDataGrid("TransposedExplicitColumns");
                    var columnHeaders = dataGrid.ColumnHeaders;
                    Assert.AreEqual(2, columnHeaders.Count);
                    Assert.AreEqual("Name", columnHeaders[0].Text);
                    Assert.AreEqual("Value", columnHeaders[1].Text);

                    Assert.AreEqual(2, dataGrid.Rows.Count);
                    Assert.AreEqual("FirstName", dataGrid[0, 0].Value);
                    Assert.AreEqual("Johan", dataGrid[0, 1].Value);
                    Assert.AreEqual("LastName", dataGrid[1, 0].Value);
                    Assert.AreEqual("Larsson", dataGrid[1, 1].Value);
                }
            }

            [Test]
            public static void Singleton()
            {
                using (var app = Application.Launch(Info.ExeFileName, "TransposedWindow"))
                {
                    var window = app.MainWindow;
                    var dataGrid = window.FindDataGrid("TransposedSingleton");
                    var columnHeaders = dataGrid.ColumnHeaders;
                    Assert.AreEqual(2, columnHeaders.Count);
                    Assert.AreEqual("Name", columnHeaders[0].Text);
                    Assert.AreEqual("C0", columnHeaders[1].Text);

                    Assert.AreEqual(2, dataGrid.Rows.Count);
                    Assert.AreEqual("FirstName", dataGrid[0, 0].Value);
                    Assert.AreEqual("Johan", dataGrid[0, 1].Value);
                    Assert.AreEqual("LastName", dataGrid[1, 0].Value);
                    Assert.AreEqual("Larsson", dataGrid[1, 1].Value);
                }
            }

            [Test]
            public static void ObservableCollection()
            {
                using (var app = Application.Launch(Info.ExeFileName, "TransposedWindow"))
                {
                    var window = app.MainWindow;
                    var dataGrid = window.FindDataGrid("TransposedObservableCollection");

                    var columnHeaders = dataGrid.ColumnHeaders;
                    Assert.AreEqual(3, columnHeaders.Count);
                    Assert.AreEqual("Name", columnHeaders[0].Text);
                    Assert.AreEqual("C0", columnHeaders[1].Text);
                    Assert.AreEqual("C1", columnHeaders[2].Text);

                    Assert.AreEqual(2, dataGrid.Rows.Count);
                    Assert.AreEqual("FirstName", dataGrid[0, 0].Value);
                    Assert.AreEqual("Johan", dataGrid[0, 1].Value);
                    Assert.AreEqual("Erik", dataGrid[0, 2].Value);
                    Assert.AreEqual("LastName", dataGrid[1, 0].Value);
                    Assert.AreEqual("Larsson", dataGrid[1, 1].Value);
                    Assert.AreEqual("Svensson", dataGrid[1, 2].Value);
                }
            }

            [Test]
            public static void ObservableCollectionEditCellInViewUpdatesDataContext()
            {
                using (var app = Application.Launch(Info.ExeFileName, "TransposedWindow"))
                {
                    var window = app.MainWindow;
                    var dataGrid = window.FindDataGrid("TransposedObservableCollection");
                    dataGrid[0, 1].Value = "New Value";
                    Keyboard.Type(Key.DOWN);
                    window.WaitUntilResponsive();

                    var reference = window.FindDataGrid("ReferenceDataGrid");
                    Assert.AreEqual("New Value", reference[0, 0].Value);
                }
            }

            [Test]
            public static void ObservableCollectionEditDataContextUpdatesView()
            {
                using (var app = Application.Launch(Info.ExeFileName, "TransposedWindow"))
                {
                    var window = app.MainWindow;
                    var reference = window.FindDataGrid("ReferenceDataGrid");

                    var dataGrid = window.FindDataGrid("TransposedObservableCollection");
                    Assert.AreEqual("Johan", dataGrid[0, 1].Value);

                    reference[0, 0].Value = "New Value";
                    Keyboard.Type(Key.DOWN);
                    Assert.AreEqual("New Value", dataGrid[0, 1].Value);
                }
            }

            [Test]
            public static void RemoveRowInDataContext()
            {
                using (var app = Application.Launch(Info.ExeFileName, "TransposedWindow"))
                {
                    var window = app.MainWindow;
                    var reference = window.FindDataGrid("ReferenceDataGrid");

                    var dataGrid = window.FindDataGrid("TransposedObservableCollection");
                    var columnHeaders = dataGrid.ColumnHeaders;
                    Assert.AreEqual(3, columnHeaders.Count);
                    Assert.AreEqual("Name", columnHeaders[0].Text);
                    Assert.AreEqual("C0", columnHeaders[1].Text);
                    Assert.AreEqual("C1", columnHeaders[2].Text);

                    Assert.AreEqual(2, dataGrid.RowCount);
                    Assert.AreEqual(3, dataGrid.ColumnCount);
                    Assert.AreEqual("FirstName", dataGrid[0, 0].Value);
                    Assert.AreEqual("Johan", dataGrid[0, 1].Value);
                    Assert.AreEqual("Erik", dataGrid[0, 2].Value);
                    Assert.AreEqual("LastName", dataGrid[1, 0].Value);
                    Assert.AreEqual("Larsson", dataGrid[1, 1].Value);
                    Assert.AreEqual("Svensson", dataGrid[1, 2].Value);

                    reference[1, 0].Click();
                    Keyboard.Type(Key.DELETE);
                    Keyboard.Type(Key.UP);
                    Assert.AreEqual(2, dataGrid.RowCount);
                    Assert.AreEqual(2, dataGrid.ColumnCount);
                    Assert.AreEqual("FirstName", dataGrid[0, 0].Value);
                    Assert.AreEqual("Johan", dataGrid[0, 1].Value);
                    Assert.AreEqual("LastName", dataGrid[1, 0].Value);
                    Assert.AreEqual("Larsson", dataGrid[1, 1].Value);
                }
            }

            [Test]
            public static void AddRowInDataContext()
            {
                using (var app = Application.Launch(Info.ExeFileName, "TransposedWindow"))
                {
                    var window = app.MainWindow;
                    var dataGrid = window.FindDataGrid("TransposedObservableCollection");
                    var columnHeaders = dataGrid.ColumnHeaders;
                    Assert.AreEqual(3, columnHeaders.Count);
                    Assert.AreEqual("Name", columnHeaders[0].Text);
                    Assert.AreEqual("C0", columnHeaders[1].Text);
                    Assert.AreEqual(2, dataGrid.RowCount);
                    Assert.AreEqual(3, dataGrid.ColumnCount);
                    Assert.AreEqual("FirstName", dataGrid[0, 0].Value);
                    Assert.AreEqual("Johan", dataGrid[0, 1].Value);
                    Assert.AreEqual("Erik", dataGrid[0, 2].Value);
                    Assert.AreEqual("LastName", dataGrid[1, 0].Value);
                    Assert.AreEqual("Larsson", dataGrid[1, 1].Value);
                    Assert.AreEqual("Svensson", dataGrid[1, 2].Value);

                    var reference = window.FindDataGrid("ReferenceDataGrid");
                    reference[2, 0].Value = "New";
                    Keyboard.Type(Key.UP);
                    window.WaitUntilResponsive();
                    columnHeaders = dataGrid.ColumnHeaders;
                    Assert.AreEqual(4, columnHeaders.Count);
                    Assert.AreEqual("Name", columnHeaders[0].Text);
                    Assert.AreEqual("C0", columnHeaders[1].Text);
                    Assert.AreEqual("C1", columnHeaders[2].Text);
                    Assert.AreEqual("C2", columnHeaders[3].Text);
                    Assert.AreEqual(2, dataGrid.RowCount);
                    Assert.AreEqual(4, dataGrid.ColumnCount);
                    Assert.AreEqual("FirstName", dataGrid[0, 0].Value);
                    Assert.AreEqual("Johan", dataGrid[0, 1].Value);
                    Assert.AreEqual("Erik", dataGrid[0, 2].Value);
                    Assert.AreEqual("New", dataGrid[0, 3].Value);
                    Assert.AreEqual("LastName", dataGrid[1, 0].Value);
                    Assert.AreEqual("Larsson", dataGrid[1, 1].Value);
                    Assert.AreEqual("Svensson", dataGrid[1, 2].Value);
                    Assert.AreEqual(string.Empty, dataGrid[1, 3].Value);
                }
            }

            [Test]
            public static void NammeColumnIsReadOnly()
            {
                using (var app = Application.Launch(Info.ExeFileName, "TransposedWindow"))
                {
                    var window = app.MainWindow;
                    var dataGrid = window.FindDataGrid("TransposedExplicitColumns");
                    var columnHeaders = dataGrid.ColumnHeaders;
                    Assert.AreEqual(2, columnHeaders.Count);
                    Assert.AreEqual("Name", columnHeaders[0].Text);
                    Assert.AreEqual("Value", columnHeaders[1].Text);

                    Assert.AreEqual(2, dataGrid.Rows.Count);
                    Assert.AreEqual(true, dataGrid[0, 0].IsReadOnly);
                    Assert.AreEqual(false, dataGrid[0, 1].IsReadOnly);
                    Assert.AreEqual(true, dataGrid[1, 0].IsReadOnly);
                    Assert.AreEqual(false, dataGrid[1, 1].IsReadOnly);
                }
            }
        }
    }
}
