namespace Gu.Wpf.DataGrid2D
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;

    /// <summary>
    /// Attached properties for specifying cell index.
    /// </summary>
    public static class Index
    {
#pragma warning disable SA1202 // Elements must be ordered by access

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
            new PropertyMetadata(null, OnStartAtChanged),
            value => ((int?)value ?? 0) >= 0);

        private static readonly DependencyProperty RowsListenerProperty = DependencyProperty.RegisterAttached(
            "RowsListener",
            typeof(RowsListener),
            typeof(Index),
            new PropertyMetadata(default(RowsListener)));

#pragma warning restore SA1202 // Elements must be ordered by access

        private static readonly RoutedEventHandler OnRowsChangedHandler = OnRowsChanged;

        /// <summary>Helper for setting <see cref="OfRowPropertyKey"/> on <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="DataGridRow"/> to set <see cref="OfRowPropertyKey"/> on.</param>
        /// <param name="value">OfRow property value.</param>
        public static void SetOfRow(this DataGridRow element, int value)
        {
            if (element is null)
            {
                throw new ArgumentNullException(nameof(element));
            }

            element.SetValue(OfRowPropertyKey, value);
        }

        /// <summary>Helper for getting <see cref="OfRowProperty"/> from <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="DataGridRow"/> to read <see cref="OfRowProperty"/> from.</param>
        /// <returns>OfRow property value.</returns>
        [AttachedPropertyBrowsableForChildren(IncludeDescendants = false)]
        [AttachedPropertyBrowsableForType(typeof(DataGridRow))]
        public static int GetOfRow(this DataGridRow element)
        {
            if (element is null)
            {
                throw new ArgumentNullException(nameof(element));
            }

            return (int)element.GetValue(OfRowProperty);
        }

#pragma warning disable WPF0013 // CLR accessor for attached property must match registered type.

        /// <summary>Helper for setting <see cref="StartAtProperty"/> on <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="Control"/> to set <see cref="StartAtProperty"/> on.</param>
        /// <param name="value">StartAt property value.</param>
        public static void SetStartAt(this Control element, int? value)
        {
            if (element is null)
            {
                throw new ArgumentNullException(nameof(element));
            }

            element.SetValue(StartAtProperty, value);
        }

        /// <summary>Helper for getting <see cref="StartAtProperty"/> from <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="Control"/> to read <see cref="StartAtProperty"/> from.</param>
        /// <returns>StartAt property value.</returns>
        [AttachedPropertyBrowsableForChildren(IncludeDescendants = false)]
        [AttachedPropertyBrowsableForType(typeof(DataGridRow))]
        [AttachedPropertyBrowsableForType(typeof(DataGrid))]
        public static int? GetStartAt(this Control element)
        {
            if (element is null)
            {
                throw new ArgumentNullException(nameof(element));
            }

            return (int?)element.GetValue(StartAtProperty);
        }

#pragma warning restore WPF0013 // CLR accessor for attached property must match registered type.

        private static void OnStartAtChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var dataGrid = (DataGrid)d;
#pragma warning disable IDISP007 // Don't dispose injected.
            (dataGrid.GetValue(RowsListenerProperty) as IDisposable)?.Dispose();
#pragma warning restore IDISP007 // Don't dispose injected.
            dataGrid.ClearValue(RowsListenerProperty);

            if (e.NewValue is null)
            {
                foreach (var item in dataGrid.Items)
                {
                    if (item is DataGridRow row)
                    {
                        BindingOperations.ClearBinding(row, DataGridRow.HeaderProperty);
                    }
                }

                dataGrid.RemoveHandler(Events.RowsChangedEvent, OnRowsChangedHandler);
                return;
            }

#pragma warning disable IDISP004, CA2000 // Don't ignore return value of type IDisposable. Disposed in beginning of method.
            dataGrid.SetCurrentValue(RowsListenerProperty, new RowsListener(dataGrid));
#pragma warning restore IDISP004, CA2000  // Don't ignore return value of type IDisposable.
            dataGrid.UpdateHandler(Events.RowsChangedEvent, OnRowsChangedHandler);
            OnRowsChanged(dataGrid, null);
        }

        private static void OnRowsChanged(object sender, RoutedEventArgs? routedEventArgs)
        {
            var dataGrid = (DataGrid)sender;
            var startAt = dataGrid.GetStartAt() ?? 0;
            for (int index = 0; index < dataGrid.ItemContainerGenerator.Items.Count; index++)
            {
                var row = (DataGridRow)dataGrid.ItemContainerGenerator.ContainerFromIndex(index);
                row?.SetOfRow(index + startAt);
            }
        }
    }
}
