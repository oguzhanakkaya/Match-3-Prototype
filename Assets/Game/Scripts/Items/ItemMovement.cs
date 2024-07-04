using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Game.Scripts.Core;
using Game.Scripts.Core.Interfaces;
using Match3System.Core.Interfaces;
using Match3System.Core.Models;
using UnityEngine;

public static class ItemMovement 
{
    public static async UniTask MoveItem(IItem item, Vector3 targetPos, float delay = .1f)
    {
        await item.Transform.DOMove(targetPos, .25f).SetEase(Ease.Flash);
    }
}
