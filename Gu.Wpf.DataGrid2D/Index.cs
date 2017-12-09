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

        /// <summary>
        /// Helper for setting OfRow property on a DataGridRow.
        /// </summary>
        /// <param name="element">DataGridRow to set OfRow property on.</param>
        /// <param name="value">OfRow property value.</param>
        public static void SetOfRow(this DataGridRow element, int value)
        {
            element.SetValue(OfRowPropertyKey, value);
        }

        /// <summary>
        /// Helper for reading OfRow property from a DataGridRow.
        /// </summary>
        /// <param name="element">DataGridRow to read OfRow property from.</param>
        /// <returns>OfRow property value.</returns>
        [AttachedPropertyBrowsableForChildren(IncludeDescendants = false)]
        [AttachedPropertyBrowsableForType(typeof(DataGridRow))]
        public static int GetOfRow(this DataGridRow element)
        {
            return (int)element.GetValue(OfRowProperty);
        }

#pragma warning disable WPF0013 // CLR accessor for attached property must match registered type.
                               /// <summary>
                               /// Helper for setting StartAt property on a Control.
                               /// </summary>
                               /// <param name="element">Control to set StartAt property on.</param>
                               /// <param name="value">StartAt property value.</param>
        public static void SetStartAt(this Control element, int value)
        {
            element.SetValue(StartAtProperty, value);
        }

        /// <summary>
        /// Helper for reading StartAt property from a Control.
        /// </summary>
        /// <param name="element">Control to read StartAt property from.</param>
        /// <returns>StartAt property value.</returns>
        [AttachedPropertyBrowsableForChildren(IncludeDescendants = false)]
        [AttachedPropertyBrowsableForType(typeof(DataGridRow))]
        [AttachedPropertyBrowsableForType(typeof(DataGrid))]
        public static int GetStartAt(this Control element)
        {
            return (int)(element.GetValue(StartAtProperty) ?? 0);
        }
#pragma warning restore WPF0013 // CLR accessor for attached property must match registered type.

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

                dataGrid.RemoveHandler(Events.RowsChangedEvent, OnRowsChangedHandler);
                return;
            }

            dataGrid.SetCurrentValue(RowsListenerProperty, new RowsListener(dataGrid));
            dataGrid.UpdateHandler(Events.RowsChangedEvent, OnRowsChangedHandler);
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
