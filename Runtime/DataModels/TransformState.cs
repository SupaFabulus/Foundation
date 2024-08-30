using System;
using SupaFabulus.Dev.Foundation.Core.Interfaces;
using UnityEngine;

namespace SupaFabulus.Dev.Foundation.Common.DataModels
{
    [Serializable]
    public struct TransformState : IInitWith<Transform>
    {
        [SerializeField]
        public bool IsLocal;
        [SerializeField]
        public Vector3 Position;
        [SerializeField]
        public Quaternion Rotation;
        [SerializeField]
        public Vector3 Scale;
        [SerializeField]
        public Vector3 LocalPosition;
        [SerializeField]
        public Quaternion LocalRotation;

        public TransformState(Transform src, bool local = true)
        {
            Position = src.position;
            Rotation = src.rotation;
            Scale = src.localScale;
            LocalPosition = src.localPosition;
            LocalRotation = src.localRotation;
            IsLocal = local;
        }
        
        public TransformState(Vector3 pos, bool local = true)
        {
            Position = pos;
            Rotation = Quaternion.identity;
            Scale = Vector3.one;
            LocalPosition = Vector3.zero;
            LocalRotation = Quaternion.identity;
            IsLocal = local;
        }
        
        public TransformState(Vector3 pos, Quaternion rot, bool local = true)
        {
            Position = pos;
            Rotation = rot;
            Scale = Vector3.one;
            LocalPosition = Vector3.zero;
            LocalRotation = Quaternion.identity;
            IsLocal = local;
        }

        public void Apply(Transform tgt)
        {
            if (tgt == null) return;
            if (IsLocal)
            {
                tgt.localPosition = LocalPosition;
                tgt.localRotation = LocalRotation;
            }
            else
            {
                tgt.position = Position;
                tgt.rotation = Rotation;
            }

            tgt.localScale = Scale;
        }

        public void InitWith(Transform initData, bool local = true)
        {
            Position = initData.position;
            Rotation = initData.rotation;
            Scale = initData.localScale;
            LocalPosition = initData.localPosition;
            LocalRotation = initData.localRotation;
            IsLocal = local;
        }
    }
}