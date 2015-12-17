namespace Gu.Wpf.DataGrid2D.Tests
{
    using System.Windows.Controls;

    public static class DataGridExt
    {
        public static object GetValue(this DataGrid dataGrid, int row, int col)
        {
            var item = dataGrid.Items[row];
            var column = dataGrid.Columns[col];
            return column.GetCellContent(item);
        }
    }
}