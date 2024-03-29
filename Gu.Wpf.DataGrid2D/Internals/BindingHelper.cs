namespace Gu.Wpf.DataGrid2D;

using System.Collections.Generic;
using System.Windows;
using System.Windows.Data;

internal static class BindingHelper
{
    private static readonly Dictionary<int, PropertyPath> IndexPaths = new();

    private static readonly Dictionary<DependencyProperty, PropertyPath> PropertyPaths = new();

    internal static BindingBuilder Bind(
        this DependencyObject target,
        DependencyProperty targetProperty)
    {
        return new BindingBuilder(target, targetProperty);
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
            UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
        };
        return (BindingExpression)BindingOperations.SetBinding(target, targetProperty, binding);
    }

    internal static PropertyPath GetPath(int index)
    {
        if (IndexPaths.TryGetValue(index, out var path))
        {
            return path;
        }

        path = new PropertyPath($"[{index}]");
        IndexPaths[index] = path;
        return path;
    }

    internal static PropertyPath GetPath(DependencyProperty property)
    {
        if (PropertyPaths.TryGetValue(property, out var path))
        {
            return path;
        }

        path = new PropertyPath(property);
        PropertyPaths[property] = path;
        return path;
    }

    internal readonly struct BindingBuilder
    {
        private readonly DependencyObject target;
        private readonly DependencyProperty targetProperty;

        internal BindingBuilder(DependencyObject target, DependencyProperty targetProperty)
        {
            this.target = target;
            this.targetProperty = targetProperty;
        }

        internal BindingExpression? OneWayTo(object source, DependencyProperty sourceProperty)
        {
            var sourcePath = GetPath(sourceProperty);
            return this.OneWayTo(source, sourcePath);
        }

        internal BindingExpression? OneWayTo(object source, int index)
        {
            var sourcePath = GetPath(index);
            return this.OneWayTo(source, sourcePath);
        }

        internal BindingExpression OneWayTo(object? source)
        {
            var binding = new Binding
            {
                Source = source,
                Mode = BindingMode.OneWay,
            };

            return (BindingExpression)BindingOperations.SetBinding(this.target, this.targetProperty, binding);
        }

        internal BindingExpression? OneWayTo(object source, PropertyPath sourcePath)
        {
            var binding = new Binding
            {
                Path = sourcePath,
                Source = source,
                Mode = BindingMode.OneWay,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
            };

            try
            {
                return (BindingExpression)BindingOperations.SetBinding(this.target, this.targetProperty, binding);
            }
            catch
            {
                // This is insanely bad.
                return null;
            }
        }
    }
}
