namespace Gu.Wpf.DataGrid2D.Tests.Internals
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using NUnit.Framework;

    public class EnumerableExtTests
    {
        [Test]
        public void EnumerableEnumerableIntIsReadOnly()
        {
            var enumerable = Enumerable.Repeat(Enumerable.Repeat(1, 1), 1);
            Assert.AreEqual(true, enumerable.IsReadOnly());
        }

        [Test]
        public void EnumerableEnumerableDummyItemIsEditable()
        {
            var enumerable = Enumerable.Repeat(Enumerable.Repeat(new DummyItem(), 1), 1);
            Assert.AreEqual(false, enumerable.IsReadOnly());
        }

        [Test]
        public void EnumerableIListIsEditable()
        {
            var enumerable = Enumerable.Repeat(new int[1], 1);
            Assert.AreEqual(false, enumerable.IsReadOnly());
        }

        [Test]
        public void SetElementAtArray()
        {
            var ints = new[] { 1, 2 };
            ints.SetElementAt(0, 5);
            CollectionAssert.AreEqual(new[] { 5, 2 }, ints);
        }

        [Test]
        public void SetElementAtList()
        {
            var ints = new List<int> { 1, 2 };
            ints.SetElementAt(0, 5);
            CollectionAssert.AreEqual(new[] { 5, 2 }, ints);
        }

        [Test]
        public void SetElementAtObservableCollection()
        {
            var ints = new ObservableCollection<int> { 1, 2 };
            ints.SetElementAt(0, 5);
            CollectionAssert.AreEqual(new[] { 5, 2 }, ints);
        }
    }
}
