namespace Gu.Wpf.DataGrid2D.Demo
{
    using System.Collections.ObjectModel;

    public class RowNumbersVm
    {
        public ObservableCollection<Person> Persons { get; } = new ObservableCollection<Person>
                                                               {
                                                                   new Person { FirstName = "Johan", LastName = "Larsson" },
                                                                   new Person { FirstName = "Erik", LastName = "Svensson" },
                                                                   new Person { FirstName = "Robert", LastName = "Johnsson" },
                                                               };
    }
}
