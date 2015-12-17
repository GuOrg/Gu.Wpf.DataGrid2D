namespace Gu.Wpf.DataGrid2D.Demo
{
    public class JaggedSourcesVm
    {
        public JaggedSourcesVm()
        {
            this.RowHeaders = new[] { "1", "2", "3" };
            this.ColumnHeaders = new[] { "AA", "AB", "AC" };

            this.JaggedRows = new int[3][];
            this.JaggedRows[0] = new[] { 1, 2 };
            this.JaggedRows[1] = new[] { 3, 4 };
            this.JaggedRows[2] = new[] { 5, 6 };

            this.DifferentLengths = new int[3][];
            this.DifferentLengths[0] = new[] { 1 };
            this.DifferentLengths[1] = new[] { 2, 3 };
            this.DifferentLengths[2] = new[] { 4, 5, 6 };
        }

        public string[] RowHeaders { get; }

        public string[] ColumnHeaders { get; }

        public int[][] JaggedRows { get; }

        public int[][] DifferentLengths { get; }
    }
}
