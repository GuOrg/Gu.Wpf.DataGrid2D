namespace Gu.Wpf.DataGrid2D
{
    using System;
    using System.ComponentModel;
    using System.Text.RegularExpressions;

    /// <summary>
    /// For specifying a cell index.
    /// </summary>
    [TypeConverter(typeof(RowColumnIndexConverter))]
    public struct RowColumnIndex : IEquatable<RowColumnIndex>
    {
        /// <summary>
        /// No selection.
        /// </summary>
        public static readonly RowColumnIndex None = new(-1);

        /// <summary>
        /// Initializes a new instance of the <see cref="RowColumnIndex"/> struct.
        /// </summary>
        /// <param name="row">The row index.</param>
        /// <param name="column">The column index.</param>
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

        /// <summary>
        /// Gets the row index.
        /// </summary>
        public int Row { get; }

        /// <summary>
        /// Gets the column index.
        /// </summary>
        public int Column { get; }

        /// <summary>Check if <paramref name="left"/> is equal to <paramref name="right"/>.</summary>
        /// <param name="left">The left <see cref="RowColumnIndex"/>.</param>
        /// <param name="right">The right <see cref="RowColumnIndex"/>.</param>
        /// <returns>True if <paramref name="left"/> is equal to <paramref name="right"/>.</returns>
        public static bool operator ==(RowColumnIndex left, RowColumnIndex right)
        {
            return left.Equals(right);
        }

        /// <summary>Check if <paramref name="left"/> is not equal to <paramref name="right"/>.</summary>
        /// <param name="left">The left <see cref="RowColumnIndex"/>.</param>
        /// <param name="right">The right <see cref="RowColumnIndex"/>.</param>
        /// <returns>True if <paramref name="left"/> is not equal to <paramref name="right"/>.</returns>
        public static bool operator !=(RowColumnIndex left, RowColumnIndex right)
        {
            return !left.Equals(right);
        }

        /// <summary>
        /// Parse a <see cref="RowColumnIndex"/> from <paramref name="text"/>.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns>A <see cref="RowColumnIndex"/>.</returns>
        public static RowColumnIndex Parse(string text)
        {
            if (TryParse(text, out var result))
            {
                return result;
            }

            throw new FormatException($"Could not parse '{text}' to a {typeof(RowColumnIndex)}");
        }

        /// <summary>
        /// Try parse a <see cref="RowColumnIndex"/> from <paramref name="text"/>.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="result">A <see cref="RowColumnIndex"/>.</param>
        /// <returns>True if success.</returns>
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

        /// <inheritdoc />
        public bool Equals(RowColumnIndex other)
        {
            return this.Row == other.Row && this.Column == other.Column;
        }

        /// <inheritdoc />
        public override bool Equals(object? obj)
        {
            return obj is RowColumnIndex index && this.Equals(index);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            unchecked
            {
                return (this.Row * 397) ^ this.Column;
            }
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"R{this.Row} C{this.Column}";
        }
    }
}
