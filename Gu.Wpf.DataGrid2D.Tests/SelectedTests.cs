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
    }
}
