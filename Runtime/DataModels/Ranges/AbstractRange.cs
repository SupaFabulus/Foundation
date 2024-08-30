using System;
using UnityEngine;

namespace SupaFabulus.Dev.Foundation.Common.DataModels.Ranges
{
    [Serializable]
    public abstract class AbstractRange<TNumber> : IRange<TNumber>
    where TNumber : struct
    {
        [SerializeField][HideInInspector]
        protected bool _initialized = false;
        [SerializeField]
        protected TNumber _default;
        [SerializeField]
        protected TNumber _value;
        [SerializeField]
        protected TNumber _min;
        [SerializeField]
        protected TNumber _max;

        public bool Initialized => _initialized;
        public abstract TNumber Min { get; set; }
        public abstract TNumber Max { get; set; }
        public abstract TNumber Value { get; set; }
        public abstract TNumber Default { get; set; }
        public virtual void SetValue(TNumber value) { Value = value; }
        
        

        protected Action<IRange<TNumber>> _onChange;
        public Action<IRange<TNumber>> OnChange => _onChange;

        public AbstractRange()
        {
            InitRange();
        }

        public AbstractRange
        (
            TNumber min = default,
            TNumber max = default,
            TNumber defaultValue = default
        )
        {
            InitRange(min, max, defaultValue);
        }

        protected virtual void InitRange
        (
            TNumber min = default,
            TNumber max = default,
            TNumber defaultValue = default
        )
        {
            if (!_initialized)
            {
                
                _min = min;
                Max = max;
                Default = defaultValue;
                Reset();
                _initialized = true;
            }
        }

        protected virtual void DeInitRange()
        {
            _initialized = false;
        }

        protected virtual void NotifyChange()
        {
            if (_initialized && _onChange != null)
            {
                _onChange(this);
            }
        }

        protected void Flatten(TNumber n)
        {
            _min = _max = _default = _value = n;
        }

        public virtual void Destroy()
        {
            DeInitRange();
        }

        public virtual void SetToMax()
        {
            _value = _max;
        }

        public virtual void SetToMin()
        {
            _value = _min;
        }

        public virtual void Reset()
        {
            _value = _default;
        }
    }
}