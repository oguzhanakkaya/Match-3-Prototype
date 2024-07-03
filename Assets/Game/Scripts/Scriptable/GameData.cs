
using Match3System.Core.Interfaces;
using Match3System.Core.Models;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class SpriteData
{
    public ItemType itemType;
    public Sprite sprite;
    public Color32 particleColor;

    public SpriteData(ItemType itemType, Sprite sprite, Color32 particleColor)
    {
        this.itemType = itemType;
        this.sprite = sprite;
        this.particleColor = particleColor;
    }
}

[CreateAssetMenu(fileName = "GameData", menuName = "Data/GameData")]
public class GameData : ScriptableObject
{
    public SpriteData[] spriteDatas;


    public List<ItemType> NonTouchableItemTypes;


    public bool CheckItemIsNonTouchable(ItemType type)
    {
        return NonTouchableItemTypes.Contains(type);
    }
    public Sprite GetSprite(ItemType itemType)
    {
        foreach (var item in spriteDatas)
        {
            if (item.itemType == itemType)
                return item.sprite;
        }
        return null;
    }
    public Color32 GetParticleColorFromItemType(ItemType itemType)
    {
        foreach (var item in spriteDatas)
        {
            if (item.itemType == itemType)
                return item.particleColor;
        }
        return Color.white;
    }
}