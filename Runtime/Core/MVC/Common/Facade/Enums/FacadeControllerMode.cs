using System;

namespace SupaFabulus.Dev.Foundation.Core.MVC.Common.Facade.Enums
{
    [Serializable]
    public enum FacadeControllerMode
    {
        Self,
        CreatedInternal,
        CreatedExternal,
        InstantiatedPrefab
    }
}