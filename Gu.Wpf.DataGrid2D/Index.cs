#pragma warning disable SA1202
namespace Gu.Wpf.DataGrid2D
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;

    public static class Index
    {
        private static readonly DependencyPropertyKey OfRowPropertyKey = DependencyProperty.RegisterAttachedReadOnly(
            "OfRow",
            typeof(int),
            typeof(Index),
            new PropertyMetadata(-1, null));

        public static readonly DependencyProperty OfRowProperty = OfRowPropertyKey.DependencyProperty;

        public static readonly DependencyProperty StartAtProperty = DependencyProperty.RegisterAttached(
            "StartAt",
            typeof(int?),
            typeof(Index),
            new PropertyMetadata(null, OnStartAtChanged));

        private static readonly DependencyProperty RowsListenerProperty = DependencyProperty.RegisterAttached(
            "RowsListener",
            typeof(RowsListener),
            typeof(Index),
            new PropertyMetadata(default(RowsListener)));

        private static readonly RoutedEventHandler OnRowsChangedHandler = OnRowsChanged;

        public static void SetOfRow(this DataGridRow element, int value)
        {
            element.SetValue(OfRowPropertyKey, value);
        }

        [AttachedPropertyBrowsableForChildren(IncludeDescendants = false)]
        [AttachedPropertyBrowsableForType(typeof(DataGridRow))]
        public static int GetOfRow(this DataGridRow element)
        {
            return (int)element.GetValue(OfRowProperty);
        }

        public static void SetStartAt(this Control element, int value)
        {
            element.SetValue(StartAtProperty, value);
        }

        [AttachedPropertyBrowsableForChildren(IncludeDescendants = false)]
        [AttachedPropertyBrowsableForType(typeof(DataGridRow))]
        [AttachedPropertyBrowsableForType(typeof(DataGrid))]
        public static int GetStartAt(this Control element)
        {
            return (int)element.GetValue(StartAtProperty);
        }

        private static void OnStartAtChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var dataGrid = (DataGrid)d;
            (dataGrid.GetValue(RowsListenerProperty) as IDisposable)?.Dispose();
            dataGrid.ClearValue(RowsListenerProperty);

            if (e.NewValue == null)
            {
                foreach (DataGridRow row in dataGrid.Items)
                {
                    BindingOperations.ClearBinding(row, DataGridRow.HeaderProperty);
                }

                dataGrid.RemoveHandler(Events.RowsChanged, OnRowsChangedHandler);
                return;
            }

            dataGrid.SetValue(RowsListenerProperty, new RowsListener(dataGrid));
            dataGrid.UpdateHandler(Events.RowsChanged, OnRowsChangedHandler);
            OnRowsChanged(dataGrid, null);
        }

        private static void OnRowsChanged(object sender, RoutedEventArgs routedEventArgs)
        {
            var dataGrid = (DataGrid)sender;
            var startAt = dataGrid.GetStartAt();
            for (int index = 0; index < dataGrid.ItemContainerGenerator.Items.Count; index++)
            {
                var row = (DataGridRow)dataGrid.ItemContainerGenerator.ContainerFromIndex(index);
                row?.SetOfRow(index + startAt);
            }
        }
    }
}
