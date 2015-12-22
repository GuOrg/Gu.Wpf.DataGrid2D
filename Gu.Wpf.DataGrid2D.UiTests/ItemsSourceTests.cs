namespace Gu.Wpf.DataGrid2D.UiTests
{
    using System.Diagnostics;
    using System.IO;
    using System.Reflection;
    using Gu.Wpf.DataGrid2D.Demo;
    using NUnit.Framework;
    using TestStack.White;
    using TestStack.White.Factory;
    using TestStack.White.UIItems;
    using TestStack.White.UIItems.Finders;
    using TestStack.White.UIItems.TabItems;

    public class ItemsSourceTests
    {

        private static ProcessStartInfo ProcessStartInfo
        {
            get
            {
                var fileName = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), $"{typeof(MainWindow).Assembly.GetName().Name}.exe");
                var processStartInfo = new ProcessStartInfo
                {
                    FileName = fileName,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true
                };
                return processStartInfo;
            }
        }

        [Test]
        public void Array2DAutoColumns()
        {
            using (var app = Application.AttachOrLaunch(ProcessStartInfo))
            {
                var window = app.GetWindow(AutomationIds.MainWindow, InitializeOption.NoCache);
                var page = window.Get<TabPage>(SearchCriteria.ByAutomationId(AutomationIds.MultiDimensionalTab));
                page.Select();
                var dataGrid = page.Get<ListView>(SearchCriteria.ByAutomationId(AutomationIds.MultiDimensionalAutoColumns));

                Assert.AreEqual(2, dataGrid.Rows[0].Cells.Count);
                Assert.AreEqual(3, dataGrid.Rows.Count);

                var c0 = dataGrid.Header.Columns[0].Text;
                Assert.AreEqual("C0", c0);
                var c1 = dataGrid.Header.Columns[1].Text;
                Assert.AreEqual("C1", c1);

                Assert.AreEqual("1", dataGrid.Cell(c0, 0).Text);
                Assert.AreEqual("3", dataGrid.Cell(c0, 1).Text);
                Assert.AreEqual("5", dataGrid.Cell(c0, 2).Text);

                Assert.AreEqual("2", dataGrid.Cell(c1, 0).Text);
                Assert.AreEqual("4", dataGrid.Cell(c1, 1).Text);
                Assert.AreEqual("6", dataGrid.Cell(c1, 2).Text);
            }
        }

        [Test]
        public void Array2DExplicitColumns()
        {
            using (var app = Application.AttachOrLaunch(ProcessStartInfo))
            {
                var window = app.GetWindow(AutomationIds.MainWindow, InitializeOption.NoCache);
                var page = window.Get<TabPage>(SearchCriteria.ByAutomationId(AutomationIds.MultiDimensionalTab));
                page.Select();
                var dataGrid = page.Get<ListView>(SearchCriteria.ByAutomationId(AutomationIds.MultiDimensionalExplicitColumns));

                Assert.AreEqual(2, dataGrid.Rows[0].Cells.Count);
                Assert.AreEqual(3, dataGrid.Rows.Count);

                var c0 = dataGrid.Header.Columns[0].Text;
                Assert.AreEqual("Col 1", c0);
                var c1 = dataGrid.Header.Columns[1].Text;
                Assert.AreEqual("Col 2", c1);

                Assert.AreEqual("1", dataGrid.Cell(c0, 0).Text);
                Assert.AreEqual("3", dataGrid.Cell(c0, 1).Text);
                Assert.AreEqual("5", dataGrid.Cell(c0, 2).Text);

                Assert.AreEqual("2", dataGrid.Cell(c1, 0).Text);
                Assert.AreEqual("4", dataGrid.Cell(c1, 1).Text);
                Assert.AreEqual("6", dataGrid.Cell(c1, 2).Text);
            }
        }
    }
}
