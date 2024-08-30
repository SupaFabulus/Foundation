using System;
using SupaFabulus.Dev.Foundation.Core.MVC.Common.Controller;
using SupaFabulus.Dev.Foundation.Core.MVC.Common.Model;
using SupaFabulus.Dev.Foundation.Core.MVC.Common.View;
using UnityEngine;

namespace SupaFabulus.Dev.Foundation.Core.MVC.Bases
{
    
    [Serializable]
    public abstract class ControllerBase<TModel, TView> : 
        AbstractController<TModel, TView>, 
        IController<TModel, TView>
    where TModel : UnityEngine.Object, IModel
    where TView : Component, IView
    {
        public bool IsInitialized => _isInitialized;
        public bool IsRunning => _isRunning;
        
        
        public virtual void InitController(TModel model, TView view)
        {
            if (!_isInitialized)
            {
                _model = model;
                _view = view;
                
                Debug.Log($"[{gameObject.name}.Controller] Initialized -- Model: [{_model}], View: [{_view}]");
                
                _isInitialized = true;
            }
        }

        public void DeInitController()
        {
            _isInitialized = false;
        }

        public void StartController()
        {
            _isRunning = true;
        }
        
        public void StopController()
        {
            _isRunning = false;
        }

        public void ResetController()
        {
            
        }
    }
}