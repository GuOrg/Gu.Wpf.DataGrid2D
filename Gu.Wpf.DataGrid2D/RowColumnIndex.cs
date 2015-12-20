namespace Gu.Wpf.DataGrid2D
{
    using System;

    public struct RowColumnIndex : IEquatable<RowColumnIndex>
    {
        internal static readonly RowColumnIndex Unset = new RowColumnIndex(-1);

        public RowColumnIndex(int row, int column)
        {
            if (row < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(row), "Row must be greater than or equal to 0");
            }

            if (column < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(column), "Column must be greater than or equal to 0");
            }

            this.Row = row;
            this.Column = column;
        }

        private RowColumnIndex(int rowcol)
        {
            this.Row = rowcol;
            this.Column = rowcol;
        }

        public int Row { get; }

        public int Column { get; }

        public static bool operator ==(RowColumnIndex left, RowColumnIndex right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(RowColumnIndex left, RowColumnIndex right)
        {
            return !left.Equals(right);
        }

        public bool Equals(RowColumnIndex other)
        {
            return this.Row == other.Row && this.Column == other.Column;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            return obj is RowColumnIndex && this.Equals((RowColumnIndex)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (this.Row * 397) ^ this.Column;
            }
        }
    }
}