namespace Gu.Wpf.DataGrid2D.Demo
{
    public class Source2DVm
    {
        public Source2DVm()
        {
            this.Data2D = new[,] { { 1, 2 }, { 3, 4 }, { 5, 6 } };
            this.ColumnHeaders = new[] { "Col1", "Col2" };
        }

        public string[] ColumnHeaders { get; }

        public int[,] Data2D { get; }
    }
}
