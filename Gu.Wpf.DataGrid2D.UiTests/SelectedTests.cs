namespace Gu.Wpf.DataGrid2D.UiTests
{
    using Gu.Wpf.DataGrid2D.Demo;
    using NUnit.Framework;
    using TestStack.White;
    using TestStack.White.Factory;
    using TestStack.White.UIItems;
    using TestStack.White.UIItems.TabItems;
    using ListView = TestStack.White.UIItems.ListView;
    using TextBox = TestStack.White.UIItems.TextBox;

    [RequiresSTA]
    public class SelectedTests
    {
        private static readonly string TabId = AutomationIds.SelectionTab;

        [Test]
        public void SelectingInViewUpdatesIndexAndCellItem()
        {
            using (var app = Application.AttachOrLaunch(Info.ProcessStartInfo))
            {
                var window = app.GetWindow(AutomationIds.MainWindow, InitializeOption.NoCache);
                var page = window.Get<TabPage>(TabId);
                page.Select();
                var dataGrid = page.Get<ListView>(AutomationIds.SelectionGrid);
                var indexBox = page.Get<TextBox>(AutomationIds.SelectedIndex);
                var itemBox = page.Get<Label>(AutomationIds.SelectedItem);
                var c0 = dataGrid.Header.Columns[0].Text;
                Assert.AreEqual("C0", c0);
                var c1 = dataGrid.Header.Columns[1].Text;
                Assert.AreEqual("C1", c1);

                dataGrid.Cell(c0, 0).Click();
                Assert.AreEqual("R0 C0", indexBox.Text);
                Assert.AreEqual("Item: 1", itemBox.Text);

                dataGrid.Cell(c1, 2).Click();
                Assert.AreEqual("R2 C1", indexBox.Text);
                Assert.AreEqual("Item: 6", itemBox.Text);
            }
        }

        [Test]
        public void SettingIndexInViewModelUpdatesSelectionAndCellItem()
        {
            using (var app = Application.AttachOrLaunch(Info.ProcessStartInfo))
            {
                var window = app.GetWindow(AutomationIds.MainWindow, InitializeOption.NoCache);
                var page = window.Get<TabPage>(TabId);
                page.Select();
                var dataGrid = page.Get<ListView>(AutomationIds.SelectionGrid);
                var indexBox = page.Get<TextBox>(AutomationIds.SelectedIndex);
                var itemBox = page.Get<Label>(AutomationIds.SelectedItem);
                var loseFocusButton = page.Get<Button>(AutomationIds.SelectionLoseFocusButton);
                var c0 = dataGrid.Header.Columns[0].Text;
                Assert.AreEqual("C0", c0);
                var c1 = dataGrid.Header.Columns[1].Text;
                Assert.AreEqual("C1", c1);

                var cell = dataGrid.Cell(c1, 1);
                Assert.IsFalse(cell.IsFocussed);
                indexBox.Text = "R1 C1";
                loseFocusButton.Click();
                Assert.IsTrue(cell.IsFocussed);
                Assert.AreEqual("Item: 4", itemBox.Text);

                cell = dataGrid.Cell(c0, 0);
                Assert.IsFalse(cell.IsFocussed);
                indexBox.Text = "R0 C0";
                loseFocusButton.Click();
                Assert.IsTrue(cell.IsFocussed);
                Assert.AreEqual("Item: 1", itemBox.Text);

                // Not sure how we want to handle out of bounds
                Assert.IsTrue(cell.IsFocussed);
                indexBox.Text = "R10 C10";
                loseFocusButton.Click();
                Assert.IsFalse(cell.IsFocussed);
                Assert.AreEqual("Item: 1", itemBox.Text);
            }
        }
    }
}
