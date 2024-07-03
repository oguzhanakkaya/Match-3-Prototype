using System;
using Game.Scripts.Core.Interfaces;
using Game.Scripts.UI.Interfaces;

namespace Game.Scripts.Core
{
    public class GameSession : IDisposable
    {
        private IContext _context;
        private IItemGenerator _itemGenerator;
        private IGameView _gameView;
        public GameSession(IContext context)
        {
            _context = context;
            // _unityGame = context.Resolve<UnityGame>();
            // _iconSets = context.Resolve<IconsSetModel[]>();
            _gameView = context.Resolve<IGameView>();
            _itemGenerator = context.Resolve<NodeItemGenerator>();
        }
        
        public void Init()
        {
        }
        public void Dispose()
        {
        }
    }
}
