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
    public static async UniTask MoveItemAsyncToGoal(IItem item, Vector3 targetPos, float delay = .1f)
    {
        await item.Transform.DOMove(targetPos, delay).SetEase(Ease.Flash);
    }
    public static void MoveItem(IItem item, Vector3 targetPos,float delay=.1f)
    {
        item.Transform.DOMove(targetPos, delay).SetEase(Ease.Flash);
    }
    public static async UniTask MoveObject(GameObject item, Vector3 targetPos, float delay = .1f)
    {
        await item.transform.DOMove(targetPos, delay).SetEase(Ease.Flash);
    }

}
