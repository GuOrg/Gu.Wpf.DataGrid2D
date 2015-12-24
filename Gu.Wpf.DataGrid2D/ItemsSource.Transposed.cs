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
                OnCoerceTransposedSource));

        public static readonly DependencyProperty PropertySourceProperty = DependencyProperty.RegisterAttached(
            "PropertySource",
            typeof(object),
            typeof(ItemsSource),
            new PropertyMetadata(
                default(object),
                OnPropertySourceChanged));

        public static void SetTransposedSource(this DataGrid element, IEnumerable value)
        {
            element.SetValue(TransposedSourceProperty, value);
        }

        [AttachedPropertyBrowsableForChildren(IncludeDescendants = false)]
        [AttachedPropertyBrowsableForType(typeof(DataGrid))]
        public static IEnumerable GetTransposedSource(this DataGrid element)
        {
            return (IEnumerable)element.GetValue(TransposedSourceProperty);
        }

        public static void SetPropertySource(DependencyObject element, object value)
        {
            element.SetValue(PropertySourceProperty, value);
        }

        [AttachedPropertyBrowsableForChildren(IncludeDescendants = false)]
        [AttachedPropertyBrowsableForType(typeof(DataGrid))]
        public static object GetPropertySource(DependencyObject element)
        {
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

            dataGrid.Bind(ItemsSourceProxyProperty)
                    .OneWayTo(dataGrid, ItemsControl.ItemsSourceProperty);
            UpdateItemsSource(dataGrid);
        }

        private static object OnCoerceTransposedSource(DependencyObject dependencyObject, object baseValue)
        {
            if (baseValue == null)
            {
                return null;
            }

            var enumerable = baseValue as IEnumerable;
            if (enumerable != null)
            {
                return baseValue;
            }

            return CreateSingletonEnumerable(baseValue);
        }

        private static void OnPropertySourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue == null)
            {
                d.SetValue(TransposedSourceProperty, null);
            }
            else
            {
                var source = CreateSingletonEnumerable(e.NewValue);
                d.SetValue(TransposedSourceProperty, source);
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
