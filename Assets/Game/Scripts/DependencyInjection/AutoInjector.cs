using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Game.Scripts.Core.DependencyInjection.Interfaces;
using UnityEngine;

namespace Game.Scripts.Core.DependencyInjection
{
    public class AutoInjector : MonoBehaviour
    {
        private readonly DependencyContainer _dependencyContainer = DependencyContainer.Instance;

        private List<MonoBehaviour> _monoBehaviours;

        private void Awake()
        {
            _monoBehaviours = GetComponents<MonoBehaviour>().ToList();
            
            InjectAll();
        }

        private void InjectAll()
        {
            foreach (var monoBehaviour in _monoBehaviours)
            {
                FieldInfo[] objectFields = monoBehaviour.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                for (int i = 0; i < objectFields.Length; i++)
                {
                    var objectField = objectFields[i];
                    if (Attribute.GetCustomAttribute(objectField, typeof(InjectAttribute)) is InjectAttribute attribute)
                    {
                        var fieldType = objectField.FieldType;

                        _dependencyContainer.TryToResolve(fieldType, out var objValue);
                        
                        objectField.SetValue(monoBehaviour, objValue);
                    }
                }

                monoBehaviour.TryGetComponent(out IHasAutoInjector hasAutoInjector);
                hasAutoInjector?.OnFieldValuesInjected();
            }
        }
    }
}
