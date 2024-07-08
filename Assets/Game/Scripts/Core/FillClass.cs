using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Game.Scripts.Core;
using Game.Scripts.Core.Interfaces;
using Match3System.Core.Interfaces;
using Match3System.Core.Models;
using PoolSystem;
using UnityEngine;
public class FillClass
{
    private readonly IPool<Item> _itemsPool;
    private readonly GameController _gameController;
    private readonly IItemGenerator _itemGenerator;
    public FillClass(SceneContext sceneContext)
    {
        _gameController = sceneContext.GetGameController();
        _itemsPool = sceneContext.Resolve<IPoolManager>().GetPool<Item>("item_basic");
        _itemGenerator = sceneContext.Resolve<NodeItemGenerator>();
    }
    public void FillInstantly(GameBoard gameBoard,LevelData levelData, ItemType[,] itemArray = null)
    {
        var itemsToShow = new List<Item>();

        for (int rowIndex = 0; rowIndex < gameBoard.RowCount; rowIndex++)
        {
            for (int columnIndex = 0; columnIndex < gameBoard.ColumnCount; columnIndex++)
            {
                IGridNode gridNode = gameBoard[new GridPoint(rowIndex,columnIndex)];

                if (gridNode.HasItem != false)
                    continue;

                Item item = null;

                if (itemArray != null)
                    item = GetItemFromPool(itemArray[rowIndex,columnIndex]);
                else
                    item = GetItemFromPool(levelData);

                item.SetPosition(GetWorldPosition(rowIndex,columnIndex));

                gridNode.SetItem(item);
                itemsToShow.Add(item);
            }
        }
        ShowItem(itemsToShow);
    }
    public async UniTask Fill(GameBoard gameBoard,int delay, LevelData levelData)
    {
        await UniTask.Delay(TimeSpan.FromSeconds(.1f));

        List<UniTask> tasks = new List<UniTask>();

        for (var columnIndex = gameBoard.ColumnCount-1; columnIndex >= 0; columnIndex--)
        {
            for (var rowIndex = gameBoard.RowCount-1; rowIndex >= 0; rowIndex--)
            {
                IGridNode gridSlot = gameBoard[new GridPoint(rowIndex, columnIndex)];

                if (gridSlot.HasItem || !levelData.spawners[columnIndex])
                    continue;

                Item item = GetItemFromPool(levelData);
                GridPoint itemGeneratorPosition = GetItemGeneratorPosition(gameBoard, rowIndex, columnIndex);
                item.SetPosition(GetWorldPosition(itemGeneratorPosition.RowIndex-2,itemGeneratorPosition.ColumnIndex));
                ShowItem(item);

                tasks.Add(ItemMovement.MoveItem(item,GetWorldPosition(itemGeneratorPosition.RowIndex,itemGeneratorPosition.ColumnIndex)));

                gridSlot.SetItem(item);
            }
        }
        await UniTask.WhenAll(tasks);
    }
    protected Vector3 GetWorldPosition(int row,int column)
    {
         return _gameController.GetWorldPosition(row,column);
    }
    protected Item GetItemFromPool(LevelData levelData)
    {
        return (Item)_itemGenerator.GetItem(levelData);
    }
    protected Item GetItemFromPool(ItemType type)
    {
        return (Item)_itemGenerator.GetSpecificItem(type);
    }
    protected void ReturnItemToPool(Item item)
    {
        _itemsPool.Recycle(item);
    }
    private void ShowItem(List<Item> items)
    {
        foreach (var item in items)
        {
            item.Show();
        }
    }
    private void ShowItem(IItem items)
    {
        items.Show();
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

