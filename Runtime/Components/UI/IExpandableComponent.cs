using SupaFabulus.Dev.Foundation.Core.Components;

namespace SupaFabulus.Dev.Foundation.Components.UI
{
    public interface IExpandableComponent : IComponentBase
    {
        void Expand(bool animate = true);
        void Contract(bool animate = true);
    }
}