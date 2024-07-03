using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.Scripts.Core.Models
{
    public class PointerEventArgs : EventArgs
    {
        public PointerEventArgs(PointerEventData.InputButton button, Vector3 worldPosition)
        {
            Button = button;
            WorldPosition = worldPosition;
        }

        public Vector3 WorldPosition { get; }
        public PointerEventData.InputButton Button { get; }
    }
}