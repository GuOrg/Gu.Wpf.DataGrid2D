namespace Gu.Wpf.DataGrid2D.Demo
{
    public class JaggedVm
    {
        public JaggedVm()
        {
            this.RowHeaders = new[] { "1", "2", "3" };
            this.ColumnHeaders = new[] { "A", "B", "C" };

            this.SameLengths = new int[3][];
            this.SameLengths[0] = new[] { 1, 2 };
            this.SameLengths[1] = new[] { 3, 4 };
            this.SameLengths[2] = new[] { 5, 6 };

            this.DifferentLengths = new int[3][];
            this.DifferentLengths[0] = new[] { 1 };
            this.DifferentLengths[1] = new[] { 2, 3 };
            this.DifferentLengths[2] = new[] { 4, 5, 6 };
        }

        public string[] RowHeaders { get; }

        public string[] ColumnHeaders { get; }

        public int[][] SameLengths { get; }

        public int[][] DifferentLengths { get; }
    }
}
