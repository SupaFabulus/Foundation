namespace SupaFabulus.Dev.Foundation.Core.Components
{
    public interface IComponentBase
    {
        bool IsInitialized { get; }

        void Init();
        void DeInit();
    }
}