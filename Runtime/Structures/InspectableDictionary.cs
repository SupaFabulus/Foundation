using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

namespace SupaFabulus.Dev.Foundation.Structures
{
    public struct ForLoopIterator
    {
        private int _initialVal;
        private int _currentVal;
        private int _terminator;
        private int _increment;
        
        public ForLoopIterator
        (
            int initialValue,
            int currentValue,
            int terminalValue,
            int increment = 1
        )
        {
            _initialVal = initialValue;
            _currentVal = currentValue;
            _terminator = terminalValue;
            _increment = increment;
        }

        public int InitialValue => _initialVal;
        public int CurrentValue
        {
            get => _currentVal;
            set => _currentVal = value;
        }
        public int TerminalValue => _terminator;
        public int IncrementValue => _increment;
    }

    public interface IInspectableDictionaryEntry<TKey, TValue>
    {
        TKey Key { get; }
        TValue Value { get; }
    }

    [Serializable]
    public struct MutableInspectableDictionaryEntry<TKey, TValue>
        : IInspectableDictionaryEntry<TKey, TValue>
    {
        public TKey Key
        {
            get => _key;
            set => _key = value;
        }

        public TValue Value
        {
            get => _value;
            set => _value = value;
        }

        [SerializeField]
        private TKey _key;
        [SerializeField]
        private TValue _value;

        
        public MutableInspectableDictionaryEntry(TKey key, TValue value)
        {
            _key = key;
            _value = value;
        }

        
        public MutableInspectableDictionaryEntry(InspectableDictionaryEntry<TKey, TValue> immutableSource)
        {
            _key = immutableSource.Key;
            _value = immutableSource.Value;
        }
        
        public InspectableDictionaryEntry<TKey, TValue> ImmutableCopy => new(_key, _value);
        
        public void Set(TKey key, TValue value)
        {
            _key = key;
            _value = value;
        }

        public void Clear()
        {
            _key = default;
            _value = default;
        }

    }
    
    [Serializable]
    public struct InspectableDictionaryEntry<TKey, TValue>
    {

        [SerializeField]
        private TKey _key;
        [SerializeField]
        private TValue _value;
        
        public TKey Key => _key;
        public TValue Value => _value;

        
        public InspectableDictionaryEntry(TKey key, TValue value)
        {
            _key = key;
            _value = value;
        }

        public void Clear()
        {
            _key = default;
            _value = default;
        }
    }

    [Serializable]
    public class InspectableDictionary<TKey, TValue> :
        ICollection,
        ICollection<KeyValuePair<TKey, TValue>>,
        IReadOnlyCollection<KeyValuePair<TKey, TValue>>,
        IDictionary,
        IDictionary<TKey, TValue>,
        IReadOnlyDictionary<TKey, TValue>,
        IEnumerable,
        IEnumerable<KeyValuePair<TKey, TValue>>,
        ISerializable,
        IDeserializationCallback
    {

        
        // 
        /*----------------------------------------------------------------------------------*/

        private bool _isSynchronized;
        private object _syncRoot;
        private bool _isReadOnly;
        private bool _isFixedSize;
        private ICollection<TKey> _keys;
        private ICollection<TValue> _values;

        // 
        /*----------------------------------------------------------------------------------*/
        
        
        [SerializeField]
        protected bool _forceUniqueKeys = true;
        [SerializeField]
        protected bool _autoRefresh = false;
        
        [SerializeField]
        internal List<MutableInspectableDictionaryEntry<TKey, TValue>> _entries = new ();

        protected Dictionary<TKey, TValue> _map = new();

        public List<MutableInspectableDictionaryEntry<TKey, TValue>> Entries => new (_entries);
        
        public Dictionary<TKey, TValue> Map => _map;

        public bool Sanitize(bool removeOffenders = true)
        {
            bool foundOffenders = false;

            string[] keyNames = Enum.GetNames(typeof(TKey));
            TKey[] keys = (TKey[])Enum.GetValues(typeof(TKey));

            if (keyNames.Length != keys.Length)
            {
                return true;
            }

            int index;
            int entryCount;
            int i;
            int c;
            
            int i_entries;
            int c_entries = _entries.Count;

            int i_keys;
            int c_keys = keys.Length;

            bool hasEntry = false;
            bool foundReplacement = false;
            TKey k;
            TKey _k;
            TValue v;
            MutableInspectableDictionaryEntry<TKey, TValue> e;
            Dictionary<TKey, int> _existsMap = new();
            for (i_keys = 0; i_keys < c_keys; i_keys++)
            {
                k = keys[i_keys];
                index = IndexOf(k);
                entryCount = 0;
                
                if (!_existsMap.ContainsKey(k))
                {
                    _existsMap.Add(k, 0);
                }
                
                for (i_entries = c_entries-1; i_entries >= 0; i_entries--)
                {
                    e = _entries[i_entries];

                    if (e.Key.Equals(k))
                    {
                        entryCount++;
                    }
                }

                foundOffenders |= (entryCount > 1);
                _existsMap[k] = entryCount;
                
                //Debug.Log($"[{k.ToString()} = {entryCount}]");
            }
            
            for (i_entries = c_entries-1; i_entries >= 0; i_entries--)
            {
                foundReplacement = false;
                e = _entries[i_entries];
                k = e.Key;
                entryCount = _existsMap[k];

                if (entryCount > 1)
                {
                    for (i_keys = 0; i_keys < c_keys; i_keys++)
                    {
                        _k = keys[i_keys];
                        if (_existsMap[_k] == 0)
                        {
                            e.Key = _k;
                            _existsMap[_k] = 1;
                            foundReplacement = true;
                            break;
                        }
                    }

                    if (foundReplacement)
                    {
                        _entries[i_entries] = e;
                        _existsMap[k]--;
                    }
                    else if (removeOffenders)
                    {
                        _entries.RemoveAt(i_entries);
                        _existsMap[k]--;
                    }
                }
            }
            

            return foundOffenders;
        }

        protected bool _isInitialized = false;

        public void Init()
        {
            if (_isInitialized) DeInit();
            if (!_isInitialized)
            {
                RebuildInternalMap();
            }
        }

        protected void RebuildInternalMap()
        {
            foreach (var e in _entries)
            {
                if (!_map.ContainsKey(e.Key))
                {
                    _map.Add(e.Key, e.Value);
                }
            }
        }

        public void DeInit()
        {
            _map.Clear();
        }

        public TValue this[TKey key]
        {
            get
            {
                if (ContainsKey(key))
                {
                    return GetEntryForKey(key).Value;
                }

                return default;
            }
            set
            {
                if (ContainsKey(key))
                {
                    MutableInspectableDictionaryEntry<TKey, TValue> entry = GetMutableEntryForKey(key);
                    int index = IndexOf(entry.Value);
                    entry.Value = value;
                    _entries[index] = entry;
                }
            }
        }

        public TKey KeyForValue(TValue value)
        {
            int i = 0;
            int c = _entries.Count;
            MutableInspectableDictionaryEntry<TKey, TValue> entry;
            for (i = 0; i < c; i++)
            {
                if (i < _entries.Count)
                {
                    entry = _entries[i];
                    if (entry.Value.Equals(value))
                        return entry.Key;
                }
            }

            return default;
        }



        public void ForEach(Action<ForLoopIterator, TKey, TValue> entryAction)
        {
            
            if (entryAction == null) return;

            int i = 0;
            int c = _entries.Count;
            MutableInspectableDictionaryEntry<TKey, TValue> entry;
            ForLoopIterator iterator = new(0, i, c);
            for (i = 0; i < c; i++)
            {
                iterator.CurrentValue = i;
                if (i < _entries.Count)
                {
                    entry = _entries[i];
                    //Debug.Log($"({entry.Key}) -- Verify Index [{i}] of Count: {c} ?== {_entries.Count}");
                    entryAction(iterator, entry.Key, entry.Value);
                }
                else
                {
                    Debug.LogWarning("Skipping out of bounds");
                }
            }
        }

        public delegate TReturnType IterationEntryDelegate<TReturnType>(ForLoopIterator iterator, TKey key, TValue value);
        public TReturnType[] ForEach<TReturnType>(IterationEntryDelegate<TReturnType> entryDelegate)
        {
            if (entryDelegate == null) return null;

            int i = 0;
            int c = _entries.Count;
            MutableInspectableDictionaryEntry<TKey, TValue> entry;
            TReturnType[] results = new TReturnType[c];
            ForLoopIterator iterator = new(0, i, c);

            for (i = 0; i < c; i++)
            {
                iterator.CurrentValue = i;
                entry = _entries[i];
                results[i] = entryDelegate(iterator, entry.Key, entry.Value);
            }

            return results;
        }

        public bool ContainsKey(TKey key)
        {
            foreach (MutableInspectableDictionaryEntry<TKey, TValue> e in _entries)
            {
                //Debug.Log($"Keys: {key} ?== {e.Key}");
                if (e.Key.Equals(key)) return true;
            }

            return false;
        }

        public bool ContainsValue(TValue value)
        {
            foreach (MutableInspectableDictionaryEntry<TKey, TValue> e in _entries)
            {
                //Debug.Log($"Keys: {key} ?== {e.Key}");
                if (e.Value.Equals(value)) return true;
            }

            return false;
        }


        internal MutableInspectableDictionaryEntry<TKey, TValue> GetMutableEntryForKey(TKey key)
        {
            int i;
            int c = _entries.Count;

            for (i = 0; i < c; i++)
            {
                if (_entries[i].Key.Equals(key))
                {
                    return _entries[i];
                }
            }

            return default;
        }


        public int IndexOf(TKey key)
        {
            int i;
            int c = _entries.Count;
            MutableInspectableDictionaryEntry<TKey, TValue> e;

            for (i = 0; i < c; i++)
            {
                e = _entries[i];
                if (e.Key.Equals(key)) return i;
            }

            return -1;
        }


        public int IndexOf(TValue value)
        {
            int i;
            int c = _entries.Count;
            MutableInspectableDictionaryEntry<TKey, TValue> e;

            for (i = 0; i < c; i++)
            {
                e = _entries[i];
                if (e.Value.Equals(value)) return i;
            }

            return -1;
        }


        public InspectableDictionaryEntry<TKey, TValue> GetEntryForKey(TKey key)
        {
            int i;
            int c = _entries.Count;

            for (i = 0; i < c; i++)
            {
                if (_entries[i].Key.Equals(key))
                {
                    //Debug.Log($"Found match: {_entries[i].Key} ?= {key}");
                    return _entries[i].ImmutableCopy;
                }
            }

            //Debug.Log($"No Found match for: {key}");
            return default;
        }

        public InspectableDictionaryEntry<TKey, TValue> GetEntryAtIndex(int index)
        {
            if (index >= 0 && index < _entries.Count)
            {
                return _entries[index].ImmutableCopy;
            }

            return default;
        }

        public bool Add(TKey key, TValue value, bool autoRefreshMap = true)
        {
            bool found = false;
            foreach (MutableInspectableDictionaryEntry<TKey, TValue> entry in _entries)
            {
                if (entry.Key.Equals(key))
                {
                    return false;
                }
            }
            
            _entries.Add(new MutableInspectableDictionaryEntry<TKey, TValue>(key, value));

            return true;
        }

        public bool Remove(TKey key, bool autoRefreshMap = true)
        {
            bool found = false;

            int i;
            int c = _entries.Count;
            MutableInspectableDictionaryEntry<TKey, TValue> entry;

            for (i = c-1; i >= 0; i--)
            {
                entry = _entries[i];
                if (entry.Key.Equals(key))
                {
                    _entries.RemoveAt(i);
                    found = true;
                }
            }
            
            return found;
        }
        
        public void Clear()
        {
            _map.Clear();
            _entries.Clear();
        }
        
        public bool IsFixedSize => _isFixedSize;

        
        
        
        
        
        
        
        
        
        // 
        /*----------------------------------------------------------------------------------*/

        
        public void CopyTo(Array array, int index)
        {
            throw new NotImplementedException();
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            throw new NotImplementedException();
        }

        public int Count => _entries.Count;

        public bool IsReadOnly => _isReadOnly;

        public object this[object key]
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

        ICollection IDictionary.Keys => (ICollection)_keys;
        ICollection IDictionary.Values => (ICollection)_values;

        ICollection<TKey> IDictionary<TKey, TValue>.Keys => _keys;
        ICollection<TValue> IDictionary<TKey, TValue>.Values => _values;

        IEnumerable<TKey> IReadOnlyDictionary<TKey, TValue>.Keys => _keys;
        IEnumerable<TValue> IReadOnlyDictionary<TKey, TValue>.Values => _values;

        public bool IsSynchronized => _isSynchronized;
        public object SyncRoot => _syncRoot;

        bool IDictionary<TKey, TValue>.TryGetValue(TKey key, out TValue value)
        {
            throw new NotImplementedException();
        }

        bool IReadOnlyDictionary<TKey, TValue>.TryGetValue(TKey key, out TValue value)
        {
            throw new NotImplementedException();
        }

        public void Add(TKey key, TValue value)
        {
            Add(key, value, true);
        }

        public bool Remove(TKey key)
        {
            return Remove(key, true);
        }


        public void Add(KeyValuePair<TKey, TValue> item)
        {
            throw new NotImplementedException();
        }

        public void Add(object key, object value)
        {
            throw new NotImplementedException();
        }


        public bool Contains(object key)
        {
            throw new NotImplementedException();
        }

        IDictionaryEnumerator IDictionary.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public void Remove(object key)
        {
            throw new NotImplementedException();
        }


        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            throw new NotImplementedException();
        }

        public void OnDeserialization(object sender)
        {
            throw new NotImplementedException();
        }
    }
}