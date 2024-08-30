using UnityEngine;

namespace SupaFabulus.Dev.Foundation.Core.Interfaces
{
    public interface IRigidbody : IGameObject
    {
        
        Rigidbody Rigidbody { get; }
        /*
        Vector3 Position { get; set; }
        Quaternion Rotation { get; set; }
        Vector3 Velocity { get; set; }
        Vector3 AngularVelocity { get; set; }
        float Mass { get; set; }
        float Drag { get; set; }
        float AngularDrag { get; set; }
        bool IsKinematic { get; set; }
        bool UseGravity { get; set; }

        Vector3 Left { get; }
        Vector3 Right { get; }
        Vector3 Down { get; }
        Vector3 Up { get; }
        Vector3 Back { get; }
        Vector3 Forward { get; }
        */
    }
}