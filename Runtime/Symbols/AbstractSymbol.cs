using System;
using UnityEngine;

namespace SupaFabulus.Dev.Foundation.Symbols
{
    [Serializable]
    public abstract class AbstractSymbol : ScriptableObject
    {
        public string SymbolName => name;
    }
    
    
    
    [Serializable]
    public abstract class AbstractSymbol<TValue> : AbstractSymbol
    {
        [SerializeField]
        protected TValue _ID;
        public TValue ID => _ID;
    }
}