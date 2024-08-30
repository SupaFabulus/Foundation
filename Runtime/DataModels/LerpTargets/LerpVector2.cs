using UnityEngine;

namespace SupaFabulus.Dev.Foundation.Common.DataModels.LerpTargets
{

    public struct LerpVector2 : ILerpTarget<Vector2>
    {
        private const float _E = 0.000001f;
        
        private Vector2 _prevTgt;
        private Vector2 _tgt;
        private Vector2 _current;
        private Vector2 _default;
        private Vector2 _min;
        private Vector2 _max;
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
            Vector2 newTargetValue, 
            bool commitCurrentValue = false
        )
        {
            _prevTgt = commitCurrentValue
                ? _current
                : _tgt;
            _tgt = newTargetValue;
        }

        public Vector2 NewTarget { set => SetNewTarget(value); }
        public bool AtTargetValue => ((_current - _tgt).magnitude < _E);
        public bool ExceededTargetValue => (_current - _tgt).magnitude > 0f;
        public bool DidComplete => AtTargetValue || ExceededTargetValue;

        public Vector2 Remaining => _tgt - _current;
        public Vector2 Delta => _tgt - _prevTgt;
        public Vector2 Extents => _max - _min;
        
        public Vector2 Lerp(float t)
        { _current = Vector2.Lerp(_current, _tgt, t * _factor); return _current; }
        
        public Vector2 LerpFull(float t)
        { _current = Vector2.Lerp(_prevTgt, _tgt, t * _factor); return _current; }
        
        public bool CheckLerp(float t)
        { _current = Vector2.Lerp(_current, _tgt, t * _factor); return DidComplete; }
        
        public bool CheckLerpFull(float t)
        { _current = Vector2.Lerp(_prevTgt, _tgt, t * _factor); return DidComplete; }

        public Vector2 LerpValue(float t) => 
            Vector2.Lerp(_current, _tgt, t * _factor);

        public Vector2 LerpValueFull(float t) => 
            Vector2.Lerp(_prevTgt, _tgt, t * _factor);
        
        
        
        
        public Vector2 LerpFree(float t)
        { _current = Vector2.LerpUnclamped(_current, _tgt, t * _factor); return _current; }
        
        public Vector2 LerpFullFree(float t)
        { _current = Vector2.LerpUnclamped(_prevTgt, _tgt, t * _factor); return _current; }
        
        public bool CheckLerpFree(float t)
        { _current = Vector2.LerpUnclamped(_current, _tgt, t * _factor); return DidComplete; }
        
        public bool CheckLerpFullFree(float t)
        { _current = Vector2.LerpUnclamped(_prevTgt, _tgt, t * _factor); return DidComplete; }

        public Vector2 LerpValueFree(float t) => 
            Vector2.LerpUnclamped(_current, _tgt, t * _factor);

        public Vector2 LerpValueFullFree(float t) => 
            Vector2.LerpUnclamped(_prevTgt, _tgt, t * _factor);
        


        public Vector2 Previous
        {
            get => _prevTgt;
            set => _prevTgt = value;
        }

        public Vector2 Target
        {
            get => _tgt;
            set => _tgt = value;
        }

        public Vector2 Current
        {
            get => _current;
            set => _current = value;
        }

        public Vector2 Default
        {
            get => _default;
            set => _default = value;
        }

        public Vector2 Min
        {
            get => _min;
            set => _min = value;
        }

        public Vector2 Max
        {
            get => _max;
            set => _max = value;
        }
        
        public float Factor
        {
            get => _factor;
            set => _factor = value;
        }

        public LerpVector2
        (
            Vector2 defaultValue,
            Vector2 minValue,
            Vector2 maxValue,
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

        public LerpVector2
        (
            Vector2 currentValue,
            Vector2 defaultValue,
            Vector2 minValue,
            Vector2 maxValue,
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

        public LerpVector2
        (
            Vector2 currentValue,
            Vector2 initialTargetValue,
            Vector2 defaultValue,
            Vector2 minValue,
            Vector2 maxValue,
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

        public LerpVector2
        (
            Vector2 currentValue,
            Vector2 initialTargetValue,
            Vector2 injectedPreviousTargetValue,
            Vector2 defaultValue,
            Vector2 minValue,
            Vector2 maxValue,
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