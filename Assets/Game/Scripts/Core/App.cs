using System;
using UnityEngine;

namespace Game.Scripts.Core
{
    public class App : MonoBehaviour
    {
        [SerializeField] private SceneContext _sceneContext;
        
        private void Awake()
        {
            _sceneContext.Initialize();
        }
    }
}
