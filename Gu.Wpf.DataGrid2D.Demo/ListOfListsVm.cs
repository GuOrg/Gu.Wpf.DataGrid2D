namespace Gu.Wpf.DataGrid2D.Demo
{
    using System.Collections.Generic;

    public class ListOfListsVm
    {
        public ListOfListsVm()
        {
            this.RowHeaders = new[] { "1", "2" , "3"};
            this.ColumnHeaders = new[] { "AA", "AB" , "AC"};
            this.ListOfListsOfItems = new List<List<ItemVm>>();
            this.ListOfListsOfInts = new List<List<int>>();
            var count = 0;
            for (int i = 0; i < 3; i++)
            {
                var itemRow = new List<ItemVm>();
                var intRow = new List<int>();
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

        public object RowHeaders { get; }

        public string[] ColumnHeaders { get; }

        public List<List<ItemVm>> ListOfListsOfItems { get; }

        public List<List<int>> ListOfListsOfInts { get; }
    }
}
