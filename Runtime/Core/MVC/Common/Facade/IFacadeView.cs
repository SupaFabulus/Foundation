using System;
using SupaFabulus.Dev.Foundation.Core.MVC.Common.Facade.Enums;

namespace SupaFabulus.Dev.Foundation.Core.MVC.Common.Facade
{
    public interface IFacadeView
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

        Type ModelType { get; }
        Type ViewType { get; }
        Type ControllerType { get; }
    }
}