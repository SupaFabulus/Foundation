using UnityEngine;

namespace SupaFabulus.Dev.Foundation.Common.DataModels.LerpTargets
{

    public struct LerpQuaternion : ILerpTarget<Quaternion>
    {
        private const float _E = 0.000001f;
        
        private Quaternion _prevTgt;
        private Quaternion _tgt;
        private Quaternion _current;
        private Quaternion _default;
        private Quaternion _min;
        private Quaternion _max;
        private float _factor;

        public void SwapCurrentAndTarget() => 
            (_current, _tgt) = (_tgt, _current);
        
        public void SwapPreviousAndTarget() =>
            (_prevTgt, _tgt) = (_tgt, _prevTgt);

        public void Reset()
        {
            _current = _default;
            _prevTgt = _default;
            _tgt = _default;
            _factor = 1f;
        }

        public void SetNewTarget
        (
            Quaternion newTargetValue, 
            bool commitCurrentValue = false
        )
        {
            _prevTgt = commitCurrentValue
                ? _current
                : _tgt;
            _tgt = newTargetValue;
        }

        public Quaternion NewTarget { set => SetNewTarget(value); }
        public bool AtTargetValue => false; // Need to design implementation
        public bool ExceededTargetValue => false; // Need to design implementation
        public bool DidComplete => AtTargetValue || ExceededTargetValue;

        public Quaternion Remaining => default; // Need to design implementation
        public Quaternion Delta => default; // Need to design implementation
        public Quaternion Extents => default; // Need to design implementation
        
        public Quaternion Lerp(float t)
        { _current = Quaternion.Lerp(_current, _tgt, t * _factor); return _current; }
        
        public Quaternion LerpFull(float t)
        { _current = Quaternion.Lerp(_prevTgt, _tgt, t * _factor); return _current; }
        
        public bool CheckLerp(float t)
        { _current = Quaternion.Lerp(_current, _tgt, t * _factor); return DidComplete; }
        
        public bool CheckLerpFull(float t)
        { _current = Quaternion.Lerp(_prevTgt, _tgt, t * _factor); return DidComplete; }

        public Quaternion LerpValue(float t) => 
            Quaternion.Lerp(_current, _tgt, t * _factor);

        public Quaternion LerpValueFull(float t) => 
            Quaternion.Lerp(_prevTgt, _tgt, t * _factor);
        
        
        
        public Quaternion LerpFree(float t)
        { _current = Quaternion.LerpUnclamped(_current, _tgt, t * _factor); return _current; }
        
        public Quaternion LerpFullFree(float t)
        { _current = Quaternion.LerpUnclamped(_prevTgt, _tgt, t * _factor); return _current; }
        
        public bool CheckLerpFree(float t)
        { _current = Quaternion.LerpUnclamped(_current, _tgt, t * _factor); return DidComplete; }
        
        public bool CheckLerpFullFree(float t)
        { _current = Quaternion.LerpUnclamped(_prevTgt, _tgt, t * _factor); return DidComplete; }

        public Quaternion LerpValueFree(float t) => 
            Quaternion.LerpUnclamped(_current, _tgt, t * _factor);

        public Quaternion LerpValueFullFree(float t) => 
            Quaternion.LerpUnclamped(_prevTgt, _tgt, t * _factor);
        
        
        
        
        
        
        public Quaternion Slerp(float t)
        { _current = Quaternion.Slerp(_current, _tgt, t * _factor); return _current; }
        
        public Quaternion SlerpFull(float t)
        { _current = Quaternion.Slerp(_prevTgt, _tgt, t * _factor); return _current; }
        
        public bool CheckSlerp(float t)
        { _current = Quaternion.Slerp(_current, _tgt, t * _factor); return DidComplete; }
        
        public bool CheckSlerpFull(float t)
        { _current = Quaternion.Slerp(_prevTgt, _tgt, t * _factor); return DidComplete; }

        public Quaternion SlerpValue(float t) => 
            Quaternion.Slerp(_current, _tgt, t * _factor);

        public Quaternion SlerpValueFull(float t) => 
            Quaternion.Slerp(_prevTgt, _tgt, t * _factor);
        
        
        
        public Quaternion SlerpFree(float t)
        { _current = Quaternion.SlerpUnclamped(_current, _tgt, t * _factor); return _current; }
        
        public Quaternion SlerpFullFree(float t)
        { _current = Quaternion.SlerpUnclamped(_prevTgt, _tgt, t * _factor); return _current; }
        
        public bool CheckSlerpFree(float t)
        { _current = Quaternion.SlerpUnclamped(_current, _tgt, t * _factor); return DidComplete; }
        
        public bool CheckSlerpFullFree(float t)
        { _current = Quaternion.SlerpUnclamped(_prevTgt, _tgt, t * _factor); return DidComplete; }

        public Quaternion SlerpValueFree(float t) => 
            Quaternion.SlerpUnclamped(_current, _tgt, t * _factor);

        public Quaternion SlerpValueFullFree(float t) => 
            Quaternion.SlerpUnclamped(_prevTgt, _tgt, t * _factor);
        
        
        
        


        public Quaternion Previous
        {
            get => _prevTgt;
            set => _prevTgt = value;
        }

        public Quaternion Target
        {
            get => _tgt;
            set => _tgt = value;
        }

        public Quaternion Current
        {
            get => _current;
            set => _current = value;
        }

        public Quaternion Default
        {
            get => _default;
            set => _default = value;
        }

        public Quaternion Min
        {
            get => _min;
            set => _min = value;
        }

        public Quaternion Max
        {
            get => _max;
            set => _max = value;
        }
        
        public float Factor
        {
            get => _factor;
            set => _factor = value;
        }

        public LerpQuaternion
        (
            Quaternion defaultValue,
            Quaternion minValue,
            Quaternion maxValue,
            float factor = 1f
        )
        {
            _default = defaultValue;
            _min = minValue;
            _max = maxValue;
            _current = _default;
            _prevTgt = _default;
            _tgt = default;
            _factor = factor;
        }

        public LerpQuaternion
        (
            Quaternion currentValue,
            Quaternion defaultValue,
            Quaternion minValue,
            Quaternion maxValue,
            float factor = 1f
        )
        {
            _default = defaultValue;
            _min = minValue;
            _max = maxValue;
            _current = currentValue;
            _prevTgt = currentValue;
            _tgt = default;
            _factor = factor;
        }

        public LerpQuaternion
        (
            Quaternion currentValue,
            Quaternion initialTargetValue,
            Quaternion defaultValue,
            Quaternion minValue,
            Quaternion maxValue,
            float factor = 1f
        )
        {
            _default = defaultValue;
            _min = minValue;
            _max = maxValue;
            _current = currentValue;
            _prevTgt = currentValue;
            _tgt = initialTargetValue;
            _factor = factor;
        }

        public LerpQuaternion
        (
            Quaternion currentValue,
            Quaternion initialTargetValue,
            Quaternion injectedPreviousTargetValue,
            Quaternion defaultValue,
            Quaternion minValue,
            Quaternion maxValue,
            float factor = 1f
        )
        {
            _default = defaultValue;
            _min = minValue;
            _max = maxValue;
            _current = currentValue;
            _prevTgt = injectedPreviousTargetValue;
            _tgt = initialTargetValue;
            _factor = factor;
        }

    }
}