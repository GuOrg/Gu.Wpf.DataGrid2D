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
                var tabItem = window.FindTabItem("AddRemoveView");
                tabItem.Click();
                var dataGrid = tabItem.FindDataGrid("DataGridData");

                var addRowBtn = window.FindButton("Row++");
                var addColBtn = window.FindButton("Column++");
                ////var remRowBtn = window.FindButton("Row--");
                var remColBtn = window.FindButton("Column--");

                addRowBtn.Click();
                addRowBtn.Click();
                addRowBtn.Click();

                addColBtn.Click();

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
