namespace Gu.Wpf.DataGrid2D
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;

    public class Array2DView : IReadOnlyList<Array2DRowView>
    {
        private readonly IReadOnlyList<Array2DRowView> rows;

        public Array2DView(Array source)
        {
            var rows = new List<Array2DRowView>();
            for (int i = 0; i < source.GetLength(0); i++)
            {
                rows.Add(new Array2DRowView(source, i));
            }

            this.rows = rows;
        }

        public int Count => this.rows.Count;

        public Array2DRowView this[int index] => this.rows[index];

        public IEnumerator<Array2DRowView> GetEnumerator() => this.rows.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
    }
}