namespace Gu.Wpf.DataGrid2D.Tests
{
    using System.Windows;
    using System.Windows.Controls;
    using NUnit.Framework;

    [RequiresSTA]
    public class ItemsSourceTests
    {
        public int[,] Data2D { get; set; }

        [Test]
        public void Array2D()
        {
            this.Data2D = new[,] { { 1, 2 }, { 3, 4 }, { 5, 6 } };
            var dataGrid = new DataGrid();
            dataGrid.Bind(ItemsSource.Array2DProperty)
                    .OneWayTo(this, new PropertyPath(nameof(this.Data2D)));
            dataGrid.Initialize();

            Assert.AreEqual(2, dataGrid.Columns.Count);
            Assert.AreEqual(3, dataGrid.Items.Count);
            /*
            Assert.AreEqual("1", dataGrid.GetCellText(0, 0));
            Assert.AreEqual(2, dataGrid.GetCellText(1, 0));
            Assert.AreEqual(3, dataGrid.GetCellText(0, 1));
            Assert.AreEqual(4, dataGrid.GetCellText(1, 1));
            Assert.AreEqual(5, dataGrid.GetCellText(0, 2));
            Assert.AreEqual(6, dataGrid.GetCellText(1, 2));
            */
        }

        [Test]
        public void Array2DTransposed()
        {
            this.Data2D = new[,] { { 1, 2 }, { 3, 4 }, { 5, 6 } };
            var dataGrid = new DataGrid();
            dataGrid.Bind(ItemsSource.Array2DTransposedProperty)
                    .OneWayTo(this, new PropertyPath(nameof(this.Data2D)));
            dataGrid.Initialize();
            Assert.AreEqual(3, dataGrid.Columns.Count);
            Assert.AreEqual(2, dataGrid.Items.Count);
            /*
            Assert.AreEqual(1, dataGrid.GetCellText(0, 0));
            Assert.AreEqual(3, dataGrid.GetCellText(1, 0));
            Assert.AreEqual(5, dataGrid.GetCellText(2, 0));
            Assert.AreEqual(2, dataGrid.GetCellText(0, 1));
            Assert.AreEqual(4, dataGrid.GetCellText(1, 1));
            Assert.AreEqual(6, dataGrid.GetCellText(2, 1));
            */
        }
    }
}
