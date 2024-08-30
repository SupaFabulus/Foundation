#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using SupaFabulus.Dev.Foundation.Core.Interfaces;
using SupaFabulus.Dev.Foundation.Core.Singletons;
using SupaFabulus.Dev.Foundation.Structures;
using UnityEngine;

namespace SupaFabulus.Dev.Foundation.EditorTools.PreviewCache
{
    [Serializable]
    public class PreviewCacheTable : InspectableDictionary<string, EditorPreviewCache> { }

    [Serializable]
    [CreateAssetMenu(
        fileName = "EditorPreviewCacheManager",
        menuName = @"Buganamo/MainFrame/" +
                   "Managers/" +
                   "Editor Preview Cache Manager"
    )]
    public class EditorPreviewCacheManager : 
        AbstractSingletonAsset<EditorPreviewCacheManager>,
        IManager, ISingleton<EditorPreviewCacheManager>
    {
        [SerializeField][HideInInspector]
        protected bool _active = false;
        public bool IsActive => _active;

        [SerializeField]
        protected EditorPreviewCache _defaultCacheAsset;
        
        [SerializeField]
        protected PreviewCacheTable _caches;
        public PreviewCacheTable Caches => _caches;
        


        protected override bool InitInstance()
        {
            BuildCacheTable();
            __initialized = true;
            return __initialized;
        }

        protected override void DeInitInstance()
        {
            DestroyCacheTable();
        }

        public bool Activate()
        {
            _active = true;
            

            return _active;
        }

        public void Deactivate()
        {
            _active = false;
            
            
        }

        public void Awake()
        {
            if (Validate())
            {
                InitInstance();
            }
        }

        public const string DEFAULT_CACHE_ID = "DEFAULT";
        public EditorPreviewCache Default => _defaultCacheAsset;
        
        public EditorPreviewCache this[string cacheID]
        {
            get => GetCacheSetByID(cacheID);
        }

        public bool AddCacheSet(string cacheID, EditorPreviewCache cacheSet)
        {
            if
            (
                _caches != null &&
                !_caches.ContainsKey(cacheID) &&
                !_caches.ContainsValue(cacheSet)
            )
            {
                _caches.Add(cacheID, cacheSet);
                return true;
            }

            return false;
        }

        public bool RemoveCacheSet(string cacheID)
        {
            if (_caches != null && _caches.ContainsKey(cacheID))
            {
                _caches.Remove(cacheID);
                return true;
            }

            return false;
        }

        public bool RemoveCacheSet(EditorPreviewCache cacheSet)
        {
            string id = GetIDForCacheSet(cacheSet);
            if (id != null)
            {
                _caches.Remove(id);
                return true;
            }

            return false;
        }

        public EditorPreviewCache GetCacheSetByID(string cacheID)
        {
            if (_caches != null && _caches.ContainsKey(cacheID))
            {
                return _caches[cacheID];
            }

            return null;
        }

        public string GetIDForCacheSet(EditorPreviewCache cacheSet)
        {
            if (_caches != null && _caches.ContainsValue(cacheSet))
            {
                foreach (KeyValuePair<string, EditorPreviewCache> entry in _caches)
                {
                    if (entry.Value == cacheSet)
                    {
                        return entry.Key;
                    }
                }
            }

            return null;
        }


        protected void BuildCacheTable()
        {
            if (_caches == null)
            {
                _caches = new PreviewCacheTable();
                AddCacheSet(DEFAULT_CACHE_ID, _defaultCacheAsset);
            }
        }

        protected void ResetAllCaches()
        {
            if (_caches != null)
            {
                string id;
                EditorPreviewCache cache;
                foreach (KeyValuePair<string, EditorPreviewCache> entry in _caches)
                {
                    id = entry.Key;
                    cache = entry.Value;

                    if (cache != null)
                    {
                        cache.ResetCache();
                    }
                }
            }
        }

        protected void DestroyCacheTable()
        {
            if (_caches != null)
            {
                _caches.Clear();
                _caches = null;
            }
        }
    }
}
#endif