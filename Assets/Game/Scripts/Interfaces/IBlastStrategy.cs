using System.Threading;
using Cysharp.Threading.Tasks;
using Match3System.Core.Interfaces;

namespace Game.Scripts.Core.Interfaces
{
    public interface IBlastStrategy<in TGridNode> where TGridNode : IGridNode
    {
        UniTask BlastItemsAsync(TGridNode gridNode1, TGridNode gridNode2 = default, CancellationToken cancellationToken = default);
    }
}
