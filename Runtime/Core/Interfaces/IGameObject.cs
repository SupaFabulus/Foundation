using UnityEngine;

namespace SupaFabulus.Dev.Foundation.Core.Interfaces
{
    public interface IGameObject
    {
        GameObject gameObject { get; }
        Transform transform { get; }
    }
}