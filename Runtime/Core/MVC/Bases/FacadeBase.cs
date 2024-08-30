using System;
using SupaFabulus.Dev.Foundation.Core.MVC.Common.Controller;
using SupaFabulus.Dev.Foundation.Core.MVC.Common.Facade;
using SupaFabulus.Dev.Foundation.Core.MVC.Common.Facade.Enums;
using SupaFabulus.Dev.Foundation.Core.MVC.Common.Model;
using SupaFabulus.Dev.Foundation.Core.MVC.Common.View;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SupaFabulus.Dev.Foundation.Core.MVC.Bases
{
    [Serializable]
    public abstract class FacadeBase<TModel, TView, TController> :
        AbstractFacade<TModel, TView, TController>,
        IFacade<TModel, TView, TController>

        where TModel : UnityEngine.Object, IModel
        where TView : Component, IView
        where TController : Component, IController<TModel, TView>
    {
        public TModel Model => _model;
        public TView View => _view;
        public TController Controller => _controller;

        public bool IsRunning => _isRunning;
        public FacadeComponentMode ViewInitMode => _viewInitMode;
        public FacadeComponentMode ControllerInitMode => _controllerInitMode;

        

        public override void InitFacade()
        {
            if (!_isInitialized)
            {
                if (_model == null)
                {
                    Debug.LogError($"Cannot Initialize Facade [{this.GetType().Name}] without Model [{typeof(TModel).Name}]!");
                    //Application.Quit(-1);
                    return;
                }

                Debug.Log($"{gameObject.name}.{this.GetType().Name}::InitFacade");
                
                CheckForSceneChangeProtection();
                InitModel();
                InitView();
                InitController();

                _isInitialized = true;
            }
        }

        

        public override void DeInitFacade()
        {
            ResetFacade();
            _isInitialized = false;
        }

        public virtual void StartFacade()
        {
            _isRunning = true;
        }

        public virtual void StopFacade()
        {
            _isRunning = false;
        }

        public virtual void ResetFacade()
        {
            _isActive = false;
            _isRunning = false;
        }

        protected override TMainComponent CreateExternalMainComponent<TMainComponent>(string gameObjectName, Transform parent = null)
        {
            GameObject gObj = new GameObject(gameObjectName);
            Transform tx = gObj.transform;
            tx.SetParent(parent != null ? parent : transform);

            return gObj.AddComponent<TMainComponent>();
        }

        protected override TMainComponent CreateInternalMainComponent<TMainComponent>()
        {
            return gameObject.AddComponent<TMainComponent>();
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
                _view = gObj.GetComponent<TView>();

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

                if (!scene.IsValid())
                {
                    Debug.LogError($"Unable to Load Scene!");
                    return false;
                }

                int count = scene.rootCount;
                if (count > 0)
                {
                    int i;
                    TView instance;
                    GameObject rootSibling;
                    GameObject[] sceneRoots = scene.GetRootGameObjects();

                    for (i = 0; i < count; i++)
                    {
                        rootSibling = sceneRoots[i];
                        
                        instance = rootSibling.GetComponent<TView>();
                        if (instance != null)
                        {
                            _view = instance;
                            return true;
                        }

                        instance = rootSibling.GetComponentInChildren<TView>();
                        
                        if (instance == null) continue;
                        
                        _view = instance;
                        return true;
                    }
                }
            }

            return false;
        }

        protected override void InitModel()
        {
            if (_model.IsInitialized)
            {
                _model.ResetModel();
            }
            else
            {
                _model.InitModel();
            }
        }

        protected override void InitMainSubView()
        {
            switch (_viewInitMode)
            {
                case FacadeComponentMode.UseInstanceReference:
                    if (_viewInstanceReference != null)
                    {
                        _view = _viewInstanceReference;
                    }
                    else
                    {
                        Debug.LogError($"Unable to find View Instance Reference!");
                    }

                    break;
                case FacadeComponentMode.FindOnSelf:
                    _view = gameObject.GetComponent<TView>();
                    break;
                
                case FacadeComponentMode.CreateOnSelf:
                    _view = CreateInternalMainComponent<TView>();
                    break;
                
                case FacadeComponentMode.ConstructNewChild:
                    _view = CreateExternalMainComponent<TView>(DefaultViewName, transform);
                    break;
                
                case FacadeComponentMode.ConstructNewSibling:
                    _view = CreateExternalMainComponent<TView>(DefaultViewName, transform.parent);
                    break;
                
                case FacadeComponentMode.InstantiatePrefabReference:
                    ViewInit_InstantiateViewPrefab();
                    break;
                
                case FacadeComponentMode.LoadScene:
                    ViewInit_LoadViewScene();
                    break;
                
                default:
                    Debug.LogError($"Unhandled ViewMode: [{_viewInitMode}]");
                    return;
            }

            if (_view != null)
            {
                IFacadeView v = _view as IFacadeView;
                if (v != null) v.InitFacade();
                else _view.InitView();
            }
        }

        protected override void InitController()
        {
            
            
            switch (_controllerInitMode)
            {
                case FacadeComponentMode.FindOnSelf:
                    _controller = gameObject.GetComponent<TController>();
                    break;
                
                case FacadeComponentMode.CreateOnSelf:
                    _controller = CreateInternalMainComponent<TController>();
                    break;
                
                case FacadeComponentMode.ConstructNewChild:
                    _controller = CreateExternalMainComponent<TController>(DefaultControllerName, transform);
                    break;
                
                case FacadeComponentMode.ConstructNewSibling:
                    _controller = CreateExternalMainComponent<TController>(DefaultControllerName, transform.parent);
                    break;
                
                case FacadeComponentMode.InstantiatePrefabReference:
                case FacadeComponentMode.LoadScene:
                default:
                    Debug.LogError($"Unhandled ViewMode: [{_viewInitMode}]");
                    return;
            }

            if (_controller != null)
            {
                _controller.InitController(_model, _view);
            }
            else
            {
                Debug.LogError($"Unable to find Controller for Facade [{gameObject.name}]");
            }
        }

        public void InitView()
        {
            throw new NotImplementedException();
        }

        public void DeInitView()
        {
            Deactivate();
            
            if (_controller != null)
            {
                _controller.DeInitController();
            }

            if (_view != null)
            {
                _view.DeInitView();
            }

            if (_model != null)
            {
                _model.DeInitModel();
            }
            
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

        public virtual bool IsInitialized => _isInitialized;
        public virtual bool IsActive => _isActive;
        public virtual bool IsVisible => _isVisible;
    }
}