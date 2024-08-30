namespace SupaFabulus.Dev.Foundation.Core.Interfaces
{
    public interface IManager
    {
        bool IsActive { get; }

        bool Activate();
        void Deactivate();
    }
}