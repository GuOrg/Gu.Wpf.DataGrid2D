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
        public static readonly string MultiDimensionalExplicitColumnsTransposed = Create();
        public static readonly string MultiDimensionalWithHeadersTransposed = Create();
        public static readonly string MultiDimensionalAutoColumnsTransposed = Create();
        public static readonly string TransposedTab = Create();
        public static readonly string TransposedExplicitColumns = Create();
        public static readonly string TransposedSingleton = Create();
        public static readonly string TransposedObservableCollection = Create();
        public static readonly string ReferenceDataGrid = Create();

        private static string Create([CallerMemberName] string name = null)
        {
            return name;
        }
    }
}
