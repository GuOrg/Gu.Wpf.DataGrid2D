namespace Gu.Wpf.DataGrid2D
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Media;

    internal static class Helpers
    {
        private static readonly Dictionary<int, PropertyPath> IndexPaths = new Dictionary<int, PropertyPath>();

        private static readonly Dictionary<DependencyProperty, PropertyPath> PropertyPaths =
            new Dictionary<DependencyProperty, PropertyPath>();

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

        internal static BindingExpression Bind(
            DependencyObject target,
            DependencyProperty targetProperty,
            object source,
            DependencyProperty sourceProperty)
        {
            return Bind(target, targetProperty, source, GetPath(sourceProperty));
        }

        internal static BindingExpression Bind(
            DependencyObject target,
            DependencyProperty targetProperty,
            object source,
            int sourceIndex)
        {
            return Bind(target, targetProperty, source, GetPath(sourceIndex));
        }

        internal static BindingExpression Bind(
            DependencyObject target,
            DependencyProperty targetProperty,
            object source,
            PropertyPath path)
        {
            var binding = new Binding
                              {
                                  Path = path,
                                  Source = source,
                                  Mode = BindingMode.OneWay,
                                  UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                              };
            return (BindingExpression)BindingOperations.SetBinding(target, targetProperty, binding);
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
            if (collection == null)
            {
                return 0;
            }

            var col = collection as ICollection;
            if (col != null)
            {
                return col.Count;
            }

            var rol = collection as IReadOnlyCollection<object>;
            if (rol != null)
            {
                return rol.Count;
            }

            return collection.Cast<object>()
                             .Count();
        }

        internal static object First(this IEnumerable collection)
        {
            var enumerator = collection.GetEnumerator();
            object first = enumerator.MoveNext() ? enumerator.Current : null;
            var disposable = enumerator as IDisposable;
            if (disposable != null)
            {
                disposable.Dispose();
            }
            return first;
        }

        internal static IEnumerable<DependencyObject> Ancestors(this DependencyObject o)
        {
            var parent = VisualTreeHelper.GetParent(o);
            while (parent != null)
            {
                yield return parent;
                parent = VisualTreeHelper.GetParent(parent);
            }
        }
    }
}
