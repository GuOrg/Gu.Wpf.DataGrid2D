namespace Gu.Wpf.DataGrid2D
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    internal static class Helpers
    {
        internal static DataTemplate CreateDefaultTemplate()
        {
            var dataTemplate = new DataTemplate();
            var factory = new FrameworkElementFactory(typeof(ContentPresenter));
            dataTemplate.VisualTree = factory;
            return dataTemplate;
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
            disposable?.Dispose();

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
