using Match3System.Core.Models;

namespace Match3System.Core.Interfaces
{
    public interface IGrid
    {
        int RowCount { get; }
        int ColumnCount { get; }
        bool IsPointOnGrid(GridPoint gridPoint);
    }
}
