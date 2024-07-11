using Cysharp.Threading.Tasks;
using DG.Tweening;
using Game.Scripts.Core.Interfaces;
using UnityEngine;

public static class ItemMovement 
{
    public static async UniTask MoveItem(IItem item, Vector3 targetPos)
    {
        await item.Transform.DOMove(targetPos, .25f).SetEase(Ease.Flash);
    }
}
