using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Game.Scripts.Core;
using Lean.Pool;
using Match3System.Core.Interfaces;
using Match3System.Core.Models;
using PoolSystem.Core;
using UnityEngine;
public class GenerateItem
{
    private readonly PoolManager _poolManager;
    private readonly GameController _gameController;
    private readonly LevelData _levelData;
    public GenerateItem(SceneContext sceneContext,LevelData levelData)
    {
        _gameController = sceneContext.GetGameController();
        _poolManager = sceneContext.Resolve<PoolManager>();
        _levelData = levelData;
    }
    public void GenerateToAllBoard(GameBoard gameBoard)
    {
        for (int rowIndex = 0; rowIndex < gameBoard.RowCount; rowIndex++)
        {
            for (int columnIndex = 0; columnIndex < gameBoard.ColumnCount; columnIndex++)
            {
                IGridNode gridNode = gameBoard[new GridPoint(rowIndex,columnIndex)];

                if (gridNode.HasItem != false)
                    continue;

                var item = GenerateRandomItem();
                item.SetPosition(GetWorldPosition(rowIndex,columnIndex));
                item.Show();

                gridNode.SetItem(item);
            }
        }
    }
    public async UniTask Fill(GameBoard gameBoard,int delay, LevelData levelData)
    {
        await UniTask.Delay(TimeSpan.FromSeconds(.1f));

        List<UniTask> tasks = new List<UniTask>();

        for (var columnIndex = gameBoard.ColumnCount-1; columnIndex >= 0; columnIndex--)
        {
            for (var rowIndex = gameBoard.RowCount-1; rowIndex >= 0; rowIndex--)
            {
                IGridNode gridNode = gameBoard[new GridPoint(rowIndex, columnIndex)];

                if (gridNode.HasItem || !levelData.spawners[columnIndex])
                    continue;

                var item = GenerateRandomItem();
                GridPoint itemGeneratorPosition = GetItemGeneratorPosition(gameBoard, rowIndex, columnIndex);
                item.SetPosition(GetWorldPosition(itemGeneratorPosition.RowIndex - 2, itemGeneratorPosition.ColumnIndex));
                item.Show();

                gridNode.SetItem(item);

                tasks.Add(ItemMovement.MoveItem(item, GetWorldPosition(itemGeneratorPosition.RowIndex, itemGeneratorPosition.ColumnIndex)));
            }
        }
        await UniTask.WhenAll(tasks);
    }
    protected Vector3 GetWorldPosition(int row,int column)
    {
         return _gameController.GetWorldPosition(row,column);
    }
    private Item GenerateRandomItem()
    {
        int i = UnityEngine.Random.Range(0, _levelData.levelItems.Count);
        return LeanPool.Spawn(_levelData.levelItems[i]);
    }
    private Item GenerateItemById(string id)
    {
        return LeanPool.Spawn(_poolManager.GetComponentFromID(id)).GetComponent<Item>();
    }
    private GridPoint GetItemGeneratorPosition(GameBoard gameBoard, int rowIndex, int columnIndex)
    {
        while (rowIndex >= 0)
        {
            var gridSlot = gameBoard[new GridPoint(rowIndex, columnIndex)];
            if (gridSlot.HasItem ==false)
                return new GridPoint(rowIndex, columnIndex);

            rowIndex--;
        }
        return new GridPoint(-1, columnIndex);
    }
}

