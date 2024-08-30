using System;
using SupaFabulus.Dev.Foundation.Core.Signals;
using UnityEngine;

namespace SupaFabulus.Dev.Foundation.Components.UI
{
    [Serializable]
    public class UIComponentBaseEvents
    {
        [SerializeField]
        private Signal _onActivate = new();
        public Signal OnActivate => _onActivate;
        [SerializeField]
        private Signal _onFocus = new();
        public Signal OnFocus => _onFocus;
        [SerializeField]
        private Signal _onUnFocus = new();
        public Signal OnUnFocus => _onUnFocus;
        [SerializeField]
        private Signal _onShow = new();
        public Signal OnShow => _onShow;
        [SerializeField]
        private Signal _onHide = new();
        public Signal OnHide => _onHide;
    }
}