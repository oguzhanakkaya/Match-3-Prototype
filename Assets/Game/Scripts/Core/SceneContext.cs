using Game.Scripts.Core.Interfaces;
using Game.Scripts.UI;
using Game.Scripts.UI.Interfaces;
using PoolSystem.Core;
using UnityEngine;

namespace Game.Scripts.Core
{
    public class SceneContext : BaseContext
    {
        [SerializeField] private GameView _gameView;
        [SerializeField] private InputSystem _inputSystem;
        [SerializeField] private PoolManager _poolManager;
        [SerializeField] private GameController _gameController;
        [SerializeField] private LevelController _levelController;
        [SerializeField] private GameData gameData;
        public override void Initialize()
        {
            base.Initialize();
            
            Register<IGameView>(_gameView);
            Register<IInputSystem>(_inputSystem);
            Register<PoolManager>(_poolManager);
            Register(GetGameSession());
            Register(_gameController);
            Register<LevelController>(_levelController);

            Container.Initialize();

            _gameView.Init();
            _gameController.Init();
            _inputSystem.Initialize();
            _levelController.Init();
        }

        private GameSession GetGameSession()
        {
            return new GameSession(this);
        }
        public GameController GetGameController()
        {
            return _gameController;
        }
        public GameData GetGameData()
        {
            return gameData;
        }
    }
}
