namespace Gu.Wpf.DataGrid2D.UiTests
{
    using System;
    using System.Windows.Automation;
    using TestStack.White.UIItems;

    public static class ListViewExt
    {
        public static ListViewCell SelectedCell(this ListView view)
        {
            var rows = view.AutomationElement.GetSelection();
            if (rows == null || rows.Length != 1)
            {
                return null;
            }

            var row = rows[0];
            var cells = row.GetSelection();

            throw new NotImplementedException();
        }

        private static AutomationElement[] GetSelection(this AutomationElement element)
        {
            return (AutomationElement[])element.GetCurrentPropertyValue(SelectionPatternIdentifiers.SelectionProperty);
        }
    }
}