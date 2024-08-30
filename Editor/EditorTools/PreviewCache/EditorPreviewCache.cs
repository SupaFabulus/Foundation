#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEngine;

namespace SupaFabulus.Dev.Foundation.EditorTools.PreviewCache
{
    [Serializable]
    [CreateAssetMenu(
        fileName = "EditorPreviewCache",
        menuName = @"Buganamo/MainFrame/" +
                   "Compositing/Utilities/" +
                   "Editor Preview Cache"
    )]
    public class EditorPreviewCache : ScriptableObject
    {

        
        
        protected Dictionary<long, RenderTexture> _cache = new();
        public Dictionary<long, RenderTexture> Entries => _cache;
        
        public RenderTexture this[int ownerInstanceID]
        {
            get => (_cache != null && _cache.ContainsKey(ownerInstanceID)) ? _cache[ownerInstanceID] : null;
        }

        public bool ContainsCacheForID(int id)
        {
            if (_cache != null)
            {
                return _cache.ContainsKey(id);
            }

            return false;
        }

        public bool Add(int id, RenderTexture cache)
        {
            if(_cache == null)
            {
                CreateCacheTable();
            }

            if (!_cache.ContainsKey(id))
            {
                _cache.Add(id, cache);
                return true;
            }
            else
            {
                Debug.LogWarning($"ID:[{id}] already exists in the cache!");
            }

            return false;
        }

        public void ResetCache()
        {
            if (_cache != null)
            {
                long id;
                RenderTexture t;
                foreach (KeyValuePair<long, RenderTexture> c in _cache)
                {
                    id = c.Key;
                    t = c.Value;
                    t.DiscardContents(true, true);
                    t.Release();
                    DestroyImmediate(t);
                    t = null;
                }
                
                _cache.Clear();
                _cache = null;
            }

            CreateCacheTable();
        }

        private void CreateCacheTable()
        {
            _cache = new Dictionary<long, RenderTexture>();
        }

        private void Awake()
        {
            
        }
    }
}
#endif