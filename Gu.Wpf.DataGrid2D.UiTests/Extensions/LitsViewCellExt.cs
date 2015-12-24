namespace Gu.Wpf.DataGrid2D.UiTests
{
    using System;
    using System.Windows.Automation;
    using TestStack.White.UIItems;

    public static class LitsViewCellExt
    {
        public static bool IsSelected(this ListViewCell cell)
        {
            var listViewRow = cell.GetParent<ListViewRow>();
            if (!listViewRow.IsSelected)
            {
                return false;
            }

            var value = listViewRow.AutomationElement.GetCurrentPropertyValue(SelectionPatternIdentifiers.SelectionProperty);


            cell.ActionListener.ActionPerforming(cell);
            return (bool)cell.AutomationElement.GetCurrentPropertyValue(SelectionItemPattern.IsSelectedProperty);
        }
    }
}
