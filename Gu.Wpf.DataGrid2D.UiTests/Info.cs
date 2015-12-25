namespace Gu.Wpf.DataGrid2D.UiTests
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Reflection;
    using Gu.Wpf.DataGrid2D.Demo;

    public static class Info
    {
        public static ProcessStartInfo ProcessStartInfo
        {
            get
            {
                var assembly = typeof(MainWindow).Assembly;
                var uri = new Uri(assembly.CodeBase, UriKind.Absolute);
                var fileName = uri.AbsolutePath;
                var processStartInfo = new ProcessStartInfo
                                       {
                                           FileName = fileName,
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