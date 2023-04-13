namespace Gu.Wpf.DataGrid2D;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

internal static class EnumerableExt
{
    internal static Type GetElementType(this IEnumerable enumerable)
    {
        var type = enumerable.GetType();
        if (type.HasElementType)
        {
            return type.GetElementType()!;
        }

        if (type.IsEnumerableOfT())
        {
            return type.GetEnumerableItemType();
        }

        return typeof(object);
    }

    internal static bool IsReadOnly(this IEnumerable<IEnumerable> source)
    {
#pragma warning disable CA1851 // Possible multiple enumerations of 'IEnumerable' collection
        if (source.All(x => x is IList))
        {
            return false;
        }

        return source.Any(x => x.GetElementType().IsPrimitive);
#pragma warning restore CA1851 // Possible multiple enumerations of 'IEnumerable' collection
    }

    internal static void SetElementAt(this IEnumerable source, int index, object? value)
    {
        if (source is IList list)
        {
            list[index] = value;
            return;
        }

        var message = $"Only sources implementing {typeof(IList)} are supported for editing.";
        throw new NotSupportedException(message);
    }

    internal static int IndexOf(this IEnumerable source, object item)
    {
        if (source is IList list)
        {
            return list.IndexOf(item);
        }

        var index = 0;
        foreach (var element in source)
        {
            if (Equals(element, item))
            {
                return index;
            }

            index++;
        }

        return -1;
    }

    internal static object? ElementAtOrDefault(this IEnumerable source, int index)
    {
        if (source is null)
        {
            return null;
        }

        if (source is IList list)
        {
            if (index >= list.Count)
            {
                return null;
            }

            return list[index];
        }

        if (source is IReadOnlyList<object> readOnlyList)
        {
            if (index >= readOnlyList.Count)
            {
                return null;
            }

            return readOnlyList[index];
        }

        var count = 0;
        foreach (var item in source)
        {
            if (count == index)
            {
                return item;
            }

            count++;
        }

        return null;
    }

    internal static bool IsEmpty(this IEnumerable source)
    {
        return !source.GetEnumerator().MoveNext();
    }
}
