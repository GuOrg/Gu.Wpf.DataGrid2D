namespace Gu.Wpf.DataGrid2D
{
    using System;
    using System.Collections;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;

    public static partial class ItemsSource
    {
        public static readonly DependencyProperty TransposedSourceProperty = DependencyProperty.RegisterAttached(
            "TransposedSource",
            typeof(IEnumerable),
            typeof(ItemsSource),
            new PropertyMetadata(
                default(IEnumerable),
                OnTransposedSourceChanged,
                CoerceTransposedSource));

        public static readonly DependencyProperty PropertySourceProperty = DependencyProperty.RegisterAttached(
            "PropertySource",
            typeof(object),
            typeof(ItemsSource),
            new PropertyMetadata(
                null,
                OnPropertySourceChanged));

        /// <summary>Helper for setting <see cref="TransposedSourceProperty"/> on <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="DataGrid"/> to set <see cref="TransposedSourceProperty"/> on.</param>
        /// <param name="value">TransposedSource property value.</param>
        public static void SetTransposedSource(this DataGrid element, IEnumerable? value)
        {
            if (element is null)
            {
                throw new ArgumentNullException(nameof(element));
            }

            element.SetValue(TransposedSourceProperty, value);
        }

        /// <summary>Helper for getting <see cref="TransposedSourceProperty"/> from <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="DataGrid"/> to read <see cref="TransposedSourceProperty"/> from.</param>
        /// <returns>TransposedSource property value.</returns>
        [AttachedPropertyBrowsableForChildren(IncludeDescendants = false)]
        [AttachedPropertyBrowsableForType(typeof(DataGrid))]
        public static IEnumerable? GetTransposedSource(this DataGrid element)
        {
            if (element is null)
            {
                throw new ArgumentNullException(nameof(element));
            }

            return (IEnumerable)element.GetValue(TransposedSourceProperty);
        }

        /// <summary>Helper for setting <see cref="PropertySourceProperty"/> on <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="DependencyObject"/> to set <see cref="PropertySourceProperty"/> on.</param>
        /// <param name="value">PropertySource property value.</param>
        public static void SetPropertySource(DependencyObject element, object? value)
        {
            if (element is null)
            {
                throw new ArgumentNullException(nameof(element));
            }

            element.SetValue(PropertySourceProperty, value);
        }

        /// <summary>Helper for getting <see cref="PropertySourceProperty"/> from <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="DependencyObject"/> to read <see cref="PropertySourceProperty"/> from.</param>
        /// <returns>PropertySource property value.</returns>
        [AttachedPropertyBrowsableForChildren(IncludeDescendants = false)]
        [AttachedPropertyBrowsableForType(typeof(DataGrid))]
        public static object? GetPropertySource(DependencyObject element)
        {
            if (element is null)
            {
                throw new ArgumentNullException(nameof(element));
            }

            return element.GetValue(PropertySourceProperty);
        }

        private static void OnTransposedSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var dataGrid = (DataGrid)d;
            var source = (IEnumerable)e.NewValue;
            if (source == null)
            {
                BindingOperations.ClearBinding(dataGrid, ItemsControl.ItemsSourceProperty);
                BindingOperations.ClearBinding(dataGrid, ItemsSourceProxyProperty);
                return;
            }

            _ = dataGrid.Bind(ItemsSourceProxyProperty)
                        .OneWayTo(dataGrid, ItemsControl.ItemsSourceProperty);
            UpdateItemsSource(dataGrid);
        }

        private static object CoerceTransposedSource(DependencyObject dependencyObject, object baseValue)
        {
            if (baseValue == null)
            {
                return null;
            }

            if (baseValue is IEnumerable)
            {
                return baseValue;
            }

            return CreateSingletonEnumerable(baseValue);
        }

        private static void OnPropertySourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue == null)
            {
                d.SetCurrentValue(TransposedSourceProperty, null);
            }
            else
            {
                var source = CreateSingletonEnumerable(e.NewValue);
                d.SetCurrentValue(TransposedSourceProperty, source);
            }
        }

        private static IEnumerable CreateSingletonEnumerable(object item)
        {
            var array = Array.CreateInstance(item.GetType(), 1);
            array.SetValue(item, 0);
            return array;
        }
    }
}
