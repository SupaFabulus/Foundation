namespace SupaFabulus.Dev.Foundation.Common.DataModels.LerpTargets
{
    public interface ILerpTarget<TValue>
    {
        TValue Lerp(float t);
        TValue LerpFull(float t);
        
        bool CheckLerp(float t);
        bool CheckLerpFull(float t);
        
        TValue LerpValue(float t);
        TValue LerpValueFull(float t);
        
        
        TValue LerpFree(float t);
        TValue LerpFullFree(float t);
        
        bool CheckLerpFree(float t);
        bool CheckLerpFullFree(float t);
        
        TValue LerpValueFree(float t);
        TValue LerpValueFullFree(float t);

        void SwapCurrentAndTarget();
        void SwapPreviousAndTarget();
        void Reset();
        void SetNewTarget(TValue newTargetValue, bool commitCurrentValue = false);

        TValue NewTarget { set; }
        
        TValue Previous { get; set; }
        TValue Target { get; set; }
        TValue Current { get; set; }
        TValue Default { get; set; }
        
        TValue Remaining { get; }
        TValue Delta { get; }

        float Factor { get; set; }

        bool AtTargetValue { get; }
        bool ExceededTargetValue { get; }
        bool DidComplete { get; }
    }
}