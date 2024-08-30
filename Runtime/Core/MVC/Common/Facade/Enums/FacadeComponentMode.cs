using System;

namespace SupaFabulus.Dev.Foundation.Core.MVC.Common.Facade.Enums
{
    [Serializable]
    public enum FacadeComponentMode
    {
        FindOnSelf,
        CreateOnSelf,
        ConstructNewChild,
        ConstructNewSibling,
        InstantiatePrefabReference,
        LoadScene,
        UseInstanceReference,
        FindInCurrentScene,
        AddressableID
    }
}