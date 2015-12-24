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
        public ObservableCollection<Person> Persons { get; } = new ObservableCollection<Person>();

        public Person Person { get; } = new Person { FirstName = "Johan", LastName = "Larsson" };
    }
}
