namespace Gu.Wpf.DataGrid2D.UiTests
{
    using System.Linq;
    using Gu.Wpf.UiAutomation;

    public static class Utils
    {
        public static TabItem FindTabItem(this Window window, string nameOrHeader, string tabControlName = "")
        {
            return
                window
                  .FindTabControl(tabControlName)
                  .Items
                  .SingleOrDefault(ti => ti.Name == nameOrHeader || ti.HeaderText == nameOrHeader);
        }
    }
}
