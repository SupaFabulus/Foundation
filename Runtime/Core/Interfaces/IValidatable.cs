using SupaFabulus.Dev.Foundation.Core.Signals;

namespace SupaFabulus.Dev.Foundation.Core.Interfaces
{
    public interface IValidatable<TOrigin>
    {
        bool Valid { get; }
        void Validate();
        void Invalidate();

        Signal<TOrigin> OnValidated { get; }
        Signal<TOrigin> OnInvalidated { get; }

        void NotifyValidated();
        void NotifyInvalidated();
    }
}