namespace Gu.Wpf.DataGrid2D.UiTests
{
    using Gu.Wpf.DataGrid2D.Demo;
    using NUnit.Framework;
    using TestStack.White;
    using TestStack.White.Factory;
    using TestStack.White.UIItems;
    using TestStack.White.UIItems.TabItems;

    public partial class ItemsSourceTests
    {
        public class CellTemplate
        {
            private static readonly string TabId = AutomationIds.CellTemplateTab;

            [Test]
            public void CellTemplateTest()
            {
                using (var app = Application.AttachOrLaunch(Info.ProcessStartInfo))
                {
                    var window = app.GetWindow(AutomationIds.MainWindow, InitializeOption.NoCache);
                    var page = window.Get<TabPage>(TabId);
                    page.Select();

                    var dataGrid1 = page.Get<ListView>(AutomationIds.CellTemplate1Grid);
                    var dataGrid1ro = page.Get<ListView>(AutomationIds.CellTemplate1GridRO);
                    var dataGrid2 = page.Get<ListView>(AutomationIds.CellTemplate2Grid);
                    var dataGrid2ro = page.Get<ListView>(AutomationIds.CellTemplate2GridRO);

                    for (int i = 0; i < 3; ++i)
                    {
                        var c0 = dataGrid1.Header.Columns[i].Text;
                        Assert.AreEqual($"C{i + 1}", c0);
                        c0 = dataGrid1ro.Header.Columns[i].Text;
                        Assert.AreEqual($"C{i + 1}", c0);
                        c0 = dataGrid2.Header.Columns[i].Text;
                        Assert.AreEqual($"C{i + 1}", c0);
                        c0 = dataGrid2ro.Header.Columns[i].Text;
                        Assert.AreEqual($"C{i + 1}", c0);
                    }

                    for (int i = 0; i < 3; ++i)
                    {
                        for (int j = 0; j < 3; ++j)
                        {
                            Assert.AreEqual($"{i + j}", dataGrid1.Cell($"C{i + 1}", j).Text);
                            Assert.AreEqual($"{i + j}", dataGrid1ro.Cell($"C{i + 1}", j).Text);
                            Assert.AreEqual($"{9 - (i + j)}", dataGrid2.Cell($"C{i + 1}", j).Text);
                            Assert.AreEqual($"{9 - (i + j)}", dataGrid2ro.Cell($"C{i + 1}", j).Text);
                        }
                    }

                    var cell = dataGrid1.Cell("C1", 0);
                    cell.DoubleClick();
                    cell.Enter("10");
                    dataGrid1.Select(1);
                    window.WaitWhileBusy();
                    Assert.AreEqual("10", dataGrid1ro.Cell("C1", 0).Text);

                    cell = dataGrid2.Cell("C1", 0);
                    cell.DoubleClick();
                    cell.Enter("11");
                    dataGrid2.Select(1);
                    window.WaitWhileBusy();
                    Assert.AreEqual("11", dataGrid2ro.Cell("C1", 0).Text);
                }
            }

            [Test]
            public void ChangeCellTemplateTest()
            {
                using (var app = Application.AttachOrLaunch(Info.ProcessStartInfo))
                {
                    var window = app.GetWindow(AutomationIds.MainWindow, InitializeOption.NoCache);
                    var page = window.Get<TabPage>(TabId);
                    page.Select();

                    var dataGrid = page.Get<ListView>(AutomationIds.CellTemplateChangingGrid);
                    var button = page.Get<Button>(AutomationIds.CellTemplateChangeButton);

                    for (int i = 0; i < 3; ++i)
                    {
                        var c0 = dataGrid.Header.Columns[i].Text;
                        Assert.AreEqual($"C{i + 1}", c0);
                    }

                    // Check the values are ok: Value1 of the test class should be displayed on startup
                    for (int i = 0; i < 3; ++i)
                    {
                        for (int j = 0; j < 3; ++j)
                        {
                            Assert.AreEqual($"{i + j}", dataGrid.Cell($"C{i + 1}", j).Text);
                        }
                    }

                    // Click the button to change the CellTemplate during runtime
                    button.Click();
                    dataGrid.Select(1);
                    window.WaitWhileBusy();

                    // Check the values again: Value2 of the test class should be displayed now
                    for (int i = 0; i < 3; ++i)
                    {
                        for (int j = 0; j < 3; ++j)
                        {
                            Assert.AreEqual($"{9 - (i + j)}", dataGrid.Cell($"C{i + 1}", j).Text);
                        }
                    }

                    button.Click();
                    dataGrid.Select(1);
                    window.WaitWhileBusy();

                    // Check the values again: Value1 of the test class should be displayed now readonly
                    for (int i = 0; i < 3; ++i)
                    {
                        for (int j = 0; j < 3; ++j)
                        {
                            var cell = dataGrid.Cell($"C{i + 1}", j);
                            cell.DoubleClick();
                            cell.Enter("11");
                            Assert.AreNotEqual("11", dataGrid.Cell($"C{i + 1}", j).Text);
                        }
                    }

                    // below causes a crash: argumentoutofrangeexception
                    button.Click();
                    dataGrid.Select(1);
                    window.WaitWhileBusy();
                }
            }
        }
    }
}