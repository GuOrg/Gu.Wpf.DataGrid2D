namespace Gu.Wpf.DataGrid2D.UiTests
{
    using Gu.Wpf.DataGrid2D.Demo;
    using Gu.Wpf.UiAutomation;
    using NUnit.Framework;

    public class AddRemoveTest
    {
        [Test]
        public void RemovingFirstColumn()
        {
            using (var app = Application.Launch(Info.ExeFileName, "AddRemoveWindow"))
            {
                var window = app.MainWindow;
                var dataGrid = window.FindDataGrid(nameof(AutomationIds.DataGridData));

                var addRowBtn = window.FindButton(nameof(AutomationIds.AddRowButton));
                var addColBtn = window.FindButton(nameof(AutomationIds.AddColumnButton));
                var remRowBtn = window.FindButton(nameof(AutomationIds.RemoveLastRowButton));
                var remColBtn = window.FindButton(nameof(AutomationIds.RemoveLastColumnButton));

                addRowBtn.Click();
                addRowBtn.Click();
                addRowBtn.Click();

                addColBtn.Click();
                addColBtn.Click();
                addColBtn.Click();

                Assert.AreEqual(3, dataGrid.RowCount);
                Assert.AreEqual(3, dataGrid.ColumnCount);

                remColBtn.Click();
                remColBtn.Click();
                remColBtn.Click();

            }
        }

    }
}