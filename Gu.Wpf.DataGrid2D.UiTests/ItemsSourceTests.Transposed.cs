namespace Gu.Wpf.DataGrid2D.UiTests
{
    using Gu.Wpf.DataGrid2D.Demo;
    using NUnit.Framework;
    using TestStack.White;
    using TestStack.White.Factory;
    using TestStack.White.UIItems;
    using TestStack.White.UIItems.TabItems;
    using TestStack.White.WindowsAPI;

    public partial class ItemsSourceTests
    {
        public class Transposed
        {
            [Test]
            public void ExplicitColumns()
            {
                using (var app = Application.AttachOrLaunch(Info.ProcessStartInfo))
                {
                    var window = app.GetWindow("MainWindow", InitializeOption.NoCache);
                    var page = window.Get<TabPage>("TransposedTab");
                    page.Select();
                    var dataGrid = page.Get<ListView>("TransposedExplicitColumns");

                    Assert.AreEqual(2, dataGrid.Rows[0].Cells.Count);
                    Assert.AreEqual(2, dataGrid.Rows.Count);

                    var c0 = dataGrid.Header.Columns[0].Text;
                    Assert.AreEqual("Name", c0);
                    var c1 = dataGrid.Header.Columns[1].Text;
                    Assert.AreEqual("Value", c1);

                    Assert.AreEqual("FirstName", dataGrid.Cell(c0, 0).Text);
                    Assert.AreEqual("LastName", dataGrid.Cell(c0, 1).Text);

                    Assert.AreEqual("Johan", dataGrid.Cell(c1, 0).Text);
                    Assert.AreEqual("Larsson", dataGrid.Cell(c1, 1).Text);
                }
            }

            [Test]
            public void Singleton()
            {
                using (var app = Application.AttachOrLaunch(Info.ProcessStartInfo))
                {
                    var window = app.GetWindow("MainWindow", InitializeOption.NoCache);
                    var page = window.Get<TabPage>("TransposedTab");
                    page.Select();
                    var dataGrid = page.Get<ListView>("TransposedSingleton");

                    Assert.AreEqual(2, dataGrid.Rows[0].Cells.Count);
                    Assert.AreEqual(2, dataGrid.Rows.Count);

                    var c0 = dataGrid.Header.Columns[0].Text;
                    Assert.AreEqual("Name", c0);
                    var c1 = dataGrid.Header.Columns[1].Text;
                    Assert.AreEqual("C0", c1);

                    Assert.AreEqual("FirstName", dataGrid.Cell(c0, 0).Text);
                    Assert.AreEqual("LastName", dataGrid.Cell(c0, 1).Text);

                    Assert.AreEqual("Johan", dataGrid.Cell(c1, 0).Text);
                    Assert.AreEqual("Larsson", dataGrid.Cell(c1, 1).Text);
                }
            }

            [Test]
            public void ObservableCollection()
            {
                using (var app = Application.AttachOrLaunch(Info.ProcessStartInfo))
                {
                    var window = app.GetWindow("MainWindow", InitializeOption.NoCache);
                    var page = window.Get<TabPage>("TransposedTab");
                    page.Select();
                    var dataGrid = page.Get<ListView>("TransposedObservableCollection");

                    Assert.AreEqual(3, dataGrid.Rows[0].Cells.Count);
                    Assert.AreEqual(2, dataGrid.Rows.Count);

                    var c0 = dataGrid.Header.Columns[0].Text;
                    Assert.AreEqual("Name", c0);
                    var c1 = dataGrid.Header.Columns[1].Text;
                    Assert.AreEqual("C0", c1);
                    var c2 = dataGrid.Header.Columns[2].Text;
                    Assert.AreEqual("C1", c2);

                    Assert.AreEqual("FirstName", dataGrid.Cell(c0, 0).Text);
                    Assert.AreEqual("LastName", dataGrid.Cell(c0, 1).Text);

                    Assert.AreEqual("Johan", dataGrid.Cell(c1, 0).Text);
                    Assert.AreEqual("Larsson", dataGrid.Cell(c1, 1).Text);

                    Assert.AreEqual("Erik", dataGrid.Cell(c2, 0).Text);
                    Assert.AreEqual("Svensson", dataGrid.Cell(c2, 1).Text);
                }
            }

            [Test]
            public void ObservableCollectionEditCellInViewUpdatesDataContext()
            {
                using (var app = Application.AttachOrLaunch(Info.ProcessStartInfo))
                {
                    var window = app.GetWindow("MainWindow", InitializeOption.NoCache);
                    var page = window.Get<TabPage>("TransposedTab");
                    page.Select();
                    var dataGrid = page.Get<ListView>("TransposedObservableCollection");
                    var cell = dataGrid.Rows[0].Cells[1];
                    cell.Click();
                    cell.Enter("New Value");
                    dataGrid.Select(1); // lose focus
                    window.WaitWhileBusy();

                    var reference = page.Get<ListView>("ReferenceDataGrid");
                    Assert.AreEqual("New Value", reference.Rows[0].Cells[0].Text);
                }
            }

            [Test]
            public void ObservableCollectionEditDataContextUpdatesView()
            {
                using (var app = Application.AttachOrLaunch(Info.ProcessStartInfo))
                {
                    var window = app.GetWindow("MainWindow", InitializeOption.NoCache);
                    var page = window.Get<TabPage>("TransposedTab");
                    page.Select();
                    var reference = page.Get<ListView>("ReferenceDataGrid");

                    var cell = reference.Rows[0].Cells[0];
                    cell.Click();
                    cell.Enter("New Value");

                    var dataGrid = page.Get<ListView>("TransposedObservableCollection");
                    Assert.AreEqual("Johan", dataGrid.Rows[0].Cells[1].Text);
                    reference.Select(1);
                    Assert.AreEqual("New Value", dataGrid.Rows[0].Cells[1].Text);
                }
            }

            [Test]
            public void RemoveRowInDataContext()
            {
                using (var app = Application.AttachOrLaunch(Info.ProcessStartInfo))
                {
                    var window = app.GetWindow("MainWindow", InitializeOption.NoCache);
                    var page = window.Get<TabPage>("TransposedTab");
                    page.Select();

                    var reference = page.Get<ListView>("ReferenceDataGrid");
                    reference.Select(1);
                    reference.KeyIn(KeyboardInput.SpecialKeys.DELETE);

                    var dataGrid = page.Get<ListView>("TransposedObservableCollection");

                    Assert.AreEqual(2, dataGrid.Rows[0].Cells.Count);
                    Assert.AreEqual(2, dataGrid.Rows.Count);

                    var c0 = dataGrid.Header.Columns[0].Text;
                    Assert.AreEqual("Name", c0);
                    var c1 = dataGrid.Header.Columns[1].Text;
                    Assert.AreEqual("C0", c1);

                    Assert.AreEqual("FirstName", dataGrid.Cell(c0, 0).Text);
                    Assert.AreEqual("LastName", dataGrid.Cell(c0, 1).Text);

                    Assert.AreEqual("Johan", dataGrid.Cell(c1, 0).Text);
                    Assert.AreEqual("Larsson", dataGrid.Cell(c1, 1).Text);
                }
            }

            [Test]
            public void AddRowInDataContext()
            {
                using (var app = Application.AttachOrLaunch(Info.ProcessStartInfo))
                {
                    var window = app.GetWindow("MainWindow", InitializeOption.NoCache);
                    var page = window.Get<TabPage>("TransposedTab");
                    page.Select();

                    var reference = page.Get<ListView>("ReferenceDataGrid");
                    var cell = reference.Rows[2].Cells[0];
                    cell.Click();
                    cell.Enter("New");
                    reference.Select(0);
                    var dataGrid = page.Get<ListView>("TransposedObservableCollection");

                    Assert.AreEqual(4, dataGrid.Rows[0].Cells.Count);
                    Assert.AreEqual(2, dataGrid.Rows.Count);

                    var c0 = dataGrid.Header.Columns[0].Text;
                    var c1 = dataGrid.Header.Columns[1].Text;
                    var c2 = dataGrid.Header.Columns[2].Text;
                    var c3 = dataGrid.Header.Columns[3].Text;

                    Assert.AreEqual("FirstName", dataGrid.Cell(c0, 0).Text);
                    Assert.AreEqual("LastName", dataGrid.Cell(c0, 1).Text);

                    Assert.AreEqual("Johan", dataGrid.Cell(c1, 0).Text);
                    Assert.AreEqual("Larsson", dataGrid.Cell(c1, 1).Text);

                    Assert.AreEqual("Erik", dataGrid.Cell(c2, 0).Text);
                    Assert.AreEqual("Svensson", dataGrid.Cell(c2, 1).Text);

                    Assert.AreEqual("New", dataGrid.Cell(c3, 0).Text);
                    Assert.AreEqual(string.Empty, dataGrid.Cell(c3, 1).Text);
                }
            }

            [Test]
            public void Reminder()
            {
                Assert.Inconclusive("Check that name column is readonly");
            }
        }
    }
}
