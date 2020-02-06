namespace Gu.Wpf.DataGrid2D.Tests.Views
{
    using System.Collections.ObjectModel;
    using NUnit.Framework;

    public static partial class Lists2DTransposedViewTests
    {
        public static class ColumnsChanged
        {
            [Test]
            public static void SignalsWhenColumnIsAdded()
            {
                var ints = new ObservableCollection<ObservableCollection<int>>
                {
                    new ObservableCollection<int>(new[] { 1, 2 }),
                    new ObservableCollection<int>(new[] { 3, 4 }),
                };
                using var view = new Lists2DTransposedView(ints);
                var count = 0;
                view.ColumnsChanged += (_, __) => count++;
                ints.Add(new ObservableCollection<int>(new[] { 5, 6 }));
                Assert.AreEqual(1, count);
            }

            [Test]
            public static void SignalsWhenColumnIsRemoved()
            {
                var ints = new ObservableCollection<ObservableCollection<int>>
                {
                    new ObservableCollection<int>(new[] { 2 }),
                    new ObservableCollection<int>(new[] { 3 }),
                    new ObservableCollection<int>(new[] { 5, 6 }),
                };
                using var view = new Lists2DTransposedView(ints);
                var count = 0;
                view.ColumnsChanged += (_, __) => count++;
                ints.RemoveAt(0);
                Assert.AreEqual(1, count);
            }

            [Test]
            public static void SignalsWhenColumnBecomesReadOnly()
            {
                var ints = new ObservableCollection<ObservableCollection<int>>
                {
                    new ObservableCollection<int>(new[] { 1, 2 }),
                    new ObservableCollection<int>(new[] { 3, 4 }),
                    new ObservableCollection<int>(new[] { 5, 6 }),
                };
                using var view = new Lists2DTransposedView(ints);
                var count = 0;
                view.ColumnsChanged += (_, __) => count++;
                ints[0].RemoveAt(0);
                Assert.AreEqual(1, count);
            }

            [Test]
            public static void SignalsWhenColumnBecomesReadOnly2()
            {
                var ints = new ObservableCollection<ObservableCollection<int>>
                {
                    new ObservableCollection<int>(new[] { 2, 3 }),
                    new ObservableCollection<int>(new[] { 3, 4 }),
                    new ObservableCollection<int>(new[] { 5, 6 }),
                };
                using var view = new Lists2DTransposedView(ints);
                var count = 0;
                view.ColumnsChanged += (_, __) => count++;
                ints[0].Add(1);
                Assert.AreEqual(1, count);
            }

            [Test]
            public static void SignalsWhenColumnBecomesEditable()
            {
                var ints = new ObservableCollection<ObservableCollection<int>>
                {
                    new ObservableCollection<int>(new[] { 2 }),
                    new ObservableCollection<int>(new[] { 3, 4 }),
                    new ObservableCollection<int>(new[] { 5, 6 }),
                };
                using var view = new Lists2DTransposedView(ints);
                var count = 0;
                view.ColumnsChanged += (_, __) => count++;
                ints[0].Add(9);
                Assert.AreEqual(1, count);
            }

            [Test]
            public static void SignalsWhenColumnsBecomesEditable()
            {
                var ints = new ObservableCollection<ObservableCollection<int>>
                {
                    new ObservableCollection<int>(new[] { 2 }),
                    new ObservableCollection<int>(new[] { 3 }),
                    new ObservableCollection<int>(new[] { 5, 6 }),
                };
                using var view = new Lists2DTransposedView(ints);
                var count = 0;
                view.ColumnsChanged += (_, __) => count++;
                ints.RemoveAt(2);
                Assert.AreEqual(1, count);
            }

            [Test]
            public static void NoChangeWhenAlreadyReadOnly()
            {
                var ints = new ObservableCollection<ObservableCollection<int>>
                {
                    new ObservableCollection<int>(new[] { 1, 2 }),
                    new ObservableCollection<int>(new[] { 3, 4, 5 }),
                };
                using var view = new Lists2DTransposedView(ints);
                var count = 0;
                view.ColumnsChanged += (_, __) => count++;
                ints[0].RemoveAt(0);
                Assert.AreEqual(0, count);
            }

            [Test]
            public static void NoChangeWhenUpdateCell()
            {
                var ints = new ObservableCollection<ObservableCollection<int>>
                {
                    new ObservableCollection<int>(new[] { 1 }),
                    new ObservableCollection<int>(new[] { 3, 4 }),
                    new ObservableCollection<int>(new[] { 5, 6 }),
                };
                using var view = new Lists2DTransposedView(ints);
                var count = 0;
                view.ColumnsChanged += (_, __) => count++;
                ints[0][0]++;
                Assert.AreEqual(0, count);

                ints[1][0]++;
                Assert.AreEqual(0, count);

                ints[2][1]++;
                Assert.AreEqual(0, count);
            }
        }
    }
}
