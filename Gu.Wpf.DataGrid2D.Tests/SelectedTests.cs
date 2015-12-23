namespace Gu.Wpf.DataGrid2D.Tests
{
    using System.Windows.Controls;
    using NUnit.Framework;

    [RequiresSTA]
    public class SelectedTests
    {
        [Test]
        public void TracksSelectedInVm()
        {
            var data = new[,] { { 1, 2 }, { 3, 4 }, { 5, 6 } };
            var dataGrid = new DataGrid();
            dataGrid.Bind(ItemsSource.Array2DProperty)
                    .OneWayTo(data);

            Assert.AreEqual(null, Selected.GetIndex(dataGrid));
            Assert.AreEqual(null, Selected.GetCellItem(dataGrid));

            //dataGrid.SelectedCells
            Assert.Inconclusive();
        }

        [Test]
        public void ChangeSelectedIndex()
        {
            var data = new[,] { { 1, 2 }, { 3, 4 }, { 5, 6 } };
            var dataGrid = new DataGrid();
            dataGrid.Bind(ItemsSource.Array2DProperty)
                    .OneWayTo(data);
            dataGrid.SelectionUnit = DataGridSelectionUnit.Cell;
            dataGrid.Initialize();
            var index = new RowColumnIndex(0, 0);
            dataGrid.SetValue(Selected.IndexProperty, index);
            Assert.AreEqual(index, Selected.GetIndex(dataGrid));
            Assert.AreEqual(1, Selected.GetCellItem(dataGrid));
            Assert.AreEqual(0, dataGrid.SelectedIndex);
            Assert.AreEqual(1, dataGrid.SelectedItem);
        }
    }
}
