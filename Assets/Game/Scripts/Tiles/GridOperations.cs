using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Game.Scripts.Core;
using Match3System.Core.Interfaces;
using Match3System.Core.Models;
using Lean.Pool;

public static class GridOperations
{
    private const float SwapDuration = .2f;
    public static async UniTask ClearSequence(GameBoard gameBoard, GameController gameController,LevelController levelController)
    {
        var matches = MatchSolver.GetMatches(gameBoard, GetLineDetectors());

        if (matches.Count > 0)
        {
            ClearMatchedItem(matches, gameBoard,gameController);
            await levelController._gridFiller.FillSequence();
            await ClearSequence(gameBoard, gameController,levelController);
        }
    }
    public static async UniTask SwapItemsAsync(GridPoint position1, GridPoint position2, GameBoard gameBoard, GameController gameController,LevelController levelController)
    {
        await SwapGameBoardItemsAsync(position1, position2, gameBoard);

        var item1Usable = gameBoard[position1].Item.IsUsableItem;
        var item2Usable = gameBoard[position2].Item.IsUsableItem;

        if (item1Usable || item2Usable)
        {
            await gameBoard[position1].Item.Use();
            await gameBoard[position2].Item.Use();
            await ClearSequence(gameBoard, gameController, levelController);
        }
        else if (MatchSolver.GetMatches(gameBoard, GetLineDetectors()).Count > 0)
        {
            await ClearSequence(gameBoard, gameController, levelController);
        }
        else
            await SwapGameBoardItemsAsync(position1, position2, gameBoard);
    }
    private static async UniTask SwapGameBoardItemsAsync(GridPoint position1, GridPoint position2, GameBoard gameBoard)
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
    public static void ClearMatchedItem(List<MatchedItems<IGridNode>> matchedItemsList,GameBoard _gameBoard, GameController gameController)
    {
        foreach (var item in matchedItemsList)
        {
            // You can create a special item (Rocket etc.) by checking at the number of items listed here.
            foreach (var item2 in item.itemsList)
                ClearTile(_gameBoard[item2],gameController);
        }
    }
    private static void ClearTile(IGridNode grid, GameController gameController)
    {
        if (grid.Item == null)
            return;

        gameController.StartParticle(grid);
        gameController.DecreaseItemDestroyCount();

        LeanPool.Despawn((UnityEngine.Component)grid.Item);
        grid.Item.Hide();
        grid.Clear();
    }
    private static List<LineDetectors> GetLineDetectors()
    {
        LineDetectors horizontalDetector = new LineDetectors(new GridPoint[]{GridPoint.Left,GridPoint.Right});
        LineDetectors verticalDetector = new LineDetectors(new GridPoint[]{GridPoint.Up,GridPoint.Down});

        return  new List<LineDetectors>() {horizontalDetector,verticalDetector};
    }
}
