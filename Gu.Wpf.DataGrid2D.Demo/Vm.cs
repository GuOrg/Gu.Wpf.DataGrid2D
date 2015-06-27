namespace Gu.Wpf.DataGrid2D.Demo
{
    using System.Collections.Generic;
    using System.Linq;

    public class Vm
    {
        public Vm()
        {
            Data2D = new[,] { { 1, 2 }, { 3, 4 }, { 5, 6 } };
            ColumnHeaders = new[] { "Col1", "Col2" };
            JaggedRows = new int[3][];
            JaggedRows[0] = new[] { 1, 2 };
            JaggedRows[1] = new[] { 3, 4 };
            JaggedRows[2] = new[] { 5, 6 };

            int count = 1;
            ListOfListsOfItems = new List<List<ItemVm>>();
            for (int i = 0; i < 3; i++)
            {
                var row = new List<ItemVm>();
                ListOfListsOfItems.Add(row);
                for (int j = 0; j < 2; j++)
                {
                    row.Add(new ItemVm(count));
                    count++;
                }
            }

            ColumnItemHeaders = Enumerable.Range(0, 2)
                                          .Select(x => new ItemVm(x))
                                          .ToArray();
        }

        public string[] ColumnHeaders { get; private set; }
       
        public ItemVm[] ColumnItemHeaders { get; private set; }
        
        public int[][] JaggedRows { get; private set; }

        public List<List<ItemVm>> ListOfListsOfItems { get; private set; } 

        public int[,] Data2D { get; private set; }
    }
}
