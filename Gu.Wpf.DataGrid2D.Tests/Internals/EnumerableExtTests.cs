namespace Gu.Wpf.DataGrid2D.Tests.Internals
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using NUnit.Framework;

    public class EnumerableExtTests
    {
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
