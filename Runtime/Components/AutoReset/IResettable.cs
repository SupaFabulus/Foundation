namespace SupaFabulus.Dev.Foundation.Components.AutoReset
{
    public interface IResettable
    {
        void RestoreInitialState();
        void RestoreResetState();
        void RestoreMostRecentState();
        void CacheMostRecentState();
        void SaveResetState();
    }
}