using UnityEngine;

namespace SupaFabulus.Dev.Foundation.Common.DataModels.LerpTargets
{

    public struct LerpVector3 : ILerpTarget<Vector3>
    {
        private const float _E = 0.000001f;
        
        private Vector3 _prevTgt;
        private Vector3 _tgt;
        private Vector3 _current;
        private Vector3 _default;
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
            Vector3 newTargetValue, 
            bool commitCurrentValue = false
        )
        {
            _prevTgt = commitCurrentValue
                ? _current
                : _tgt;
            _tgt = newTargetValue;
        }

        public Vector3 NewTarget { set => SetNewTarget(value, true); }
        public bool AtTargetValue => ((_current - _tgt).magnitude < _E);
        public bool ExceededTargetValue => (_current - _tgt).magnitude > 0f;
        public bool DidComplete => AtTargetValue || ExceededTargetValue;

        public Vector3 Remaining => _tgt - _current;
        public Vector3 Delta => _tgt - _prevTgt;

        public Vector3 Lerp(float t)
        { _current = Vector3.Lerp(_current, _tgt, t * _factor); return _current; }
        
        public Vector3 LerpFull(float t)
        { _current = Vector3.Lerp(_prevTgt, _tgt, t * _factor); return _current; }
        
        public bool CheckLerp(float t)
        { _current = Vector3.Lerp(_current, _tgt, t * _factor); return DidComplete; }
        
        public bool CheckLerpFull(float t)
        { _current = Vector3.Lerp(_prevTgt, _tgt, t * _factor); return DidComplete; }

        public Vector3 LerpValue(float t) => 
            Vector3.Lerp(_current, _tgt, t * _factor);

        public Vector3 LerpValueFull(float t) => 
            Vector3.Lerp(_prevTgt, _tgt, t * _factor);
        
        
        
        
        public Vector3 LerpFree(float t)
        { _current = Vector3.LerpUnclamped(_current, _tgt, t * _factor); return _current; }
        
        public Vector3 LerpFullFree(float t)
        { _current = Vector3.LerpUnclamped(_prevTgt, _tgt, t * _factor); return _current; }
        
        public bool CheckLerpFree(float t)
        { _current = Vector3.LerpUnclamped(_current, _tgt, t * _factor); return DidComplete; }
        
        public bool CheckLerpFullFree(float t)
        { _current = Vector3.LerpUnclamped(_prevTgt, _tgt, t * _factor); return DidComplete; }

        public Vector3 LerpValueFree(float t) => 
            Vector3.LerpUnclamped(_current, _tgt, t * _factor);

        public Vector3 LerpValueFullFree(float t) => 
            Vector3.LerpUnclamped(_prevTgt, _tgt, t * _factor);
        


        public Vector3 Previous
        {
            get => _prevTgt;
            set => _prevTgt = value;
        }

        public Vector3 Target
        {
            get => _tgt;
            set => _tgt = value;
        }

        public Vector3 Current
        {
            get => _current;
            set => _current = value;
        }

        public Vector3 Default
        {
            get => _default;
            set => _default = value;
        }
        
        public float Factor
        {
            get => _factor;
            set => _factor = value;
        }

        public LerpVector3
        (
            Vector3 defaultValue,
            float factor = 1f
        )
        {
            _default = defaultValue;
            _current = _default;
            _prevTgt = _default;
            _tgt = default;
            _factor = factor;
        }

        public LerpVector3
        (
            Vector3 currentValue,
            Vector3 defaultValue,
            float factor = 1f
        )
        {
            _default = defaultValue;
            _current = currentValue;
            _prevTgt = currentValue;
            _tgt = default;
            _factor = factor;
        }

        public LerpVector3
        (
            Vector3 currentValue,
            Vector3 initialTargetValue,
            Vector3 defaultValue,
            float factor = 1f
        )
        {
            _default = defaultValue;
            _current = currentValue;
            _prevTgt = currentValue;
            _tgt = initialTargetValue;
            _factor = factor;
        }

        public LerpVector3
        (
            Vector3 currentValue,
            Vector3 initialTargetValue,
            Vector3 injectedPreviousTargetValue,
            Vector3 defaultValue,
            float factor = 1f
        )
        {
            _default = defaultValue;
            _current = currentValue;
            _prevTgt = injectedPreviousTargetValue;
            _tgt = initialTargetValue;
            _factor = factor;
        }

    }
}