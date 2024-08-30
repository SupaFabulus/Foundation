using UnityEngine;

namespace SupaFabulus.Dev.Foundation.Common.DataModels.Ranges
{
    public class RangeInt : AbstractRange<int>
    {
        public RangeInt()
        {
            InitRange(0, 1, 0);
            Reset();
        }
        
        public RangeInt
        (
            int min = 0, 
            int max = 1, 
            int defaultValue = 0
        )
        {
            InitRange(min, max, defaultValue);
            Reset();
        }
        
        public override int Min
        {
            get => _min; 
            set {
                if (value >= _max)
                {
                    Flatten(value);
                    NotifyChange();
                }
            }
        }
        public override int Max
        {
            get => _max;
            set
            {
                if (value <= _min)
                {
                    Flatten(value);
                    NotifyChange();
                }
            }
        }
        public override int Value
        {
            get => _value;
            set
            {
                _value = Mathf.Clamp(value, _min, _max);
                NotifyChange();
            }
        }
        public override int Default
        {
            get => _default; 
            set => _default = Mathf.Clamp(value, _min, _max);
        }
    }
}