namespace Gu.Wpf.DataGrid2D.Tests
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Controls;
    using NUnit.Framework;

    [RequiresSTA]
    public class Source2DTests
    {
        [Test]
        public void BindItemsSource2D()
        {
            Data2d = new[,] { { 1, 2 }, { 3, 4 } };
            var dataGrid = new DataGrid();
            dataGrid.Bind(Source2D.ItemsSource2DProperty)
                    .OneWayTo(this, new PropertyPath(nameof(Data2d)));
            Assert.AreEqual(2, dataGrid.Columns.Count);
            var rowsSource = (List<IList>)dataGrid.GetRowsSource();
            CollectionAssert.AreEqual(new[] { 1, 2 }, rowsSource[0]);
            CollectionAssert.AreEqual(new[] { 3, 4 }, rowsSource[1]);
        }

        public int[,] Data2d { get; set; }
    }
}
