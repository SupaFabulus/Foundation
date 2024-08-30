using SupaFabulus.Dev.Foundation.Core.Interfaces;

namespace SupaFabulus.Dev.Foundation.Components.AutoReset
{
    public interface IResettableState<TResetData, TResetTarget> : IInitWith<TResetTarget>
    {
         void ApplyState(TResetTarget target);
         void SetAll(TResetData state);
         void RestoreInitialState();
         void RestoreResetState();
         void RestoreMostRecentState();
         void CacheMostRecentState();
         void SaveResetState();

         TResetData InitialState { get; set; }
         TResetData ResetState { get; set; }
         TResetData MostRecentState { get; set; }
         TResetData CurrentState { get; set; }
    }
}