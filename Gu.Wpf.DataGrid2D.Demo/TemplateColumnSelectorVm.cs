namespace Gu.Wpf.DataGrid2D.Demo
{
    using System.Collections.ObjectModel;

    public class TemplateColumnSelectorVm
    {
        public TemplateColumnSelectorVm()
        {
            this.RowHeaders = new ObservableCollection<string>(new[] { "1", "2", "3" });
            this.ColumnHeaders = new ObservableCollection<string>(new[] { "A", "B", "C" });
            this.ListOfListsOfItems = new ObservableCollection<ObservableCollection<ItemVm>>();
            var count = 1;
            for (int i = 0; i < 3; i++)
            {
                var itemRow = new ObservableCollection<ItemVm>();
                this.ListOfListsOfItems.Add(itemRow);
                for (int j = 0; j < 2; j++)
                {
                    var itemVm = new ItemVm(count);
                    itemRow.Add(itemVm);
                    count++;
                }
            }
        }

        public ObservableCollection<string> RowHeaders { get; }

        public ObservableCollection<string> ColumnHeaders { get; }

        public ObservableCollection<ObservableCollection<ItemVm>> ListOfListsOfItems { get; }
    }
}
