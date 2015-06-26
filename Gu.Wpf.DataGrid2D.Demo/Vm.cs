namespace Gu.Wpf.DataGrid2D.Demo
{
    using System.Collections.Generic;
    using System.Linq;

    public class Vm
    {
        public Vm()
        {
            Data = new[,] { { 1, 2 }, { 3, 4 } };
            ColumnHeaders = new[] { "Col1", "Col2" };
            Rows = new int[2][];
            Rows[0] = new[] { 1, 2 };
            Rows[1] = new[] { 3, 4 };

            int count = 1;
            Items = new List<List<ItemVm>>();
            for (int i = 0; i < 2; i++)
            {
                var row = new List<ItemVm>();
                Items.Add(row);
                for (int j = 3; j < 4; j++)
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
        
        public int[][] Rows { get; private set; }

        public List<List<ItemVm>> Items { get; private set; } 

        public int[,] Data { get; private set; }
    }
}
