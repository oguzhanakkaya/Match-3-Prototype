using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Scripts.Core.DependencyInjection
{
    public class DependencyContainer
    {
        public static DependencyContainer Instance { get; } = new DependencyContainer();
        
        private Dictionary<Type, object> _registeredInstances;

        public virtual void Initialize()
        {
            _registeredInstances = new Dictionary<Type, object>();
        }
    
        public T Resolve<T>()
        {
            if (TryToResolve(typeof(T), out var obj))
            {
                return (T) obj;
            }

            return default;
        }

        public bool TryToResolve(Type type, out object obj)
        {
            if(_registeredInstances.TryGetValue(type, out obj)) return true;
            Debug.LogError($"Registered type({type}) is not registered!");
            return false;
        }

        public void Register<T>(T instance)
        {
            _registeredInstances.Add(typeof(T), instance);
        }
    }
}
