using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Buganamo.DD.Common
{
    [Serializable]
    public abstract class CollectionAsset<TAsset> : ScriptableObject
    where TAsset : UnityEngine.Object
    {
        [SerializeField]
        protected bool _shuffle = false;
        
        [SerializeField]
        protected List<TAsset> _assets;

        protected Dictionary<string, TAsset> _lookupTable = new();

        protected int _currentIndex = 0;
        protected List<TAsset> _shuffledList = new();
        protected List<TAsset> _preShuffledList = new();
        
        public List<TAsset> Assets => _assets;
        public int CurrentIndex => _currentIndex;

        public bool Shuffle
        {
            get => _shuffle;
            set => _shuffle = value;
        }

        public TAsset Current => AssetAtIndex(_currentIndex);
        public TAsset Next => AssetAtIndex(IncrementIndex);
        public TAsset AssetByName(string assetName) =>
            _lookupTable != null && _lookupTable.Count > 0 && _lookupTable.ContainsKey(assetName)
                ? _lookupTable[assetName]
                : null;

        public TAsset this[int index] { get => AssetAtIndex(index); }
        public TAsset this[string assetName] { get => AssetByName(assetName); }


        public virtual void InitCollection()
        {
            ResetCollection();
        }

        public virtual void ResetCollection()
        {
            _currentIndex = 0;
            if(_shuffle) ShuffleCollection();
        }

        protected virtual void InitLookupTable()
        {
            if (_assets == null || _assets.Count < 1) return;
            
            if (_lookupTable == null)
            {
                _lookupTable = new();
            }
            else
            {
                _lookupTable.Clear();
            }

            int i;
            int c = _assets.Count;
            string id;
            TAsset a;

            for (i = 0; i < c; i++)
            {
                a = _assets[i];
                if (a != null)
                {
                    id = a.name;
                    if (!_lookupTable.ContainsKey(id))
                    {
                        _lookupTable.Add(id, a);
                    }
                }
            }
        }

        protected virtual void ClearLookupTable()
        {
            if (_lookupTable != null)
            {
                _lookupTable.Clear();
                _lookupTable = null;
            }
        }


        private void Awake()
        {
            if (_shuffle)
            {
                ShuffleCollection();
            }
        }

        public virtual void ShuffleCollection()
        {
            _shuffledList.Clear();
            _preShuffledList.Clear();
            _preShuffledList.AddRange(_assets);

            int i;
            int c = _assets.Count;
            TAsset a;

            while (_preShuffledList.Count > 1)
            {
                i = Mathf.FloorToInt(Random.value * c);
                a = _preShuffledList[i];
                if (a != null)
                {
                    _shuffledList.Add(a);
                }
            }
        }

        protected TAsset AssetAtIndex(int index)
        {
            return _shuffle && _shuffledList != null && _shuffledList.Count > 0
                ? _shuffledList[Math.Clamp(index, 0, _shuffledList.Count - 1)]
                : (_assets != null && _assets.Count > 0) 
                    ?_assets[Math.Clamp(index, 0, _assets.Count - 1)]
                    : null;
        }

        protected int IncrementIndex
        {
            get
            {
                _currentIndex = (_currentIndex + 1) % _assets.Count;
                if (_currentIndex == 0 && _shuffle)
                {
                    ShuffleCollection();
                }

                return _currentIndex;
            }
        }
    }
}