namespace Gu.Wpf.DataGrid2D.Demo;

using System.Collections.ObjectModel;

public class ObservableVm
{
    public ObservableVm()
    {
        this.RowHeaders = new ObservableCollection<string>(new[] { "1", "2", "3" });
        this.ColumnHeaders = new ObservableCollection<string>(new[] { "A", "B", "C" });
        this.ListOfListsOfItems = new ObservableCollection<ObservableCollection<ItemVm>>();
        this.ListOfListsOfInts = new ObservableCollection<ObservableCollection<int>>();
        var count = 1;
        for (int i = 0; i < 3; i++)
        {
            var itemRow = new ObservableCollection<ItemVm>();
            var intRow = new ObservableCollection<int>();
            this.ListOfListsOfItems.Add(itemRow);
            this.ListOfListsOfInts.Add(intRow);
            for (int j = 0; j < 2; j++)
            {
                var itemVm = new ItemVm(count);
                itemRow.Add(itemVm);
                intRow.Add(count);
                count++;
            }
        }
    }

    public ObservableCollection<string> RowHeaders { get; }

    public ObservableCollection<string> ColumnHeaders { get; }

    public ObservableCollection<ObservableCollection<ItemVm>> ListOfListsOfItems { get; }

    public ObservableCollection<ObservableCollection<int>> ListOfListsOfInts { get; }
}
