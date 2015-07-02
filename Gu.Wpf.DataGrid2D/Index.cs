namespace Gu.Wpf.DataGrid2D
{
    using System.Windows;
    using System.Windows.Controls;

    public static class Index
    {
        private static readonly DependencyPropertyKey OfPropertyKey = DependencyProperty.RegisterAttachedReadOnly(
            "Of",
            typeof(int),
            typeof(Index),
            new PropertyMetadata(-1));

        public static readonly DependencyProperty OfProperty = OfPropertyKey.DependencyProperty;

        public static readonly DependencyProperty InProperty = DependencyProperty.RegisterAttached(
            "In",
            typeof(DataGrid),
            typeof(Index),
            new PropertyMetadata(default(DataGrid), OnInChanged));

        public static readonly DependencyProperty StartAtProperty = DependencyProperty.RegisterAttached(
            "StartAt",
            typeof (int),
            typeof (Index),
            new PropertyMetadata(default(int)));

        public static void SetOf(this DataGridRow element, int value)
        {
            element.SetValue(OfPropertyKey, value);
        }

        [AttachedPropertyBrowsableForChildren(IncludeDescendants = false)]
        [AttachedPropertyBrowsableForType(typeof(DataGridRow))]
        public static int GetOf(this DataGridRow element)
        {
            return (int)element.GetValue(OfProperty);
        }

        public static void SetIn(this DataGridRow element, DataGrid value)
        {
            element.SetValue(InProperty, value);
        }

        [AttachedPropertyBrowsableForChildren(IncludeDescendants = false)]
        [AttachedPropertyBrowsableForType(typeof(DataGridRow))]
        public static DataGrid GetIn(this DataGridRow element)
        {
            return (DataGrid)element.GetValue(InProperty);
        }

        public static void SetStartAt(this DataGridRow element, int value)
        {
            element.SetValue(StartAtProperty, value);
        }

        [AttachedPropertyBrowsableForChildren(IncludeDescendants = false)]
        [AttachedPropertyBrowsableForType(typeof(DataGridRow))]
        public static int GetStartAt(this DataGridRow element)
        {
            return (int)element.GetValue(StartAtProperty);
        }

        private static void OnInChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var row = (DataGridRow)d;
            row.SetOf(row.GetIndex() + row.GetStartAt());
        }
    }
}
