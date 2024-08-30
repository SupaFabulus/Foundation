using UnityEngine;

namespace SupaFabulus.Dev.Foundation.Common.DataModels.LerpTargets
{

    public struct LerpColor : ILerpTarget<Color>
    {
        private const float _E = 0.000001f;
        
        private Color _prevTgt;
        private Color _tgt;
        private Color _current;
        private Color _default;
        private Color _min;
        private Color _max;
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
            Color newTargetValue, 
            bool commitCurrentValue = false
        )
        {
            _prevTgt = commitCurrentValue
                ? _current
                : _tgt;
            _tgt = newTargetValue;
        }

        public Color NewTarget { set => SetNewTarget(value); }
        public bool AtTargetValue => false; // Need to design implementation
        public bool ExceededTargetValue => false; // Need to design implementation
        public bool DidComplete => AtTargetValue || ExceededTargetValue;

        public Color Remaining => _tgt - _current;
        public Color Delta => _tgt - _prevTgt;
        public Color Extents => _max - _min;
        
        public Color Lerp(float t)
        { _current = Color.Lerp(_current, _tgt, t * _factor); return _current; }
        
        public Color LerpFull(float t)
        { _current = Color.Lerp(_prevTgt, _tgt, t * _factor); return _current; }
        
        public bool CheckLerp(float t)
        { _current = Color.Lerp(_current, _tgt, t * _factor); return DidComplete; }
        
        public bool CheckLerpFull(float t)
        { _current = Color.Lerp(_prevTgt, _tgt, t * _factor); return DidComplete; }

        public Color LerpValue(float t) => 
            Color.Lerp(_current, _tgt, t * _factor);

        public Color LerpValueFull(float t) => 
            Color.Lerp(_prevTgt, _tgt, t * _factor);

        public Color LerpFree(float t)
        { _current = Color.LerpUnclamped(_current, _tgt, t * _factor); return _current; }
        
        public Color LerpFullFree(float t)
        { _current = Color.LerpUnclamped(_prevTgt, _tgt, t * _factor); return _current; }
        
        public bool CheckLerpFree(float t)
        { _current = Color.LerpUnclamped(_current, _tgt, t * _factor); return DidComplete; }
        
        public bool CheckLerpFullFree(float t)
        { _current = Color.LerpUnclamped(_prevTgt, _tgt, t * _factor); return DidComplete; }

        public Color LerpValueFree(float t) => 
            Color.LerpUnclamped(_current, _tgt, t * _factor);

        public Color LerpValueFullFree(float t) => 
            Color.LerpUnclamped(_prevTgt, _tgt, t * _factor);
        
        

        public Color Previous
        {
            get => _prevTgt;
            set => _prevTgt = value;
        }

        public Color Target
        {
            get => _tgt;
            set => _tgt = value;
        }

        public Color Current
        {
            get => _current;
            set => _current = value;
        }

        public Color Default
        {
            get => _default;
            set => _default = value;
        }

        public Color Min
        {
            get => _min;
            set => _min = value;
        }

        public Color Max
        {
            get => _max;
            set => _max = value;
        }
        
        public float Factor
        {
            get => _factor;
            set => _factor = value;
        }

        public LerpColor
        (
            Color defaultValue,
            Color minValue,
            Color maxValue,
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

        public LerpColor
        (
            Color currentValue,
            Color defaultValue,
            Color minValue,
            Color maxValue,
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

        public LerpColor
        (
            Color currentValue,
            Color initialTargetValue,
            Color defaultValue,
            Color minValue,
            Color maxValue,
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

        public LerpColor
        (
            Color currentValue,
            Color initialTargetValue,
            Color injectedPreviousTargetValue,
            Color defaultValue,
            Color minValue,
            Color maxValue,
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