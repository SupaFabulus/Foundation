using System;

namespace SupaFabulus.Dev.Foundation.Core.MVC.Common.Facade.Enums
{
    [Serializable]
    public enum FacadeViewMode
    {
        None,
        GetFromSelf,
        AttachToSelf,
        Constructed,
        PrefabReference,
        Scene,
        AddressableID
    }
}