using PoolSystem;
using UnityEngine;

namespace Plugins.PoolSystem.Extensions
{
    public static class PoolingExtensions
    {
        public static void Recycle<T>(this IPoolObject<T> item) where T : Component
        {
            item.Pool.Recycle(item.Component);
        }
    }
}
