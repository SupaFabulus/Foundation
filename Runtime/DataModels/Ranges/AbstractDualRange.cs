using System;
using UnityEngine;

namespace SupaFabulus.Dev.Foundation.Common.DataModels.Ranges
{
    [Serializable]
    public abstract class AbstractDualRange<TNumber, TRange>
    where TNumber : struct
    where TRange : IRange<TNumber>, new()
    {
        [SerializeField][HideInInspector]
        protected bool _initialized = false;
        
        [SerializeField]
        protected TRange _rangeA;
        [SerializeField]
        protected TRange _rangeB;

        public bool Initialized => _initialized;
        
        public TRange RangeA => _rangeA;
        public TRange RangeB => _rangeB;

        public AbstractDualRange
        (
            TNumber min,
            TNumber max,
            TNumber defaultValue
        )
        {
            _rangeA = new TRange();
            _rangeB = new TRange();
            SetupRange(_rangeA, min, max, defaultValue);
            SetupRange(_rangeB, min, max, defaultValue);
        }
        
        public AbstractDualRange()
        {
            _rangeA = new TRange();
            _rangeB = new TRange();
            _initialized = false;
        }

        public AbstractDualRange
        (
            TNumber minA,
            TNumber maxA,
            TNumber defaultValueA,
            TNumber minB,
            TNumber maxB,
            TNumber defaultValueB
        )
        {
            _rangeA = new TRange();
            _rangeB = new TRange();
            SetupRange(_rangeA, minA, maxA, defaultValueA);
            SetupRange(_rangeB, minB, maxB, defaultValueB);
            _initialized = true;
        }
        
        protected void SetupRange
        (
            in TRange r,
            TNumber min = default,
            TNumber max = default,
            TNumber defaultValue = default
        )
        {
            r.Min = min;
            r.Max = max;
            r.Default = defaultValue;
            r.Reset();
            _initialized = true;
        }


        public virtual void Destroy()
        {
            _rangeA?.Destroy();
            _rangeB?.Destroy();

            _rangeA = default;
            _rangeB = default;

            _initialized = false;
        }
    }
}