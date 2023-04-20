namespace Gu.Wpf.DataGrid2D.UiTests
{
    using Gu.Wpf.UiAutomation;
    using NUnit.Framework;

    public static partial class ItemsSourceTests
    {
        public static class CellTemplate
        {
            [TestCase("CellTemplate1Grid")]
            [TestCase("CellTemplate1GridRO")]
            [TestCase("CellTemplate2Grid")]
            [TestCase("CellTemplate2GridRO")]
            public static void Headers(string name)
            {
                using var app = Application.Launch(Info.ExeFileName, "CellTemplateWindow");
                var window = app.MainWindow;
                var dataGrid = window.FindDataGrid(name);
                Assert.AreEqual(3, dataGrid.ColumnHeaders.Count);
                Assert.AreEqual("C1", dataGrid.ColumnHeaders[0].Text);
                Assert.AreEqual("C2", dataGrid.ColumnHeaders[1].Text);
                Assert.AreEqual("C3", dataGrid.ColumnHeaders[2].Text);

                Assert.AreEqual(3, dataGrid.Rows.Count);
                Assert.AreEqual("R1", dataGrid.Rows[0].Header.Text);
                Assert.AreEqual("R2", dataGrid.Rows[1].Header.Text);
                Assert.AreEqual("R3", dataGrid.Rows[2].Header.Text);
            }

            [TestCase("CellTemplate1Grid")]
            [TestCase("CellTemplate1GridRO")]
            [TestCase("CellTemplate2Grid")]
            [TestCase("CellTemplate2GridRO")]
            public static void CellTemplateTest(string name)
            {
                using var app = Application.Launch(Info.ExeFileName, "CellTemplateWindow");
                var window = app.MainWindow;
                var dataGrid = window.FindDataGrid(name);

                for (var r = 0; r < 3; ++r)
                {
                    for (var c = 0; c < 3; ++c)
                    {
                        var expected = name.Contains("2")
                                           ? $"{9 - (r + c)}"
                                           : $"{r + c}";
                        Assert.AreEqual(expected, dataGrid[r, c].FindTextBlock().Text);
                    }
                }
            }

            [TestCase("CellTemplate1Grid")]
            [TestCase("CellTemplate2Grid")]
            public static void Edit(string name)
            {
                using var app = Application.Launch(Info.ExeFileName, "CellTemplateWindow");
                var window = app.MainWindow;
                var dataGrid = window.FindDataGrid(name);
                var readOnlyDataGrid = window.FindDataGrid(name + "RO");
                var cell = dataGrid[0, 0];
                cell.Click();
                cell.Click();
                cell.Enter("10");
                Keyboard.Type(Key.ENTER);
                window.WaitUntilResponsive();
                Assert.AreEqual("10", cell.FindTextBlock().Text);
                Assert.AreEqual("10", readOnlyDataGrid[0, 0].FindTextBlock().Text);

                cell.Click();
                cell.Click();
                cell.Enter("11");
                Keyboard.Type(Key.ENTER);
                window.WaitUntilResponsive();
                Assert.AreEqual("11", cell.FindTextBlock().Text);
                Assert.AreEqual("11", readOnlyDataGrid[0, 0].FindTextBlock().Text);
            }

            [Test]
            public static void ChangeCellTemplateTest()
            {
                using var app = Application.Launch(Info.ExeFileName, "CellTemplateWindow");
                var window = app.MainWindow;
                var dataGrid = window.FindDataGrid("CellTemplateChangingGrid");
                var button = window.FindButton("CellTemplateChangeButton");

                for (var i = 0; i < 3; ++i)
                {
                    var c0 = dataGrid.ColumnHeaders[i].Text;
                    Assert.AreEqual($"C{i + 1}", c0);
                }

                // Check the values are ok: Value1 of the test class should be displayed on startup
                for (var r = 0; r < 3; ++r)
                {
                    for (var c = 0; c < 3; ++c)
                    {
                        Assert.AreEqual($"{r + c}", dataGrid[r, c].FindTextBlock().Text);
                    }
                }

                // Click the button to change the CellTemplate during runtime
                button.Click();

                // Check the values again: Value2 of the test class should be displayed now
                for (var r = 0; r < 3; ++r)
                {
                    for (var c = 0; c < 3; ++c)
                    {
                        Assert.AreEqual($"{9 - (r + c)}", dataGrid[r, c].FindTextBlock().Text);
                    }
                }

                button.Click();

                // Check the values again: Value1 of the test class should be displayed now readonly
                for (var r = 0; r < 3; ++r)
                {
                    for (var c = 0; c < 3; ++c)
                    {
                        var cell = dataGrid[r, c];
                        cell.DoubleClick();
                        cell.Enter("11");
                        Assert.AreNotEqual("11", dataGrid[r, c].FindTextBlock().Text);
                    }
                }

                // below causes a crash: argumentoutofrangeexception
                button.Click();
            }
        }
    }
}
