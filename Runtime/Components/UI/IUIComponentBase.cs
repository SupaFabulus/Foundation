using SupaFabulus.Dev.Foundation.Core.Components;

namespace SupaFabulus.Dev.Foundation.Components.UI
{
    public interface IUIComponentBase : IComponentBase
    {
        bool IsAnimated { get; set; }
        bool IsEnabled { get; set; }
        bool IsVisible { get; set; }
        bool IsFocused { get; set; }
        bool IsHighlighted { get; set; }
        bool IsActive { get; set; }
        
        void Enable(bool animate = true);
        void Disable(bool animate = true);
        void Show(bool animate = true);
        void Hide(bool animate = true);
        void Focus(bool animate = true);
        void UnFocus(bool animate = true);
        void Highlight(bool animate = true);
        void UnHighlight(bool animate = true);
        void Activate(bool animate = true);
        void DeActivate(bool animate = true);
    }
}