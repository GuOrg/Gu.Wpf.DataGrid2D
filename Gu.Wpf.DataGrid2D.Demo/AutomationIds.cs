namespace Gu.Wpf.DataGrid2D.Demo
{
    using System.Runtime.CompilerServices;

    public static class AutomationIds
    {
        public static readonly string MainWindow = Create();
        public static readonly string VanillaGroupBox = Create();
        public static readonly string MultiDimensionalTab = Create();
        public static readonly string MultiDimensionalAutoColumns = Create();

        private static string Create([CallerMemberName] string name = null)
        {
            return name;
        }
    }
}
