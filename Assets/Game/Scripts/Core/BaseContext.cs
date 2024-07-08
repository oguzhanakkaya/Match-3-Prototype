using Game.Scripts.Core.DependencyInjection;
using Game.Scripts.Core.Interfaces;
using UnityEngine;

namespace Game.Scripts.Core
{
    public class BaseContext : MonoBehaviour, IContext
    {
        private readonly DependencyContainer _dependencyContainer = DependencyContainer.Instance;

        public virtual void Initialize()
        {
            _dependencyContainer.Initialize();
        }
    
        public T Resolve<T>()
        {
            return _dependencyContainer.Resolve<T>();
        }

        public void Register<T>(T instance)
        {
            _dependencyContainer.Register(instance);
        }
    }
}
