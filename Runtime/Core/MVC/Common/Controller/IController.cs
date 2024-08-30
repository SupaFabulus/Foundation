using SupaFabulus.Dev.Foundation.Core.MVC.Common.Model;
using SupaFabulus.Dev.Foundation.Core.MVC.Common.View;
using UnityEngine;

namespace SupaFabulus.Dev.Foundation.Core.MVC.Common.Controller
{
    public interface IController<in TModel, in TView> : IMainComponent
    where TModel : UnityEngine.Object, IModel
    where TView : Component, IView
    {
        void InitController(TModel model, TView view);
        void DeInitController();

        void StartController();
        void StopController();
        void ResetController();

        bool IsInitialized { get; }
        bool IsRunning { get; }
    }
}