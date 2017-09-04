namespace Gu.Wpf.DataGrid2D.UiTests
{
    using System;
    using System.Threading;

    using Gu.Wpf.DataGrid2D.Demo;
    using NUnit.Framework;
    using TestStack.White;
    using TestStack.White.Factory;
    using TestStack.White.UIItems;
    using TestStack.White.UIItems.ListBoxItems;
    using TestStack.White.UIItems.TabItems;
    using ListView = TestStack.White.UIItems.ListView;
    using TextBox = TestStack.White.UIItems.TextBox;

    [Apartment(ApartmentState.STA)]
    public class SelectedTests
    {
        private static readonly string TabId = "SelectionTab";

        [Test]
        public void SelectingInViewUpdatesIndexAndCellItem()
        {
            using (var app = Application.AttachOrLaunch(Info.ProcessStartInfo))
            {
                var window = app.GetWindow("MainWindow", InitializeOption.NoCache);
                var page = window.Get<TabPage>(TabId);
                page.Select();
                var dataGrid = page.Get<ListView>("SelectionGrid");
                var indexBox = page.Get<TextBox>("SelectedIndex");
                var itemBox = page.Get<Label>("SelectedItem");
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
                var window = app.GetWindow("MainWindow", InitializeOption.NoCache);
                var page = window.Get<TabPage>(TabId);
                page.Select();
                var dataGrid = page.Get<ListView>("SelectionGrid");
                Console.WriteLine($"DataGrid.HelpText: {dataGrid.HelpText}");
                Console.WriteLine($"DataGrid.ItemStatus: {dataGrid.ItemStatus()}");
                var indexBox = page.Get<TextBox>("SelectedIndex");
                var itemBox = page.Get<Label>("SelectedItem");
                var loseFocusButton = page.Get<Button>("SelectionLoseFocusButton");
                var c0 = dataGrid.Header.Columns[0].Text;
                Assert.AreEqual("C0", c0);
                var c1 = dataGrid.Header.Columns[1].Text;
                Assert.AreEqual("C1", c1);

                indexBox.Text = "R1 C1";
                loseFocusButton.Click();
                Assert.AreEqual("Item: 4", itemBox.Text);

                indexBox.Text = "R0 C0";
                loseFocusButton.Click();
                Assert.AreEqual("Item: 1", itemBox.Text);

                // Not sure how we want to handle out of bounds
                indexBox.Text = "R10 C10";
                loseFocusButton.Click();
                Assert.AreEqual(string.Empty, itemBox.Text);
            }
        }

        [Test]
        public void SettingCellItemInViewModelUpdatesSelectionAndIndex()
        {
            using (var app = Application.AttachOrLaunch(Info.ProcessStartInfo))
            {
                var window = app.GetWindow("MainWindow", InitializeOption.NoCache);
                var page = window.Get<TabPage>(TabId);
                page.Select();
                var dataGrid = page.Get<ListView>("SelectionGrid");
                var indexBox = page.Get<TextBox>("SelectedIndex");
                var itemBox = page.Get<ListBox>("SelectionList");
                var c0 = dataGrid.Header.Columns[0].Text;
                Assert.AreEqual("C0", c0);
                var c1 = dataGrid.Header.Columns[1].Text;
                Assert.AreEqual("C1", c1);

                itemBox.Select(3);
                Assert.AreEqual("R1 C1", indexBox.Text);

                itemBox.Select(0);
                Assert.AreEqual("R0 C0", indexBox.Text);
            }
        }

        [Test]
        public void Reminder()
        {
            Assert.Inconclusive("Assert cell.IsSelected when supported");
        }
    }
}
