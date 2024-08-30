using System;
using SupaFabulus.Dev.Foundation.Core.Signals;
using UnityEngine;

namespace SupaFabulus.Dev.Foundation.Components.UI
{
    
    [Serializable]
    public class ExpandableComponentEvents
    {
        [SerializeField]
        private Signal _onExpand = new();
        public Signal OnExpand => _onExpand;
        [SerializeField]
        private Signal _onContract = new();
        public Signal OnContract => _onContract;
    }
}