namespace Gu.Wpf.DataGrid2D.Tests
{
    using System.Collections.ObjectModel;
    using System.Threading;
    using System.Windows.Controls;
    using Gu.Wpf.DataGrid2D.Tests.Views.Stubs;
    using NUnit.Framework;

    [Apartment(ApartmentState.STA)]
    public static class ItemsSourceTests
    {
        [Test]
        public static void Array2D()
        {
            var array = new[,]
                        {
                            { 1, 2 },
                            { 3, 4 },
                            { 5, 6 },
                        };
            var dataGrid = new DataGrid();
            dataGrid.SetValue(ItemsSource.Array2DProperty, array);
            dataGrid.Initialize();

            Assert.IsInstanceOf<Array2DView>(dataGrid.ItemsSource);
            Assert.AreEqual(2, dataGrid.Columns.Count);
            Assert.AreEqual(3, dataGrid.Items.Count);

            Assert.AreEqual(1, dataGrid.GetCellValue(0, 0));
            Assert.AreEqual(2, dataGrid.GetCellValue(0, 1));

            Assert.AreEqual(3, dataGrid.GetCellValue(1, 0));
            Assert.AreEqual(4, dataGrid.GetCellValue(1, 1));

            Assert.AreEqual(5, dataGrid.GetCellValue(2, 0));
            Assert.AreEqual(6, dataGrid.GetCellValue(2, 1));
        }

        [Test]
        public static void Array2DTransposed()
        {
            var array = new[,]
                        {
                            { 1, 2 },
                            { 3, 4 },
                            { 5, 6 },
                        };
            var dataGrid = new DataGrid();
            dataGrid.SetValue(ItemsSource.Array2DTransposedProperty, array);
            dataGrid.Initialize();
            Assert.IsInstanceOf<Array2DView>(dataGrid.ItemsSource);
            Assert.AreEqual(3, dataGrid.Columns.Count);
            Assert.AreEqual(2, dataGrid.Items.Count);

            Assert.AreEqual(1, dataGrid.GetCellValue(0, 0));
            Assert.AreEqual(3, dataGrid.GetCellValue(0, 1));
            Assert.AreEqual(5, dataGrid.GetCellValue(0, 2));

            Assert.AreEqual(2, dataGrid.GetCellValue(1, 0));
            Assert.AreEqual(4, dataGrid.GetCellValue(1, 1));
            Assert.AreEqual(6, dataGrid.GetCellValue(1, 2));
        }

        [Test]
        public static void RowsSourceRemove()
        {
            var ints = new ObservableCollection<ObservableCollection<int>>
                           {
                               new ObservableCollection<int>(new[] { 1, 2 }),
                               new ObservableCollection<int>(new[] { 3, 4 }),
                               new ObservableCollection<int>(new[] { 5, 6 }),
                           };
            var dataGrid = new DataGrid();
            dataGrid.SetValue(ItemsSource.RowsSourceProperty, ints);
            dataGrid.Initialize();
            Assert.IsInstanceOf<Lists2DView>(dataGrid.ItemsSource);

            Assert.AreEqual(1, dataGrid.GetCellValue(0, 0));
            Assert.AreEqual(2, dataGrid.GetCellValue(0, 1));

            Assert.AreEqual(3, dataGrid.GetCellValue(1, 0));
            Assert.AreEqual(4, dataGrid.GetCellValue(1, 1));

            Assert.AreEqual(5, dataGrid.GetCellValue(2, 0));
            Assert.AreEqual(6, dataGrid.GetCellValue(2, 1));

            ints.RemoveAt(2);
            Assert.AreNotSame(ints, dataGrid.ItemsSource);
            Assert.AreEqual(2, dataGrid.Columns.Count);
            Assert.AreEqual(2, dataGrid.Items.Count);

            Assert.AreEqual(1, dataGrid.GetCellValue(0, 0));
            Assert.AreEqual(2, dataGrid.GetCellValue(0, 1));

            Assert.AreEqual(3, dataGrid.GetCellValue(1, 0));
            Assert.AreEqual(4, dataGrid.GetCellValue(1, 1));
        }

        [Test]
        public static void RowsSourceAddRow()
        {
            var ints = new ObservableCollection<ObservableCollection<int>>
                       {
                           new ObservableCollection<int>(new[] { 1, 2 }),
                           new ObservableCollection<int>(new[] { 3, 4 }),
                           new ObservableCollection<int>(new[] { 5, 6 }),
                       };
            var dataGrid = new DataGrid();
            dataGrid.SetValue(ItemsSource.RowsSourceProperty, ints);
            dataGrid.Initialize();
            Assert.IsInstanceOf<Lists2DView>(dataGrid.ItemsSource);

            Assert.AreEqual(1, dataGrid.GetCellValue(0, 0));
            Assert.AreEqual(2, dataGrid.GetCellValue(0, 1));

            Assert.AreEqual(3, dataGrid.GetCellValue(1, 0));
            Assert.AreEqual(4, dataGrid.GetCellValue(1, 1));

            Assert.AreEqual(5, dataGrid.GetCellValue(2, 0));
            Assert.AreEqual(6, dataGrid.GetCellValue(2, 1));

            ints.Add(new ObservableCollection<int> { 7, 8 });
            Assert.AreNotSame(ints, dataGrid.ItemsSource);
            Assert.AreEqual(2, dataGrid.Columns.Count);
            Assert.AreEqual(4, dataGrid.Items.Count);

            Assert.AreEqual(1, dataGrid.GetCellValue(0, 0));
            Assert.AreEqual(2, dataGrid.GetCellValue(0, 1));

            Assert.AreEqual(3, dataGrid.GetCellValue(1, 0));
            Assert.AreEqual(4, dataGrid.GetCellValue(1, 1));

            Assert.AreEqual(5, dataGrid.GetCellValue(2, 0));
            Assert.AreEqual(6, dataGrid.GetCellValue(2, 1));

            // Need to force an update to test below.
            ////Assert.AreEqual(7, dataGrid.GetCellValue(3, 0));
            ////Assert.AreEqual(8, dataGrid.GetCellValue(3, 1));
        }

        [Test]
        public static void ColumnsSource()
        {
            var ints = new ObservableCollection<ObservableCollection<int>>
                           {
                               new ObservableCollection<int>(new[] { 1, 2 }),
                               new ObservableCollection<int>(new[] { 3, 4 }),
                               new ObservableCollection<int>(new[] { 5, 6 }),
                           };
            var dataGrid = new DataGrid();
            dataGrid.SetValue(ItemsSource.ColumnsSourceProperty, ints);
            dataGrid.Initialize();
            Assert.IsInstanceOf<Lists2DTransposedView>(dataGrid.ItemsSource);

            Assert.AreEqual(1, dataGrid.GetCellValue(0, 0));
            Assert.AreEqual(3, dataGrid.GetCellValue(0, 1));
            Assert.AreEqual(5, dataGrid.GetCellValue(0, 2));

            Assert.AreEqual(2, dataGrid.GetCellValue(1, 0));
            Assert.AreEqual(4, dataGrid.GetCellValue(1, 1));
            Assert.AreEqual(6, dataGrid.GetCellValue(1, 2));
        }

        [Test]
        public static void TransposedSource()
        {
            var persons = new ObservableCollection<Person> { new Person { FirstName = "Johan", LastName = "Larsson" } };
            var dataGrid = new DataGrid();
            dataGrid.SetValue(ItemsSource.TransposedSourceProperty, persons);
            dataGrid.Initialize();

            var itemsSource = dataGrid.ItemsSource;
            Assert.IsInstanceOf<TransposedItemsSource>(itemsSource);
            Assert.AreEqual(2, dataGrid.Columns.Count);
            Assert.AreEqual(2, dataGrid.Items.Count);

            Assert.AreEqual("FirstName", dataGrid.GetCellValue(0, 0));
            Assert.AreEqual("Johan", dataGrid.GetCellValue(0, 1));

            Assert.AreEqual("LastName", dataGrid.GetCellValue(1, 0));
            Assert.AreEqual("Larsson", dataGrid.GetCellValue(1, 1));

            persons.Add(new Person { FirstName = "Erik", LastName = "Svensson" });
            Assert.AreNotSame(itemsSource, dataGrid.ItemsSource);
            Assert.AreEqual(3, dataGrid.Columns.Count);
            Assert.AreEqual(2, dataGrid.Items.Count);

            /*
            Assert.AreEqual("FirstName", dataGrid.GetCellValue(0, 0));
            Assert.AreEqual("Johan", dataGrid.GetCellValue(0, 1));
            Assert.AreEqual("Erik", dataGrid.GetCellValue(0, 2));

            Assert.AreEqual("LastName", dataGrid.GetCellValue(1, 0));
            Assert.AreEqual("Larsson", dataGrid.GetCellValue(1, 1));
            Assert.AreEqual("Svensson", dataGrid.GetCellValue(1, 2));
            */
        }

        [Test]
        public static void EmptyRowsSource()
        {
            var ints = new ObservableCollection<ObservableCollection<int>>();

            var dataGrid = new DataGrid();
            dataGrid.SetValue(ItemsSource.RowsSourceProperty, ints);
            dataGrid.Initialize();

            ints.Add(new ObservableCollection<int> { 0, 0, 0 });
            ints.Add(new ObservableCollection<int> { 1, 2, 3 });

            dataGrid.UpdateLayout();

            Assert.AreEqual(0, dataGrid.GetCellValue(0, 0));
            Assert.AreEqual(0, dataGrid.GetCellValue(0, 2));

            Assert.AreEqual(1, dataGrid.GetCellValue(1, 0));
            Assert.AreEqual(2, dataGrid.GetCellValue(1, 1));
        }

        [Test]
        public static void EmptyColumnsSource()
        {
            var ints = new ObservableCollection<ObservableCollection<int>>();

            var dataGrid = new DataGrid();
            dataGrid.SetValue(ItemsSource.ColumnsSourceProperty, ints);
            dataGrid.Initialize();

            ints.Add(new ObservableCollection<int>());
            ints.Add(new ObservableCollection<int> { 1, 2, 3 });
            ints.Clear();
            ints.Add(new ObservableCollection<int> { 4, 5, 6 });
            ints.Add(new ObservableCollection<int> { 7, 8, 9 });

            dataGrid.UpdateLayout();

            Assert.AreEqual(4, dataGrid.GetCellValue(0, 0));
            Assert.AreEqual(5, dataGrid.GetCellValue(1, 0));

            Assert.AreEqual(6, dataGrid.GetCellValue(2, 0));
            Assert.AreEqual(9, dataGrid.GetCellValue(2, 1));
        }
    }
}
