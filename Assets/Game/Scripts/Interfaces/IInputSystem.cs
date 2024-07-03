using System;
using Game.Scripts.Core.Models;

namespace Game.Scripts.Core.Interfaces
{
    public interface IInputSystem
    {
        event EventHandler<PointerEventArgs> PointerDown;
        event EventHandler<PointerEventArgs> PointerDrag;
        event EventHandler<PointerEventArgs> PointerUp;
    }
}
