using System;
using Match3System.Core.Interfaces;
using Match3System.Core.Models;
using UnityEngine;
public class GameBoard
{
    public int      RowCount { get; set; }
    public int      ColumnCount { get; set; }
    public float    tileSize { get; set; }
    public Vector3  _originPosition { get; set; }


    private IGridNode[,] _gridSlots;

    public void SetGridSlots(IGridNode[,] gridSlots,Vector3 originPos,float tileSize)
    {
        RowCount = gridSlots.GetLength(0);
        ColumnCount = gridSlots.GetLength(1);
        _gridSlots = gridSlots;

        _originPosition = originPos;
        this.tileSize = tileSize;
    }
    public IGridNode this[GridPoint gridPoint] => _gridSlots[gridPoint.RowIndex,gridPoint.ColumnIndex];
    public bool IsPointerOnGrid(Vector3 pointerPos,out GridPoint point)
    {
        point = GetGridPositionByPointer(pointerPos);

        return IsPositionOnGrid(point);
    }
    private GridPoint GetGridPositionByPointer(Vector3 worldPointerPosition)
    {
        var rowIndex = (worldPointerPosition - _originPosition).y / tileSize;
        var columnIndex = (worldPointerPosition - _originPosition).x / tileSize;

        return new GridPoint(Convert.ToInt32(-rowIndex), Convert.ToInt32(columnIndex));
    }
    public bool IsPositionOnGrid(GridPoint gridPosition)
    {
        return gridPosition.RowIndex >= 0 &&
               gridPosition.RowIndex < RowCount &&
               gridPosition.ColumnIndex >= 0 &&
               gridPosition.ColumnIndex < ColumnCount;
    }
    public void Dispose(){}
}
