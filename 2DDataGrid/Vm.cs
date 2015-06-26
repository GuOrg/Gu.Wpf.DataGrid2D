namespace _2DDataGrid
{
    public class Vm
    {
        public Vm()
        {
            Data = new[,] { { 1, 2 }, { 3, 4 } };
            ColumnHeaders = new[] { "Col1", "Col2" };
            Rows = new int[2][];
            Rows[0] = new[] { 1, 2 };
            Rows[1] = new[] { 3, 4 };
        }

        public string[] ColumnHeaders { get; private set; }
        
        public int[][] Rows { get; private set; }

        public int[,] Data { get; private set; }
    }
}
