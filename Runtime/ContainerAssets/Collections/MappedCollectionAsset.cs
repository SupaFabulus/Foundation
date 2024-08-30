using System;
using SupaFabulus.Dev.Foundation.Structures;
using UnityEngine;

namespace SupaFabulus.Dev.SpriteShot.Controllers
{
    [Serializable]
    public abstract class MappedCollectionAsset<TKey, TValue> : ScriptableObject
    {
        [SerializeField]
        protected InspectableDictionary<TKey, TValue> _source = new ();

        public TValue this[TKey key] =>
            (_source.ContainsKey(key)) ? _source[key] : default;

        public virtual void Init()
        {
            if (_source != null) _source.Init();
        }
        
        public virtual void DeInit()
        {
            if(_source != null) _source.DeInit();
        }

        /*
        private void OnEnable() => DeInit();
        private void OnDisable() => DeInit();
        private void Awake() => DeInit();
        */
    }
}