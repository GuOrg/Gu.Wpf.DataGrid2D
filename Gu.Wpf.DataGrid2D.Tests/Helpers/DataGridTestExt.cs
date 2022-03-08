namespace Gu.Wpf.DataGrid2D.Tests
{
    using System.ComponentModel;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;

    using NUnit.Framework;

    public static class DataGridTestExt
    {
        public static object GetCellValue(this DataGrid dataGrid, int row, int col)
        {
            var item = dataGrid.Items[row];
            var column = (DataGridTextColumn)dataGrid.Columns[col];
            var dataContext = column.GetCellContent(item)?.DataContext;
            Assert.NotNull(dataContext);
            var propName = ((Binding)column.Binding).Path.Path;
            var propertyInfo = TypeDescriptor.GetProperties(dataContext).OfType<PropertyDescriptor>().Single(x => x.Name == propName);
            return propertyInfo.GetValue(dataContext);
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
