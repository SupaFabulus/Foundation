using System;

namespace SupaFabulus.Dev.Foundation.Common.DataModels.Ranges

{
    public interface IRange<TNumber>
    where TNumber : struct
    {
        bool Initialized { get; }
        TNumber Min { get; set; }
        TNumber Max { get; set; }
        TNumber Value { get; set; }
        TNumber Default { get; set; }
        
        Action<IRange<TNumber>> OnChange { get; }

        void SetValue(TNumber value);
        void SetToMax();
        void SetToMin();
        void Reset();
        void Destroy();
    }
}