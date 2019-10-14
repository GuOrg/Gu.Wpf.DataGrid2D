namespace Gu.Wpf.DataGrid2D.Tests.Internals
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using NUnit.Framework;

    public static class EnumerableExtTests
    {
        [Test]
        public static void EnumerableEnumerableIntIsReadOnly()
        {
            var enumerable = Enumerable.Repeat(Enumerable.Repeat(1, 1), 1);
            Assert.AreEqual(true, enumerable.IsReadOnly());
        }

        [Test]
        public static void EnumerableEnumerableDummyItemIsEditable()
        {
            var enumerable = Enumerable.Repeat(Enumerable.Repeat(new DummyItem(), 1), 1);
            Assert.AreEqual(false, enumerable.IsReadOnly());
        }

        [Test]
        public static void EnumerableIListIsEditable()
        {
            var enumerable = Enumerable.Repeat(new int[1], 1);
            Assert.AreEqual(false, enumerable.IsReadOnly());
        }

        [Test]
        public static void SetElementAtArray()
        {
            var ints = new[] { 1, 2 };
            ints.SetElementAt(0, 5);
            CollectionAssert.AreEqual(new[] { 5, 2 }, ints);
        }

        [Test]
        public static void SetElementAtList()
        {
            var ints = new List<int> { 1, 2 };
            ints.SetElementAt(0, 5);
            CollectionAssert.AreEqual(new[] { 5, 2 }, ints);
        }

        [Test]
        public static void SetElementAtObservableCollection()
        {
            var ints = new ObservableCollection<int> { 1, 2 };
            ints.SetElementAt(0, 5);
            CollectionAssert.AreEqual(new[] { 5, 2 }, ints);
        }
    }
}
