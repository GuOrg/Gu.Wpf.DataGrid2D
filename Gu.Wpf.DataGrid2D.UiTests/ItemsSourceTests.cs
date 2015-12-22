namespace Gu.Wpf.DataGrid2D.UiTests
{
    using System.Diagnostics;
    using System.IO;
    using System.Reflection;
    using Gu.Wpf.DataGrid2D.Demo;
    using NUnit.Framework;
    using TestStack.White;
    using TestStack.White.Factory;
    using TestStack.White.UIItems;
    using TestStack.White.UIItems.Finders;
    using TestStack.White.UIItems.TabItems;

    public partial class ItemsSourceTests
    {
        private static ProcessStartInfo ProcessStartInfo
        {
            get
            {
                var fileName = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly()
                                                                          .Location),
                                            $"{typeof (MainWindow).Assembly.GetName() .Name}.exe");
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
