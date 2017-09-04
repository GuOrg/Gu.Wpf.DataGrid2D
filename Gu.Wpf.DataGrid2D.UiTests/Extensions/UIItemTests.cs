namespace Gu.Wpf.DataGrid2D.UiTests
{
    using System;

    using Gu.Wpf.DataGrid2D.Demo;

    using NUnit.Framework;

    using TestStack.White;
    using TestStack.White.Factory;
    using TestStack.White.UIItems;
    using TestStack.White.UIItems.TabItems;

    [Explicit]
    public class UIItemTests
    {
        [Test]
        public void Dump()
        {
            using (var app = Application.AttachOrLaunch(Info.ProcessStartInfo))
            {
                var window = app.GetWindow("MainWindow", InitializeOption.NoCache);
                var page = window.Get<TabPage>("SelectionTab");
                page.Select();
                var dataGrid = page.Get<ListView>("SelectionGrid");
                Console.WriteLine(dataGrid.GetType());
                dataGrid.DumpSupportedPatterns();
                dataGrid.DumpSupportedProperties();

                Console.WriteLine();
                var row = dataGrid.Rows[0];
                Console.WriteLine(row.GetType());
                row.DumpSupportedPatterns();
                row.DumpSupportedProperties();

                Console.WriteLine();
                var cell = row.Cells[0];
                Console.WriteLine(cell.GetType());
                cell.DumpSupportedPatterns();
                cell.DumpSupportedProperties();
            }
        }
    }
}
