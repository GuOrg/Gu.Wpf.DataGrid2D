namespace Gu.Wpf.DataGrid2D.Tests
{
    using System.Windows;
    using System.Windows.Controls;

    public static class DataGridExt
    {
        public static string GetCellText(this DataGrid dataGrid, int row, int col)
        {
            var item = dataGrid.Items[row];
            var column = dataGrid.Columns[col];
            var cellContent = (TextBlock) column.GetCellContent(item);
            return cellContent?.Text;
        }

        public static void Initialize(this UIElement element)
        {
            var userControl = new UserControl { Content = element };
            userControl.BeginInit(); // hacking different things here
            userControl.Measure(new Size(100, 100));
            userControl.Arrange(new Rect(new Size(100, 100)));
            element.RaiseEvent(new RoutedEventArgs(FrameworkElement.LoadedEvent));
        }
    }
}