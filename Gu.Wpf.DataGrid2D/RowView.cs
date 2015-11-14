namespace Gu.Wpf.DataGrid2D
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public class RowView : IReadOnlyList<object>
    {
        private readonly Array _source;
        private readonly int _rowIndex;

        public RowView(Array source, int rowIndex)
        {
            _source = source;
            _rowIndex = rowIndex;
        }

        public IEnumerator<object> GetEnumerator()
        {
            for (int j = 0; j < _source.GetLength(1); j++)
            {
               yield return _source.GetValue(_rowIndex, j);
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public int Count => _source.GetLength(0);

        public object this[int index] => _source.GetValue(_rowIndex, index);
    }
}
