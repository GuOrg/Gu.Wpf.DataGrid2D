namespace Gu.Wpf.DataGrid2D.UiTests
{
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
                var fileName = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly()
                                                                          .Location),
                                            $"{typeof(MainWindow).Assembly.GetName().Name}.exe");
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