using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gu.Wpf.DataGrid2D.UiTests
{
    using System.Diagnostics;
    using System.Windows.Controls;
    using Gu.Wpf.DataGrid2D.Demo;
    using NUnit.Framework;
    using TestStack.White;
    using TestStack.White.Factory;
    using TestStack.White.UIItems.Finders;
    using TestStack.White.UIItems.TabItems;
    using TestStack.White.UIItems.WPFUIItems;
    using ListView = TestStack.White.UIItems.ListView;

    public partial class ItemsSourceTests
    {
        public class Transposed
        {
            [Test]
            public void ExplicitColumns()
            {
                using (var app = Application.AttachOrLaunch(ProcessStartInfo))
                {
                    var window = app.GetWindow(AutomationIds.MainWindow, InitializeOption.NoCache);
                    var page = window.Get<TabPage>(SearchCriteria.ByAutomationId(AutomationIds.TransposedTab));
                    page.Select();
                    var dataGrid = page.Get<ListView>(SearchCriteria.ByAutomationId(AutomationIds.TransposedExplicitColumns));

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
                using (var app = Application.AttachOrLaunch(ProcessStartInfo))
                {
                    var window = app.GetWindow(AutomationIds.MainWindow, InitializeOption.NoCache);
                    var page = window.Get<TabPage>(SearchCriteria.ByAutomationId(AutomationIds.TransposedTab));
                    page.Select();
                    var dataGrid = page.Get<ListView>(SearchCriteria.ByAutomationId(AutomationIds.TransposedSingleton));

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
                using (var app = Application.AttachOrLaunch(ProcessStartInfo))
                {
                    var window = app.GetWindow(AutomationIds.MainWindow, InitializeOption.NoCache);
                    var page = window.Get<TabPage>(SearchCriteria.ByAutomationId(AutomationIds.TransposedTab));
                    page.Select();
                    var dataGrid = page.Get<ListView>(SearchCriteria.ByAutomationId(AutomationIds.TransposedObservableCollection));

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
            public void ObservableCollectionEditInViewUpdatesDataContext()
            {
                using (var app = Application.AttachOrLaunch(ProcessStartInfo))
                {
                    var window = app.GetWindow(AutomationIds.MainWindow, InitializeOption.NoCache);
                    var page = window.Get<TabPage>(SearchCriteria.ByAutomationId(AutomationIds.TransposedTab));
                    page.Select();
                    var dataGrid = page.Get<ListView>(SearchCriteria.ByAutomationId(AutomationIds.TransposedObservableCollection));

                    Assert.Fail("Edit value and check that other view updates");
                }
            }

            [Test]
            public void TestName()
            {
                Assert.Fail("Name column is readonly");
            }
        }
    }
}
