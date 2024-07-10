using Game.Scripts.Core.Models;
using UnityEngine;
public class GameEvents : MonoBehaviour
{

    public struct OnPointerDown : IEvent 
    {
        public PointerEventArgs point;
        public OnPointerDown(PointerEventArgs point)
        {
            this.point = point;
        }
    }
    public struct OnPointerUp : IEvent
    {
        public PointerEventArgs point;
        public OnPointerUp(PointerEventArgs point)
        {
            this.point = point;
        }
    }
    public struct OnMoveCompleted : IEvent
    {
        public int moveCount;
        public OnMoveCompleted(int moveCount)
        {
            this.moveCount = moveCount;
        }
    }
    public struct OnItemDestroyed : IEvent
    {
        public int remainingItemCount;
        public OnItemDestroyed(int remainingItemCount)
        {
            this.remainingItemCount = remainingItemCount;
        }
    }
    public struct OnLevelCompleted : IEvent { }
    public struct OnLevelFailed : IEvent { }
    public struct OnLevelLoaded : IEvent{

        public int moveCount,destroyItemCount;

        public OnLevelLoaded(int moveCount, int destroyItemCount)
        {
            this.moveCount = moveCount;
            this.destroyItemCount = destroyItemCount;
        }
    }
    public struct OnLevelStarted : IEvent {}


}