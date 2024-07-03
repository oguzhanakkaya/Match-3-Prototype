using System;
using System.Runtime.CompilerServices;

namespace Match3System.Core.Models
{
    public readonly struct GridPoint : IEquatable<GridPoint>
    {
        public int RowIndex { get; }
        public int ColumnIndex { get; }
        
        public GridPoint(int rowIndex, int columnIndex)
        {
            RowIndex = rowIndex;
            ColumnIndex = columnIndex;
        }
        
        public static GridPoint Zero { get; } = new GridPoint(0, 0);
        public static GridPoint Up { get; } = new GridPoint(-1, 0);
        public static GridPoint Down { get; } = new GridPoint(1, 0);
        public static GridPoint Left { get; } = new GridPoint(0, -1);
        public static GridPoint Right { get; } = new GridPoint(0, 1);
        public static GridPoint UpLeft { get; } = new GridPoint(-1, -1);
        public static GridPoint UpRight { get; } = new GridPoint(-1, 1);
        public static GridPoint DownLeft { get; } = new GridPoint(1, -1);
        public static GridPoint DownRight { get; } = new GridPoint(1, 1);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(GridPoint other)
        {
            return RowIndex == other.RowIndex && ColumnIndex == other.ColumnIndex;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(GridPoint a, GridPoint b)
        {
            return a.RowIndex == b.RowIndex && a.ColumnIndex == b.ColumnIndex;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(GridPoint a, GridPoint b)
        {
            return a.RowIndex != b.RowIndex || a.ColumnIndex != b.ColumnIndex;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static GridPoint operator +(GridPoint a, GridPoint b)
        {
            return new GridPoint(a.RowIndex + b.RowIndex, a.ColumnIndex + b.ColumnIndex);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static GridPoint operator -(GridPoint a, GridPoint b)
        {
            return new GridPoint(a.RowIndex - b.RowIndex, a.ColumnIndex - b.ColumnIndex);
        }
        
        public override bool Equals(object obj)
        {
            return obj is GridPoint other && Equals(other);
        }

        public override int GetHashCode()
        {
            return RowIndex.GetHashCode() ^ (ColumnIndex.GetHashCode() << 2);
        }
    }
}
