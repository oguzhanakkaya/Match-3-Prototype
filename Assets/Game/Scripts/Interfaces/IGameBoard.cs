using Match3System.Core.Interfaces;
using Match3System.Core.Models;

namespace Game.Scripts.Core.Interfaces
{
    public interface IGameBoard<out TGridNode> : IGrid where TGridNode : IGridNode
    {
        TGridNode this[GridPoint gridPoint] { get; }
        TGridNode this[int rowIndex, int columnIndex] { get; }

        bool IsPointOnBoard(GridPoint gridPoint);
    }
}
