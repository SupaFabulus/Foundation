using UnityEngine;

namespace SupaFabulus.Dev.Foundation.Common.DataModels.LerpTargets
{

    public struct LerpFloat : ILerpTarget<float>
    {
        private const float _E = 0.000001f;
        
        private float _prevTgt;
        private float _tgt;
        private float _current;
        private float _default;
        private float _min;
        private float _max;
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
            float newTargetValue, 
            bool commitCurrentValue = false
        )
        {
            _prevTgt = commitCurrentValue
                ? _current
                : _tgt;
            _tgt = newTargetValue;
        }

        public float NewTarget { set => SetNewTarget(value); }
        public bool AtTargetValue => ((_current - _tgt) < _E);
        public bool ExceededTargetValue => (_current - _tgt) > 0f;
        public bool DidComplete => AtTargetValue || ExceededTargetValue;

        public float Remaining => _tgt - _current;
        public float Delta => _tgt - _prevTgt;
        public float Extents => _max - _min;
        
        public float Lerp(float t)
        { _current = Mathf.Lerp(_current, _tgt, t * _factor); return _current; }
        
        public float LerpFull(float t)
        { _current = Mathf.Lerp(_prevTgt, _tgt, t * _factor); return _current; }
        
        public bool CheckLerp(float t)
        { _current = Mathf.Lerp(_current, _tgt, t * _factor); return DidComplete; }
        
        public bool CheckLerpFull(float t)
        { _current = Mathf.Lerp(_prevTgt, _tgt, t * _factor); return DidComplete; }

        public float LerpValue(float t) => 
            Mathf.Lerp(_current, _tgt, t * _factor);

        public float LerpValueFull(float t) => 
            Mathf.Lerp(_prevTgt, _tgt, t * _factor);
        


        
        
        public float LerpFree(float t)
        { _current = Mathf.LerpUnclamped(_current, _tgt, t * _factor); return _current; }
        
        public float LerpFullFree(float t)
        { _current = Mathf.LerpUnclamped(_prevTgt, _tgt, t * _factor); return _current; }
        
        public bool CheckLerpFree(float t)
        { _current = Mathf.LerpUnclamped(_current, _tgt, t * _factor); return DidComplete; }
        
        public bool CheckLerpFullFree(float t)
        { _current = Mathf.LerpUnclamped(_prevTgt, _tgt, t * _factor); return DidComplete; }

        public float LerpValueFree(float t) => 
            Mathf.LerpUnclamped(_current, _tgt, t * _factor);

        public float LerpValueFullFree(float t) => 
            Mathf.LerpUnclamped(_prevTgt, _tgt, t * _factor);
        
        

        public float Previous
        {
            get => _prevTgt;
            set => _prevTgt = value;
        }

        public float Target
        {
            get => _tgt;
            set => _tgt = value;
        }

        public float Current
        {
            get => _current;
            set => _current = value;
        }

        public float Default
        {
            get => _default;
            set => _default = value;
        }

        public float Min
        {
            get => _min;
            set => _min = value;
        }

        public float Max
        {
            get => _max;
            set => _max = value;
        }
        
        public float Factor
        {
            get => _factor;
            set => _factor = value;
        }

        public LerpFloat
        (
            float defaultValue,
            float minValue,
            float maxValue,
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

        public LerpFloat
        (
            float currentValue,
            float defaultValue,
            float minValue,
            float maxValue,
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

        public LerpFloat
        (
            float currentValue,
            float initialTargetValue,
            float defaultValue,
            float minValue,
            float maxValue,
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

        public LerpFloat
        (
            float currentValue,
            float initialTargetValue,
            float injectedPreviousTargetValue,
            float defaultValue,
            float minValue,
            float maxValue,
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