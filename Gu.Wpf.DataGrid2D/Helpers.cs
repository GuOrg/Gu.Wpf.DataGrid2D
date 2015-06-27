namespace Gu.Wpf.DataGrid2D
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;

    internal static class Helpers
    {
        private static readonly Dictionary<int, PropertyPath> IndexPaths = new Dictionary<int, PropertyPath>();
        private static readonly Dictionary<DependencyProperty, PropertyPath> PropertyPaths = new Dictionary<DependencyProperty, PropertyPath>();

        internal static DataTemplate CreateDefaultTemplate()
        {
            var dataTemplate = new DataTemplate();
            var factory = new FrameworkElementFactory(typeof(ContentPresenter));
            dataTemplate.VisualTree = factory;
            return dataTemplate;
        }

        internal static void UpdateHandler(this UIElement element, RoutedEvent routedEvent, Delegate handler)
        {
            element.RemoveHandler(routedEvent, handler);
            element.AddHandler(routedEvent, handler);
        }

        internal static void Bind(DependencyObject target, DependencyProperty targetProperty, object source, PropertyPath path)
        {
            var binding = new Binding
            {
                Path = path,
                Source = source,
                Mode = BindingMode.OneWay,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            };
            BindingOperations.SetBinding(target, targetProperty, binding);
        }

        internal static PropertyPath GetPath(int index)
        {
            PropertyPath path;
            if (!IndexPaths.TryGetValue(index, out path))
            {
                path = new PropertyPath(string.Format("[{0}]", index));
                IndexPaths[index] = path;
            }
            return path;
        }

        internal static PropertyPath GetPath(DependencyProperty property)
        {
            PropertyPath path;
            if (!PropertyPaths.TryGetValue(property, out path))
            {
                //path = new PropertyPath(string.Format("({0}.{1})", typeof(Source2D).Name, property.Name));
                path = new PropertyPath(property);
                PropertyPaths[property] = path;
            }
            return path;
        }

        internal static int Count(this IEnumerable collection)
        {
            var array = collection as IList;
            if (array != null)
            {
                return array.Count;
            }
            int count = 0;
            foreach (var item in collection)
            {
                count++;
            }
            return count;
        }
    }
}
