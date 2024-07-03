using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Match3System.Core.Interfaces;
using Match3System.Core.Models;
using UnityEngine.UIElements;

public static class SwapItem
{
    private const float SwapDuration = .2f;


    public static async UniTask SwapItemsAsync(GridPoint position1, GridPoint position2,GameBoard gameBoard)
    {
        await SwapGameBoardItemsAsync(position1, position2,gameBoard);

        if (MatchSolver.GetMatches(gameBoard).Count > 0)
            GridOperations.ClearMatchedItem(MatchSolver.GetMatches(gameBoard),gameBoard);
        else
            await SwapGameBoardItemsAsync(position1, position2, gameBoard);



       /* if (IsSolved(position1, position2, out var solvedData))
        {
           // NotifySequencesSolved(solvedData);
           // await ExecuteJobsAsync(fillStrategy.GetSolveJobs(GameBoard, solvedData), cancellationToken);
        }
        else
        {
           await SwapGameBoardItemsAsync(position1, position2,gameBoard);
        }
       */
    }
  /*  protected bool IsSolved(GridPosition position1, GridPosition position2, out SolvedData<TGridSlot> solvedData)
    {
        solvedData = _gameBoardSolver.Solve(GameBoard, position1, position2);
        return solvedData.SolvedSequences.Count > 0;
    }
    */

    private static async UniTask SwapGameBoardItemsAsync(GridPoint position1, GridPoint position2,GameBoard gameBoard)
    {
        var gridSlot1 = gameBoard[position1];
        var gridSlot2 = gameBoard[position2];

        await StartSwapItemsAnim(gridSlot1, gridSlot2);
    }

    private static async UniTask StartSwapItemsAnim(IGridNode gridSlot1, IGridNode gridSlot2)
    {
        var item1 = gridSlot1.Item;
        var item2 = gridSlot2.Item;

        var item1WorldPosition = item1.GetPosition();
        var item2WorldPosition = item2.GetPosition();

        var sequence = DOTween.Sequence();

        await sequence
           .Join(item1.Transform.DOMove(item2WorldPosition, SwapDuration))
           .Join(item2.Transform.DOMove(item1WorldPosition, SwapDuration))
           .SetEase(Ease.Flash);

        item1.SetPosition(item2WorldPosition);
        item2.SetPosition(item1WorldPosition);

        gridSlot1.SetItem(item2);
        gridSlot2.SetItem(item1);


    }
}
