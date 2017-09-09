namespace Gu.Wpf.DataGrid2D.UiTests
{
    using Gu.Wpf.UiAutomation;
    using NUnit.Framework;

    public class MainWindowTests
    {
        [Test]
        public void ClickAllTabs()
        {
            // Just a smoke test so that we do not explode.
            using (var app = Application.Launch(Info.ExeFileName))
            {
                var window = app.MainWindow;
                Assert.AreEqual("MainWindow", window.Title);
                var tab = window.FindTabControl();
                foreach (var tabItem in tab.Items)
                {
                    tabItem.Click();
                }
            }
        }
    }
}