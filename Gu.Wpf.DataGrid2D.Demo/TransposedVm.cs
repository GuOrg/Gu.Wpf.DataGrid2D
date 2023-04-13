namespace Gu.Wpf.DataGrid2D.Demo
{
    using System.Collections.ObjectModel;

    public class TransposedVm
    {
        public TransposedVm()
        {
            this.Persons.Add(this.Person);
            this.Persons.Add(new Person { FirstName = "Erik", LastName = "Svensson" });
        }

        public ObservableCollection<Person> Persons { get; } = new();

        public Person Person { get; } = new() { FirstName = "Johan", LastName = "Larsson" };
    }
}
