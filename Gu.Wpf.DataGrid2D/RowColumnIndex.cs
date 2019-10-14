namespace Gu.Wpf.DataGrid2D
{
    using System;
    using System.ComponentModel;
    using System.Text.RegularExpressions;

    [TypeConverter(typeof(RowColumnIndexConverter))]
    public struct RowColumnIndex : IEquatable<RowColumnIndex>
    {
        public static readonly RowColumnIndex None = new RowColumnIndex(-1);

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

        public static RowColumnIndex Parse(string text)
        {
            if (TryParse(text, out var result))
            {
                return result;
            }

            throw new FormatException($"Could not parse '{text}' to a {typeof(RowColumnIndex)}");
        }

        public static bool TryParse(string text, out RowColumnIndex result)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                result = RowColumnIndex.None;
                return false;
            }

            var match = Regex.Match(text, @"^ *R(?<row>\d+) *C(?<col>\d+) *$", RegexOptions.IgnoreCase);
            if (match.Success)
            {
                if (int.TryParse(match.Groups["row"].Value, out var row) &&
                    int.TryParse(match.Groups["col"].Value, out var col) &&
                    row >= 0 &&
                    col >= 0)
                {
                    result = new RowColumnIndex(row, col);
                    return true;
                }
            }

            result = RowColumnIndex.None;
            return false;
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

            return obj is RowColumnIndex index && this.Equals(index);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (this.Row * 397) ^ this.Column;
            }
        }

        public override string ToString()
        {
            return $"R{this.Row} C{this.Column}";
        }
    }
}
