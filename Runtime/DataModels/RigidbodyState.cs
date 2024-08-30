using System;
using SupaFabulus.Dev.Foundation.Core.Interfaces;
using UnityEngine;

namespace SupaFabulus.Dev.Foundation.Common.DataModels
{
    
    [Serializable]
    public struct RigidbodyState : IInitWith<Rigidbody>
    {
        [SerializeField]
        public Vector3 Position;
        [SerializeField]
        public Quaternion Rotation;
        [SerializeField]
        public Vector3 Velocity;
        [SerializeField]
        public Vector3 AngularVelocity;
        [SerializeField]
        public float Drag;
        [SerializeField]
        public float AngularDrag;
        [SerializeField]
        public float Mass;
        [SerializeField]
        public bool UseGravity;
        [SerializeField]
        public bool IsKinematic;
        [SerializeField]
        public RigidbodyConstraints Constraints;
        [SerializeField]
        public RigidbodyInterpolation Interpolation;
        [SerializeField]
        public bool FreezeRotation;
        [SerializeField]
        public bool DetectCollisions;
        [SerializeField]
        public float MaxAngularVelocity;
        [SerializeField]
        public float MaxDepenetrationVelocity;
    
        // Constructor to initialize the struct with a Rigidbody's values
        public RigidbodyState(Rigidbody rigidbody)
        {
            Position = rigidbody.position;
            Rotation = rigidbody.rotation;
            Velocity = rigidbody.linearVelocity;
            AngularVelocity = rigidbody.angularVelocity;
            Drag = rigidbody.linearDamping;
            AngularDrag = rigidbody.angularDamping;
            Mass = rigidbody.mass;
            UseGravity = rigidbody.useGravity;
            IsKinematic = rigidbody.isKinematic;
            Constraints = rigidbody.constraints;
            Interpolation = rigidbody.interpolation;
            FreezeRotation = rigidbody.freezeRotation;
            DetectCollisions = rigidbody.detectCollisions;
            MaxAngularVelocity = rigidbody.maxAngularVelocity;
            MaxDepenetrationVelocity = rigidbody.maxDepenetrationVelocity;
        }
    
        // Method to apply Rigidbody's public properties to the RigidbodyState struct
        public void Apply(Rigidbody rigidbody)
        {
            if (rigidbody == null) return;
            
            rigidbody.linearDamping = Drag;
            rigidbody.angularDamping = AngularDrag;
            rigidbody.maxAngularVelocity = MaxAngularVelocity;
            
            rigidbody.mass = Mass;
            rigidbody.useGravity = UseGravity;
            
            rigidbody.constraints = Constraints;
            rigidbody.freezeRotation = FreezeRotation;
            
            rigidbody.interpolation = Interpolation;
            rigidbody.detectCollisions = DetectCollisions;
            rigidbody.maxDepenetrationVelocity = MaxDepenetrationVelocity;
            
            rigidbody.isKinematic = IsKinematic;
            if (!IsKinematic)
            {
                rigidbody.linearVelocity = Velocity;
                rigidbody.angularVelocity = AngularVelocity;
            }
            
            rigidbody.position = Position;
            rigidbody.rotation = Rotation;
        }
        
        public void InitWith(Rigidbody rigidbody, bool local = true)
        {
            Position = rigidbody.position;
            Rotation = rigidbody.rotation;
            Velocity = rigidbody.linearVelocity;
            AngularVelocity = rigidbody.angularVelocity;
            Drag = rigidbody.linearDamping;
            AngularDrag = rigidbody.angularDamping;
            Mass = rigidbody.mass;
            UseGravity = rigidbody.useGravity;
            IsKinematic = rigidbody.isKinematic;
            Constraints = rigidbody.constraints;
            Interpolation = rigidbody.interpolation;
            FreezeRotation = rigidbody.freezeRotation;
            DetectCollisions = rigidbody.detectCollisions;
            MaxAngularVelocity = rigidbody.maxAngularVelocity;
            MaxDepenetrationVelocity = rigidbody.maxDepenetrationVelocity;
        }
    }
}