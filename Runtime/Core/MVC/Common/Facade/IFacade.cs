using SupaFabulus.Dev.Foundation.Core.MVC.Common.Controller;
using SupaFabulus.Dev.Foundation.Core.MVC.Common.Facade.Enums;
using SupaFabulus.Dev.Foundation.Core.MVC.Common.Model;
using SupaFabulus.Dev.Foundation.Core.MVC.Common.View;
using UnityEngine;

namespace SupaFabulus.Dev.Foundation.Core.MVC.Common.Facade
{
    public interface IFacade
    <
        out TModel,
        out TView,
        out TController
    > : IView
        where TModel : UnityEngine.Object, IModel
        where TView : Component, IView
        where TController : Component, IController<TModel, TView>
    {
        void InitFacade();
        void DeInitFacade();

        void StartFacade();
        void StopFacade();
        void ResetFacade();

        bool IsRunning { get; }
        FacadeComponentMode ViewInitMode { get; }
        FacadeComponentMode ControllerInitMode { get; }

        string DefaultControllerName { get; }
        string DefaultViewName { get; }

        TModel Model { get; }
        TView View { get; }
        TController Controller { get; }
    }
}