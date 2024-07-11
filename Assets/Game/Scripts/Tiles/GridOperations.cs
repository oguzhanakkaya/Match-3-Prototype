using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Game.Scripts.Core;
using Match3System.Core.Interfaces;
using Match3System.Core.Models;
using Lean.Pool;

public class GridOperations
{
    private const float SwapDuration = .2f;

    private readonly LevelController _levelController;
    private readonly GameController _gameController;
    private readonly GameBoard _gameBoard;
    public GridOperations(SceneContext sceneContext)
    {
        _levelController = sceneContext.Resolve<LevelController>();
        _gameController = sceneContext.Resolve<GameController>();
        _gameBoard = _levelController._gameBoard;
    }
    public async UniTask ClearSequence()
    {
        var matches = MatchSolver.GetMatches(_gameBoard, GetLineDetectors());

        if (matches.Count > 0)
        {
            await ClearMatchedItem(matches);
            await ClearSequence();
        }
    }
    public async UniTask SwapItemsAsync(GridPoint position1, GridPoint position2)
    {
        await SwapGameBoardItemsAsync(position1, position2);

        var item1Usable = _gameBoard[position1].Item.IsUsableItem;
        var item2Usable = _gameBoard[position2].Item.IsUsableItem;

        if (item1Usable || item2Usable)
        {
            await _gameBoard[position1].Item.Use();
            await _gameBoard[position2].Item.Use();
            await ClearSequence();
        }
        else if (MatchSolver.GetMatches(_gameBoard, GetLineDetectors()).Count > 0)
        {
            await ClearSequence();
        }
        else
            await SwapGameBoardItemsAsync(position1, position2);
    }
    private async UniTask SwapGameBoardItemsAsync(GridPoint position1, GridPoint position2)
    {
        var gridSlot1 = _gameBoard[position1];
        var gridSlot2 = _gameBoard[position2];

        await StartSwapItemsAnim(gridSlot1, gridSlot2);
    }
    private async UniTask StartSwapItemsAnim(IGridNode gridSlot1, IGridNode gridSlot2)
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
    public async UniTask ClearMatchedItem(List<MatchedItems<IGridNode>> matchedItemsList)
    {
        Jobs jobs = new Jobs();
        foreach (var item in matchedItemsList)
        {
            // You can create a special item (Rocket etc.) by checking at the number of items listed here.
            foreach (var item2 in item.itemsList)
            {
                ClearTile(_gameBoard[item2]);
                jobs.Add(_levelController._gridFiller.FillSequence(item2.ColumnIndex));
            }
           
        }
        await jobs.ExecuteJob();
    }
    private void ClearTile(IGridNode grid)
    {
        if (grid.Item == null)
            return;

        _levelController.StartParticle(grid);
        _gameController.DecreaseItemDestroyCount();

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
