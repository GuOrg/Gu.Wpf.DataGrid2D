namespace Gu.Wpf.DataGrid2D
{
    using System;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    public class Sort
    {
        public static readonly DependencyProperty ThreeStateProperty = DependencyProperty.RegisterAttached(
            "ThreeState",
            typeof(bool),
            typeof(Sort),
            new PropertyMetadata(default(bool), OnThreeStateChanged));

        private static readonly DependencyProperty SubscriptionProperty = DependencyProperty.RegisterAttached(
            "Subscription",
            typeof(SortingSubscription),
            typeof(Sort),
            new PropertyMetadata(default(SortingSubscription)));

        public static void SetThreeState(DependencyObject element, bool value)
        {
            element.SetValue(ThreeStateProperty, value);
        }

        public static bool GetThreeState(DependencyObject element)
        {
            return (bool)element.GetValue(ThreeStateProperty);
        }

        private static void OnThreeStateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue)
            {
                d.SetCurrentValue(SubscriptionProperty, new SortingSubscription((DataGrid)d));
            }
            else
            {
                (d.GetValue(SubscriptionProperty) as IDisposable)?.Dispose();
                d.ClearValue(SubscriptionProperty);
            }
        }

        private static void OnDataGridSorting(object sender, DataGridSortingEventArgs e)
        {
            var dataGrid = (DataGrid)sender;
            var sortPropertyName = e.Column.SortMemberPath;
            if (!string.IsNullOrEmpty(sortPropertyName))
            {
                // sorting is cleared when the previous state is Descending
                if (e.Column.SortDirection.HasValue && e.Column.SortDirection.Value == ListSortDirection.Descending)
                {
                    e.Column.SetCurrentValue(DataGridColumn.SortDirectionProperty, null);
                    if ((Keyboard.Modifiers & ModifierKeys.Shift) != ModifierKeys.Shift)
                    {
                        var sortDescriptions = dataGrid.Items.SortDescriptions;
                        for (var i = sortDescriptions.Count - 1; i >= 0; i--)
                        {
                            if (sortDescriptions[i].PropertyName == sortPropertyName)
                            {
                                sortDescriptions.RemoveAt(i);
                            }
                        }
                    }
                    else
                    {
                        dataGrid.Items.SortDescriptions.Clear();
                    }

                    dataGrid.Items.Refresh();
                    e.Handled = true;
                }
            }
        }

        private sealed class SortingSubscription : IDisposable
        {
            private readonly DataGrid dataGrid;

            public SortingSubscription(DataGrid dataGrid)
            {
                this.dataGrid = dataGrid;
                this.dataGrid.Sorting += OnDataGridSorting;
            }

            public void Dispose()
            {
                this.dataGrid.Sorting -= OnDataGridSorting;
            }
        }
    }
}
