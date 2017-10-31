namespace Gu.Wpf.DataGrid2D.Tests.Views
{
    using Gu.Wpf.DataGrid2D.Tests.Views.Stubs;
    using NUnit.Framework;

    public class TransposedItemsSourceTests
    {
        [Test]
        public void Create()
        {
            var person = new Person { FirstName = "Johan", LastName = "Larsson" };
            var persons = new[] { person };
            using (var view = new TransposedItemsSource(persons))
            {
                Assert.AreEqual(2, view.Count);
                var row0 = view[0];
                Assert.AreEqual(typeof(string), row0.GetProperties()[0].ComponentType);
                Assert.AreEqual("Name", row0.GetProperties()[0].Name);
                Assert.AreEqual(nameof(Person.FirstName), row0.GetProperties()[0].GetValue(row0));

                Assert.AreEqual("C0", row0.GetProperties()[1].Name);
                Assert.AreEqual("Johan", row0.GetProperties()[1].GetValue(row0));

                var row1 = view[1];
                Assert.AreEqual(typeof(string), row1.GetProperties()[0].ComponentType);
                Assert.AreEqual("Name", row1.GetProperties()[0].Name);
                Assert.AreEqual(nameof(Person.LastName), row1.GetProperties()[0].GetValue(row1));

                Assert.AreEqual("C0", row1.GetProperties()[1].Name);
                Assert.AreEqual("Larsson", row1.GetProperties()[1].GetValue(row1));
            }
        }
    }
}
