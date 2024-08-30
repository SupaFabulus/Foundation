using SupaFabulus.Dev.Foundation.Core.MVC.Common.Model;
using SupaFabulus.Dev.Foundation.Core.MVC.Common.View;
using UnityEngine;

namespace SupaFabulus.Dev.Foundation.Core.MVC.Common.Controller
{
    public abstract class AbstractController<TModel, TView> : MonoBehaviour
    where TModel : UnityEngine.Object, IModel
    where TView : UnityEngine.Object, IView
    {
        [SerializeField][HideInInspector]
        protected TModel _model;
        [SerializeField][HideInInspector]
        protected TView _view;
        
        [SerializeField][HideInInspector]
        protected bool _isInitialized = false;
        [SerializeField][HideInInspector]
        protected bool _isRunning = false;
        
    }
}