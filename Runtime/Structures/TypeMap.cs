using System;
using System.Collections.Generic;

namespace SupaFabulus.Dev.Foundation.Structures
{
    public class TypeMap<T>
    {
        private Dictionary<Type, T> _map;
        public Dictionary<Type, T> Map => _map;

        public T this[Type t]
        {
            get => _map != null && _map.ContainsKey(t)
                ? _map[t]
                : default;
            set
            {
                if (_map != null && _map.ContainsKey(t)) 
                    _map[t] = value;
            }
        }

        public bool ContainsKey(Type t) => _map != null && _map.ContainsKey(t);

        public bool Add(T inst)
        {
            if (inst == null) return false;
            if (_map == null) _map = new();
            Type t = typeof(T);
            if (!_map.ContainsKey(t))
            {
                _map.Add(t, inst);
                return true;
            }

            return false;
        }

        public bool Remove(Type type)
        {
            if (type == null || _map == null) return false;
            
            if (_map.ContainsKey(type))
            {
                _map.Remove(type);
                return true;
            }

            return false;
        }

        public bool Remove(T inst)
        {
            if (inst == null || _map == null) return false;
            Type t = typeof(T);
            if (_map.ContainsKey(t) && _map.ContainsValue(inst) && Equals(_map[t], inst))
            {
                _map.Remove(t);
                return true;
            }

            return false;
        }

        public void Clear() => _map?.Clear();
    }
}