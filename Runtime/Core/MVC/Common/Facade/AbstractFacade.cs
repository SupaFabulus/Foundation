using System;
using SupaFabulus.Dev.Foundation.Core.MVC.Common.Controller;
using SupaFabulus.Dev.Foundation.Core.MVC.Common.Facade.Enums;
using SupaFabulus.Dev.Foundation.Core.MVC.Common.Model;
using SupaFabulus.Dev.Foundation.Core.MVC.Common.View;
using UnityEngine;

namespace SupaFabulus.Dev.Foundation.Core.MVC.Common.Facade
{
    [Serializable]
    public abstract class AbstractFacade
    <
        TModel,
        TView,
        TController
    > 
        : AbstractView
        where TModel : UnityEngine.Object, IModel
        where TView : Component, IView
        where TController : Component, IController<TModel, TView>
    {
        [SerializeField]
        protected TModel _model;
        [SerializeField][HideInInspector]
        protected TView _view;
        [SerializeField][HideInInspector]
        protected TController _controller;

        [SerializeField]
        protected TView _viewInstanceReference;
        
        [SerializeField]
        protected bool _initOnStart = false;
        
        [SerializeField]
        protected bool _surviveSceneChange = false;

        protected virtual void CheckForSceneChangeProtection()
        {
            if(_surviveSceneChange) ProtectFromSceneChange();
        }
        protected virtual void ProtectFromSceneChange()
        {
            DontDestroyOnLoad(gameObject);
        }
        
        [SerializeField]
        protected FacadeComponentMode _controllerInitMode = FacadeComponentMode.FindOnSelf;
        [SerializeField]
        protected string _defaultControllerName = "[Controller]";
        
        [SerializeField][HideInInspector]
        protected bool _isRunning = false;

        protected abstract TMainComponent CreateExternalMainComponent<TMainComponent>(string gameObjectName, Transform parent = null)
            where TMainComponent : Component, IMainComponent;

        protected abstract TMainComponent CreateInternalMainComponent<TMainComponent>()
            where TMainComponent : Component, IMainComponent;

        protected virtual void Start()
        {
            if (_initOnStart)
            {
                InitFacade();
            }
        }

        public abstract void InitFacade();
        public abstract void DeInitFacade();
        
        protected abstract void InitModel();
        protected abstract void InitController();
        public virtual string DefaultControllerName => _defaultControllerName;

    }
}