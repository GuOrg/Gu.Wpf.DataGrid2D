namespace Gu.Wpf.DataGrid2D
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public class Array2DView : IReadOnlyList<RowView>
    {
        private readonly IReadOnlyList<RowView> rows;

        public Array2DView(Array source)
        {
            var rows = new List<RowView>();
            for (int i = 0; i < source.GetLength(0); i++)
            {
                rows.Add(new RowView(source, i));
            }

            this.rows = rows;
        }

        public int Count => this.rows.Count;

        public RowView this[int index] => this.rows[index];

        public IEnumerator<RowView> GetEnumerator() => this.rows.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
    }
}