using System;
using SupaFabulus.Dev.Foundation.Core.MVC.Common.Facade.Enums;
using UnityEngine;

namespace SupaFabulus.Dev.Foundation.Core.MVC.Common.View
{
    [Serializable]
    public abstract class AbstractView : MonoBehaviour
    {
        [SerializeField][HideInInspector]
        protected bool _isInitialized = false;
        [SerializeField][HideInInspector]
        protected bool _isActive = false;
        [SerializeField][HideInInspector]
        protected bool _isVisible = false;
        
        [SerializeField]
        protected string _viewSceneName;
        [SerializeField]
        protected GameObject _viewPrefabAsset;
        [SerializeField]
        protected string _defaultViewName = "[View]";
        [SerializeField]
        protected FacadeComponentMode _viewInitMode = FacadeComponentMode.FindOnSelf;


        protected abstract void CacheComponents();
        protected abstract void ClearComponents();
        protected abstract void StartEventHandlers();
        protected abstract void StopEventHandlers();
        protected abstract void InitMainSubView();
        protected abstract bool ViewInit_InstantiateViewPrefab();
        protected abstract bool ViewInit_LoadViewScene();
        public virtual string DefaultViewName => _defaultViewName;
    }
}