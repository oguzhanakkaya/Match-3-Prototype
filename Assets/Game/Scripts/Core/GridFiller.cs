using Cysharp.Threading.Tasks;
using Game.Scripts.Core;
using Game.Scripts.Core.Interfaces;
using Lean.Pool;
using Match3System.Core.Interfaces;
using Match3System.Core.Models;
using PoolSystem.Core;
using UnityEngine;
public class GridFiller
{
    private readonly PoolManager        _poolManager;
    private readonly LevelController    _levelController;
    private readonly LevelData          _levelData;
    private readonly GameBoard          _gameBoard;
    private readonly IGridNode[,]       _gridNode;

    private int RowCount,ColumnCount;
    public GridFiller(SceneContext sceneContext,LevelData levelData)
    {
        _poolManager = sceneContext.Resolve<PoolManager>();
        _levelController = sceneContext.Resolve<LevelController>();
        _levelData = levelData;
        _gridNode = _levelController.GetGameBoardNodes();
        _gameBoard = _levelController._gameBoard;

        RowCount = _gridNode.GetLength(0);
        ColumnCount = _gridNode.GetLength(1);
    }
    public async UniTask FillSequence(int columnIndex)
    {
        await FallDown(columnIndex);
     //   await Fill();
    }
    public void GenerateToAllBoard()
    {
        for (int rowIndex = 0; rowIndex < RowCount; rowIndex++)
        {
            for (int columnIndex = 0; columnIndex < ColumnCount; columnIndex++)
            {
                IGridNode gridNode = _gridNode[rowIndex,columnIndex];

                if (gridNode.HasItem != false)
                    continue;

                var item = GenerateRandomItem();
                item.SetPosition(GetWorldPosition(rowIndex,columnIndex));
                item.Show();

                gridNode.SetItem(item);
            }
        }
    }
    public async UniTask FallDown(int columnIndex)
    {
        Jobs fallDownJobs = new Jobs();

        for (var rowIndex = RowCount - 1; rowIndex >= 0; rowIndex--)
        {
                if (CanMoveDown(new GridPoint(rowIndex, columnIndex), out GridPoint gridPoint, _gameBoard))
                {
                    IItem item = _gridNode[rowIndex,columnIndex].Item;

                    if (item == null)
                        continue;

                    fallDownJobs.Add(ItemMovement.MoveItem(item,GetWorldPosition(gridPoint.RowIndex, gridPoint.ColumnIndex)));

                    _gridNode[rowIndex, columnIndex].Clear();
                    _gridNode[gridPoint.RowIndex, gridPoint.ColumnIndex].SetItem(item);
                }
        }
        await fallDownJobs.ExecuteJob();
        await Fill();
    }
    public async UniTask Fill()
    {
        Jobs jobs=new Jobs();

        for (var columnIndex = _gridNode.GetLength(1) - 1; columnIndex >= 0; columnIndex--)
        {
            for (var rowIndex = _gridNode.GetLength(0) - 1; rowIndex >= 0; rowIndex--)
            {
                IGridNode gridNode = _gridNode[rowIndex, columnIndex];

                if (gridNode.HasItem || !_levelData.spawners[columnIndex])
                    continue;

                var item = GenerateRandomItem();
                GridPoint itemGeneratorPosition = GetItemGeneratorPosition(rowIndex, columnIndex);
                item.SetPosition(GetWorldPosition(itemGeneratorPosition.RowIndex - 2, itemGeneratorPosition.ColumnIndex));
                item.Show();

                gridNode.SetItem(item);

                jobs.Add(ItemMovement.MoveItem(item, GetWorldPosition(itemGeneratorPosition.RowIndex, itemGeneratorPosition.ColumnIndex)));
            }
        }
        await jobs.ExecuteJob();
    }
    protected Vector3 GetWorldPosition(int row,int column)
    {
         return _levelController.GetWorldPosition(row,column);
    }
    private ItemBase GenerateRandomItem()
    {
        int i = UnityEngine.Random.Range(0, _levelData.levelItems.Count);
        return LeanPool.Spawn(_levelData.levelItems[i]);
    }
    private ItemBase GenerateItemById(string id)
    {
        return LeanPool.Spawn(_poolManager.GetComponentFromID(id)).GetComponent<ItemBase>();
    }
    private GridPoint GetItemGeneratorPosition( int rowIndex, int columnIndex)
    {
        while (rowIndex >= 0)
        {
            var gridSlot = _gridNode[rowIndex, columnIndex];
            if (gridSlot.HasItem ==false)
                return new GridPoint(rowIndex, columnIndex);

            rowIndex--;
        }
        return new GridPoint(-1, columnIndex);
    }
    public static bool CanMoveDown(GridPoint point, out GridPoint gridPosition, GameBoard _gameBoard)
    {
        bool canMoveDown = false;
        gridPosition = point;

        for (int i = 0; i < _gameBoard.RowCount; i++)
        {
            if (CanDropDown(_gameBoard, gridPosition))
            {
                canMoveDown = true;

                gridPosition += GridPoint.Down;

            }
        }
        return canMoveDown;
    }
    private static bool CanDropDown(GameBoard gameBoard, GridPoint point)
    {
        GridPoint gridPosition = point + GridPoint.Down;

        return gameBoard.IsPositionOnGrid(gridPosition) &&
            !gameBoard[new GridPoint(gridPosition.RowIndex, gridPosition.ColumnIndex)].HasItem;
    }
}

