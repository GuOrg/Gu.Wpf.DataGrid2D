namespace Gu.Wpf.DataGrid2D.UiTests
{
    using Gu.Wpf.UiAutomation;
    using NUnit.Framework;

    public class AddRemoveTest
    {
        [Test]
        public void RemoveFirstColumn()
        {
            using (var app = Application.Launch(Info.ExeFileName, "AddRemoveWindow"))
            {
                var window = app.MainWindow;
                var dataGrid = window.FindDataGrid("DataGridData");

                var addRowBtn = window.FindButton("Row++");
                addRowBtn.Click();
                addRowBtn.Click();
                addRowBtn.Click();

                var addColBtn = window.FindButton("Column++");
                addColBtn.Click();
                // hack around framework bug.
                _ = dataGrid.FindAllChildren();
                Assert.AreEqual("0", dataGrid[0, 0].Value);
                Assert.AreEqual("0", dataGrid[1, 0].Value);
                Assert.AreEqual("0", dataGrid[2, 0].Value);

                addColBtn.Click();
                Assert.AreEqual("1", dataGrid[0, 1].Value);
                Assert.AreEqual("1", dataGrid[1, 1].Value);
                Assert.AreEqual("1", dataGrid[2, 1].Value);

                addColBtn.Click();
                Assert.AreEqual("2", dataGrid[0, 2].Value);
                Assert.AreEqual("2", dataGrid[1, 2].Value);
                Assert.AreEqual("2", dataGrid[2, 2].Value);

                Assert.AreEqual(3, dataGrid.RowCount);
                Assert.AreEqual(3, dataGrid.ColumnCount);

                var remColBtn = window.FindButton("Column--");
                remColBtn.Click();
                Assert.AreEqual("1", dataGrid[0, 1].Value);
                Assert.AreEqual("1", dataGrid[1, 1].Value);
                Assert.AreEqual("1", dataGrid[2, 1].Value);

                remColBtn.Click();
                Assert.AreEqual("0", dataGrid[0, 0].Value);
                Assert.AreEqual("0", dataGrid[1, 0].Value);
                Assert.AreEqual("0", dataGrid[2, 0].Value);

                remColBtn.Click();
            }
        }
    }
}
