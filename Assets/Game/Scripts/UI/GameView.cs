using Game.Scripts.Core;
using Game.Scripts.Core.Interfaces;
using Game.Scripts.Core.Models;
using Game.Scripts.Items;
using Game.Scripts.UI.Interfaces;
using PoolSystem;
using PoolSystem.Core;
using UnityEngine;

namespace Game.Scripts.UI
{
    public class GameView : MonoBehaviour, IGameView
    {
        [SerializeField] private SceneContext _sceneContext;

        private IInputSystem _inputSystem;
       // private IItemGenerator _nodeItemGenerator;

        private void Start()
        {
            _inputSystem = _sceneContext.Resolve<IInputSystem>();

           // _nodeItemGenerator = _sceneContext.Resolve<IItemGenerator>();
            
            _inputSystem.PointerDown += OnPointerDown;
        }

        private void OnPointerDown(object sender, PointerEventArgs e)
        {
          //  var itemBasic = _nodeItemGenerator.GetItem();
          //  itemBasic.Transform.position = e.WorldPosition;
        }
    }
}
