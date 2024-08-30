using System;
using UnityEngine;

namespace SupaFabulus.Dev.Foundation.Core.MVC.Common.Model
{
    [Serializable]
    public abstract class AbstractComponentModel : MonoBehaviour
    {
        [SerializeField]
        protected bool _isInitialized = false;
        [SerializeField]
        protected bool _resetOnInit = false;
    }
}