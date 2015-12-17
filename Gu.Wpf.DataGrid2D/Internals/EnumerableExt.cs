namespace Gu.Wpf.DataGrid2D
{
    using System.Collections;
    using System.Collections.Generic;

    internal static class EnumerableExt
    {
        public static T ElementAtOrDefault<T>(this IEnumerable source, int index)
        {
            return (T)(source.ElementAtOrDefault(index) ?? default(T));
        }

        public static object ElementAtOrDefault(this IEnumerable source, int index)
        {
            if (source == null)
            {
                return null;
            }

            var list = source as IList;
            if (list != null)
            {
                if (index >= list.Count)
                {
                    return null;
                }

                return list[index];
            }

            var readOnlyList = source as IReadOnlyList<object>;
            if (readOnlyList != null)
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
    }
}
