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
                Assert.IsFalse(cell.IsFocussed);
                Assert.AreEqual("Item: 4", itemBox.Text);

                //dataGrid.Cell(c1, 2).Click();
                //Assert.AreEqual("R2 C1", indexBox.Text);
                //Assert.AreEqual("Item: 6", itemBox.Text);
            }
        }

        //[Test]
        //public void TracksSelected()
        //{
        //    var data = new[,] { { 1, 2 }, { 3, 4 }, { 5, 6 } };
        //    var dataGrid = new DataGrid();
        //    dataGrid.Bind(ItemsSource.Array2DProperty)
        //            .OneWayTo(data);

        //    Assert.AreEqual(null, Selected.GetIndex(dataGrid));
        //    Assert.AreEqual(null, Selected.GetCellItem(dataGrid));

        //    var index = new RowColumnIndex(0, 0);
        //    Selected.SetIndex(dataGrid, index);
        //    Assert.AreEqual(index, Selected.GetIndex(dataGrid));
        //    Assert.AreEqual(1, Selected.GetCellItem(dataGrid));
        //}

        //[Test]
        //public void ChangeSelectedIndex()
        //{
        //    var data = new[,] { { 1, 2 }, { 3, 4 }, { 5, 6 } };
        //    var dataGrid = new DataGrid();
        //    dataGrid.Bind(ItemsSource.Array2DProperty)
        //            .OneWayTo(data);
        //    dataGrid.SelectionUnit = DataGridSelectionUnit.Cell;
        //    dataGrid.Initialize();
        //    var index = new RowColumnIndex(0, 0);
        //    dataGrid.SetValue(Selected.IndexProperty, index);
        //    Assert.AreEqual(index, Selected.GetIndex(dataGrid));
        //    Assert.AreEqual(1, Selected.GetCellItem(dataGrid));
        //    Assert.AreEqual(0, dataGrid.SelectedIndex);
        //    Assert.AreEqual(1, dataGrid.SelectedItem);
        //}
    }
}
