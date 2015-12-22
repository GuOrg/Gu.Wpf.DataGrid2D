namespace Gu.Wpf.DataGrid2D.Demo
{
    using System.Runtime.CompilerServices;

    public static class AutomationIds
    {
        public static readonly string MainWindow = Create();
        public static readonly string MultiDimensionalTab = Create();
        public static readonly string MultiDimensionalAutoColumns = Create();
        public static readonly string MultiDimensionalExplicitColumns = Create();
        public static readonly string MultiDimensionalWithHeaders = Create();

        private static string Create([CallerMemberName] string name = null)
        {
            return name;
        }
    }
}
