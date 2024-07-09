using System;
using Game.Scripts.Core.Interfaces;
using Game.Scripts.Core.Models;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.Scripts.Core
{
    public class InputSystem : MonoBehaviour, IInputSystem
    {
        public event EventHandler<PointerEventArgs> PointerDown;
        public event EventHandler<PointerEventArgs> PointerDrag;
        public event EventHandler<PointerEventArgs> PointerUp;
    
        [SerializeField] private Camera _camera;
        [SerializeField] private EventTrigger _eventTrigger;

        private EventBus _eventBus;

        public void Initialize()
        {
            _eventBus = ServiceLocator.Instance.Resolve<EventBus>();

            var pointerDown = new EventTrigger.Entry { eventID = EventTriggerType.PointerDown };
            pointerDown.callback.AddListener(data => { OnPointerDown((PointerEventData) data); });

            var pointerDrag = new EventTrigger.Entry { eventID = EventTriggerType.Drag };
            pointerDrag.callback.AddListener(data => { OnPointerDrag((PointerEventData) data); });

            var pointerUp = new EventTrigger.Entry { eventID = EventTriggerType.PointerUp };
            pointerUp.callback.AddListener(data => { OnPointerUp((PointerEventData) data); });

            _eventTrigger.triggers.Add(pointerDown);
            _eventTrigger.triggers.Add(pointerDrag);
            _eventTrigger.triggers.Add(pointerUp);
        }

        private void OnPointerDown(PointerEventData e)
        {
            _eventBus.Fire(new GameEvents.OnPointerDown(GetPointerEventArgs(e)));
        }

        private void OnPointerDrag(PointerEventData e)
        {
        }

        private void OnPointerUp(PointerEventData e)
        {
            _eventBus.Fire(new GameEvents.OnPointerUp(GetPointerEventArgs(e)));
        }

        private PointerEventArgs GetPointerEventArgs(PointerEventData e)
        {
            return new PointerEventArgs(e.button, GetWorldPosition(e.position));
        }

        private Vector2 GetWorldPosition(Vector2 screenPosition)
        {
            return _camera.ScreenToWorldPoint(screenPosition);
        }
    }
}
