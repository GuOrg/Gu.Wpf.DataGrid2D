namespace Gu.Wpf.DataGrid2D.UiTests
{
    using System;
    using Gu.Wpf.DataGrid2D.Demo;

    public static class Info
    {
        public static string ExeFileName { get; } = new Uri(typeof(MainWindow).Assembly.CodeBase, UriKind.Absolute).LocalPath;
    }
}