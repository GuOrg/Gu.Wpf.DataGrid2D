namespace Gu.Wpf.DataGrid2D
{
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;

    public static class SingleClick
    {
        public static readonly DependencyProperty EditProperty = DependencyProperty.RegisterAttached(
            "Edit",
            typeof(bool),
            typeof(SingleClick),
            new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.Inherits));

        static SingleClick()
        {
            EventManager.RegisterClassHandler(typeof(DataGridCell), UIElement.PreviewMouseLeftButtonDownEvent, new RoutedEventHandler(OnPreviewMouseLeftButtonDown));
        }

        public static void SetEdit(DependencyObject element, bool value)
        {
            element.SetValue(EditProperty, value);
        }

        public static bool GetEdit(DependencyObject element)
        {
            return (bool)element.GetValue(EditProperty);
        }

        private static void OnPreviewMouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            var cell = (DataGridCell)sender;
            if (GetEdit(cell) && !cell.IsReadOnly && !cell.IsEditing)
            {
                var dataGrid = cell.Ancestors()
                                   .OfType<DataGrid>()
                                   .FirstOrDefault();
                if (dataGrid == null)
                {
                    return;
                }

                dataGrid.BeginEdit();
                if (dataGrid.SelectionUnit == DataGridSelectionUnit.FullRow)
                {
                    cell.Ancestors()
                        .OfType<DataGridRow>()
                        .FirstOrDefault()
                        ?.SetCurrentValue(DataGridRow.IsSelectedProperty, true);
                }
                else
                {
                    cell.SetCurrentValue(DataGridCell.IsSelectedProperty, true);
                }

                cell.Focus();
            }
        }
    }
}
