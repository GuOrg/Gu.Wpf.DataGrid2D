namespace Gu.Wpf.DataGrid2D.Demo
{
    using System.Runtime.CompilerServices;

    public static class AutomationIds
    {
        public static readonly string MainWindow = Create();
        public static readonly string AutoColumns = Create();
        public static readonly string ExplicitColumns = Create();
        public static readonly string WithHeaders = Create();
        public static readonly string AutoColumnsTransposed = Create();
        public static readonly string ExplicitColumnsTransposed = Create();
        public static readonly string WithHeadersTransposed = Create();
        public static readonly string DifferentLengthsTransposed = Create();

        public static readonly string MultiDimensionalTab = Create();

        public static readonly string JaggedTab = Create();
        public static readonly string AutoColumnsDifferentLengths = Create();


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

        public static readonly string UpdateDataButton = Create();
        public static readonly string DataTextBox = Create();


        private static string Create([CallerMemberName] string name = null)
        {
            return name;
        }
    }
}
