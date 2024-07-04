using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Game.Scripts.Core;
using Game.Scripts.Core.Interfaces;
using Match3System.Core.Models;

public static class ItemFallDown
{
    public static async UniTask FallDown(GameBoard _gameBoard,GameController gameController,float delay=0)
    {
        await UniTask.Delay(TimeSpan.FromSeconds(delay));

        List<UniTask> tasks = new List<UniTask>();

        for (var rowIndex = _gameBoard.RowCount - 1; rowIndex >= 0; rowIndex--)
        {
             for (var columnIndex = _gameBoard.ColumnCount - 1; columnIndex >= 0; columnIndex--)
             {
                if (CanMoveDown(new GridPoint(rowIndex, columnIndex), out GridPoint gridPoint,_gameBoard))
                {
                    IItem item = _gameBoard[new GridPoint(rowIndex, columnIndex)].Item;

                    if (item == null)
                        continue;

                    tasks.Add(ItemMovement.MoveItem(item, gameController.GetWorldPosition(gridPoint.RowIndex, gridPoint.ColumnIndex)));

                    _gameBoard[new GridPoint(rowIndex, columnIndex)].Clear();
                    _gameBoard[new GridPoint(gridPoint.RowIndex, gridPoint.ColumnIndex)].SetItem(item);
                 }
             }
        }
        await UniTask.WhenAll(tasks);
        await UniTask.DelayFrame(1);
    }
    public static bool CanMoveDown(GridPoint point, out GridPoint gridPosition,GameBoard _gameBoard)
    {
        bool canMoveDown=false;
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
    private static bool CanDropDown(GameBoard gameBoard,GridPoint point)
    {
        GridPoint gridPosition = point + GridPoint.Down;

        return gameBoard.IsPositionOnGrid(gridPosition) &&
            !gameBoard[new GridPoint(gridPosition.RowIndex, gridPosition.ColumnIndex)].HasItem;
    }
}
