using System;
using SupaFabulus.Dev.Foundation.Core.MVC.Common.Facade.Enums;
using SupaFabulus.Dev.Foundation.Core.MVC.Common.View;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SupaFabulus.Dev.Foundation.Core.MVC.Bases
{
    [Serializable]
    public abstract class ViewBase : AbstractView, IView
    {

        public virtual bool IsInitialized => _isInitialized;
        public virtual bool IsActive => _isActive;
        public virtual bool IsVisible => _isVisible;


        public virtual void InitView()
        {
            if (!_isInitialized)
            {
                CacheComponents();
                InitMainSubView();
                _isInitialized = true;
            }
        }

        public virtual void DeInitView()
        {
            Deactivate();
            ClearComponents();
            _isInitialized = false;
        }

        public virtual void Activate()
        {
            StartEventHandlers();
            _isActive = true;
        }

        public virtual void Deactivate()
        {
            StopEventHandlers();
            _isActive = false;
        }

        public virtual void ResetView()
        {
            _isActive = false;
        }

        public virtual void Show()
        {
            _isVisible = true;
        }

        public virtual void Hide()
        {
            _isVisible = false;
        }

        protected override void InitMainSubView()
        {
            switch (_viewInitMode)
            {
                case FacadeComponentMode.InstantiatePrefabReference:
                    ViewInit_InstantiateViewPrefab();
                    break;
                
                case FacadeComponentMode.LoadScene:
                    ViewInit_LoadViewScene();
                    break;
                
                case FacadeComponentMode.FindOnSelf:
                case FacadeComponentMode.CreateOnSelf:
                case FacadeComponentMode.ConstructNewChild:
                case FacadeComponentMode.ConstructNewSibling:
                default:
                    Debug.LogWarning($"Unhandled ViewMode: [{_viewInitMode}]");
                    return;
            }
        }

        protected override bool ViewInit_InstantiateViewPrefab()
        {
            if
            (
                _viewPrefabAsset != null && 
                _viewPrefabAsset.gameObject != null
            )
            {
                GameObject gObj = GameObject.Instantiate(_viewPrefabAsset.gameObject);
                gObj.name = _defaultViewName;
                return true;
            }

            return false;
        }

        protected override bool ViewInit_LoadViewScene()
        {
            if
            (
                _viewSceneName != null && 
                !String.IsNullOrEmpty(_viewSceneName) && 
                !String.IsNullOrWhiteSpace(_viewSceneName)
            )
            {
                LoadSceneParameters p = new LoadSceneParameters
                    (LoadSceneMode.Additive, LocalPhysicsMode.Physics3D);
                SceneManager.LoadScene(_viewSceneName, p);
                Scene scene = SceneManager.GetSceneByName(_viewSceneName);
                
                if (scene.IsValid())
                {
                    return true;
                }
            }

            Debug.LogError($"Unable to load View Scene: [{_viewSceneName}]");
            return false;
        }
    }
}