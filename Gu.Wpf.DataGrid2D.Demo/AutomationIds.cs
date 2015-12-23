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

        public static readonly string JaggedTab = Create();
        public static readonly string JaggedAutoColumns = Create();
        public static readonly string JaggedAutoColumnsDifferentLengths = Create();
        public static readonly string JaggedExplicitColumns = Create();
        public static readonly string JaggedWithHeaders = Create();
        public static readonly string JaggedExplicitColumnsTransposed = Create();
        public static readonly string JaggedWithHeadersTransposed = Create();
        public static readonly string JaggedAutoColumnsTransposed = Create();
        public static readonly string JaggedAutoColumnsDifferentLengthsTransposed = Create();

        public static readonly string TransposedTab = Create();
        public static readonly string TransposedExplicitColumns = Create();
        public static readonly string TransposedSingleton = Create();
        public static readonly string TransposedObservableCollection = Create();
        public static readonly string ReferenceDataGrid = Create();

        public static readonly string SelectionTab = Create();
        public static readonly string SelectionGrid = Create();
        public static readonly string SelectedIndex = Create();
        public static readonly string SelectedItem = Create();
        public static readonly string SelectionLoseFocusButton = Create();
        public static readonly string SelectionList = Create();


        private static string Create([CallerMemberName] string name = null)
        {
            return name;
        }
    }
}
