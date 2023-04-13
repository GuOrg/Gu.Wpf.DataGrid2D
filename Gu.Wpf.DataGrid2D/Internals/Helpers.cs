namespace Gu.Wpf.DataGrid2D;

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
        return collection switch
        {
            null => 0,
            ICollection col => col.Count,
            IReadOnlyCollection<object> rol => rol.Count,
            _ => collection.Cast<object>().Count(),
        };
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
