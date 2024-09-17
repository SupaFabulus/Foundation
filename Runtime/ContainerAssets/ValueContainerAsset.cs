using System;
using UnityEngine;

namespace SupaFabulus.Dev.Foundation.ContainerAssets
{
    [Serializable]
    public abstract class ValueContainerAsset<TValue> : ScriptableObject
    {
        [SerializeField]
        protected TValue _value;
        public virtual TValue Value => _value;
    }
}