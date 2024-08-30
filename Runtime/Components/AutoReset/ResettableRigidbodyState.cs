using System;
using SupaFabulus.Dev.Foundation.Common.DataModels;
using UnityEngine;

namespace SupaFabulus.Dev.Foundation.Components.AutoReset
{
    [Serializable]
    public class ResettableRigidbodyState :
        AbstractResettableState<RigidbodyState, Rigidbody>, IResettableState<RigidbodyState, Rigidbody>
    {
        public override void InitWith(Rigidbody initData, bool local = true)
        {
            SetAll(new RigidbodyState(initData));
        }
        
        public override void ApplyState(Rigidbody target)
        {
            target.position = _currentState.Position;
            target.rotation = _currentState.Rotation;
            target.isKinematic = _currentState.IsKinematic;
            target.useGravity = _currentState.UseGravity;
        }
    }
}