using System;
using SupaFabulus.Dev.Foundation.Common.DataModels;
using UnityEngine;

namespace SupaFabulus.Dev.Foundation.Components.AutoReset
{
    [Serializable]
    public class ResettableTransformState : 
        AbstractResettableState<TransformState, Transform>, IResettableState<TransformState, Transform>
    {
        public override void InitWith(Transform initData, bool local = true)
        {
            SetAll(new TransformState(initData));
        }

        public override void ApplyState(Transform target)
        {
            target.position = _currentState.Position;
            target.rotation = _currentState.Rotation;
            target.localScale = _currentState.Scale;
        }
    }
}