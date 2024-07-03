
using Match3System.Core.Interfaces;
using Match3System.Core.Models;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "Data/LevelData")]
public class LevelData : ScriptableObject
{
    public int rowIndex, columnIndex;

    public List<ItemType> levelItems;

    public int moveCount;


}