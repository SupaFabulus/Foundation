using UnityEngine;

namespace SupaFabulus.Dev.Foundation.Components.UI
{
    public interface IRaycastTarget : IUIComponentBase
    {
        bool IsPointOfInterest { get; set; }
        
        GameObject GameObject { get; }
        Transform Transform { get; }
        string ObjectName { get; }
        string DisplayName { get; }
        string Description { get; }
    }
}