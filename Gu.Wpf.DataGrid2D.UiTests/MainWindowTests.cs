namespace Gu.Wpf.DataGrid2D.UiTests
{
    using Gu.Wpf.UiAutomation;
    using NUnit.Framework;

    public class MainWindowTests
    {
        [Test]
        public void ClickAllTabs()
        {
            using (var app = Application.Launch(Info.ExeFileName))
            {
                var window = app.MainWindow;
                var tabControl = window.FindTabControl();
                foreach (var item in tabControl.Items)
                {
                    item.Click();
                    window.WaitUntilResponsive();
                }
            }
        }
    }
}