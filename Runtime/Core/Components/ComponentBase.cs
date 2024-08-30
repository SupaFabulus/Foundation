using System;
using UnityEngine;

namespace SupaFabulus.Dev.Foundation.Core.Components
{


    [Serializable]
    public abstract class ComponentBase : MonoBehaviour
    {
        [Header("Component Base")][Space(2)]
        [SerializeField]
        protected bool _initOnAwake = false;
        [SerializeField] 
        protected bool _autoDetectComponents = true;
        [SerializeField][HideInInspector]
        protected bool _isInitialized = false;


        public bool IsInitialized => _isInitialized;
        public bool AutoDetectComponents => _autoDetectComponents;
        
        

        private void Awake()
        {
            if (_initOnAwake)
            {
                Init();
            }
        }

        protected abstract void CacheComponents();
        protected abstract void ClearComponents();

        
        

        public virtual void Init()
        {
            if (!_isInitialized)
            {
                CacheComponents();
                _isInitialized = true;
            }
        }

        public virtual void DeInit()
        {
            ClearComponents();
            _isInitialized = false;
        }
        
        

        protected virtual void OnValidate()
        {
            if (_autoDetectComponents)
            {
                CacheComponents();
            }
        }

    }
}