namespace Gu.Wpf.DataGrid2D.Demo
{
    using System.Collections.ObjectModel;
    using System.Linq;

    public class RowNumbersVm
    {
        private static readonly string[] FirstNames = { "Johan", "Erik", "Lynn" };
        private static readonly string[] LastNames = { "Larsson", "Svensson" };

        public ObservableCollection<Person> Persons { get; } = CreatePersons(100);

        private static ObservableCollection<Person> CreatePersons(int n)
        {
            var persons = Enumerable.Range(0, n)
                                    .Select(x => new Person { FirstName = FirstNames[x % 3], LastName = LastNames[x % 2] });
            return new ObservableCollection<Person>(persons);
        }
    }
}
