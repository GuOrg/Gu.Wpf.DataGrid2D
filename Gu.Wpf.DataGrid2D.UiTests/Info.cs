namespace Gu.Wpf.DataGrid2D.UiTests
{
    using System;
    using System.Diagnostics;

    using Gu.Wpf.DataGrid2D.Demo;

    public static class Info
    {
        public static string ExeFileName { get; } = new Uri(typeof(MainWindow).Assembly.CodeBase, UriKind.Absolute).LocalPath;

        public static ProcessStartInfo ProcessStartInfo
        {
            get
            {
                var processStartInfo = new ProcessStartInfo
                                       {
                                           FileName = ExeFileName,
                                           UseShellExecute = false,
                                           CreateNoWindow = true,
                                           RedirectStandardOutput = true,
                                           RedirectStandardError = true
                                       };
                return processStartInfo;
            }
        }
    }
}