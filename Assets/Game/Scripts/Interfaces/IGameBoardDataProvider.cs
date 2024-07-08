using Match3System.Core.Interfaces;

namespace Game.Scripts.Core.Interfaces
{
    public interface IGameBoardDataProvider<out TGridNode> where TGridNode : IGridNode
    {
        TGridNode[,] GetGameBoardNodes();
    }
}
