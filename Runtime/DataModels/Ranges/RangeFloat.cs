

using UnityEngine;

namespace SupaFabulus.Dev.Foundation.Common.DataModels.Ranges
{
    public class RangeFloat : AbstractRange<float>
    {
        
        public RangeFloat()
        {
            InitRange(0f, 1f, 0f);
            Reset();
        }
        
        public RangeFloat
        (
            float min = 0f, 
            float max = 1f, 
            float defaultValue = 0f
        )
        {
            InitRange(min, max, defaultValue);
            Reset();
        }

        public override float Min
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
        public override float Max
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
        public override float Value
        {
            get => _value;
            set
            {
                _value = Mathf.Clamp(value, _min, _max);
                NotifyChange();
            }
        }
        public override float Default
        {
            get => _default; 
            set => _default = Mathf.Clamp(value, _min, _max);
        }
    }
}