using System;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace SupaFabulus.Dev.Foundation.Components.ShadedTransform
{
    [Serializable]
#if UNITY_EDITOR
    [ExecuteInEditMode]
    #endif
    public class SharedTransformController : MonoBehaviour
    {
        [Header("Local Coordinate System")]
        [SerializeField]
        public Transform SharedTransform;
        
        [Space(8)][Header("Local Target")][Space(4)]
        [SerializeField]
        public Transform LocalTarget;
        [SerializeField]
        public Vector3 LocalPosition;
        [SerializeField]
        public Transform LocalRotationReference;
        
        [Space(8)][Header("Global Target")][Space(4)]
        [SerializeField]
        public Transform GlobalTarget;
        [SerializeField]
        public Vector3 GlobalPosition;
        [SerializeField]
        public Transform GlobalRotationReference;
        
        [Space(8)][Header("Motion")][Space(4)]
        [SerializeField]
        public float Speed;
        
        [Space(8)][Header("Display")][Space(4)]
        [SerializeField]
        public float CoordinateSystemIndicatorSize = 1f;

        
        
        public void Start()
        {
            AssignSharedTransform();
        }

        private void AssignSharedTransform()
        {
            if (SharedTransform != null)
            {
                SharedTransformUtility.SharedTransform = SharedTransform;
            }
        }

        private void LateUpdate()
        {
            AssignSharedTransform();
            if (SharedTransformUtility.SharedTransform == null) return;

            if (LocalTarget != null) UpdateLocalTarget();
            if (GlobalTarget != null) UpdateGlobalTarget();
        }

        protected void UpdateLocalTarget()
        {
            Vector3 pos;
            Vector3 localPos;
            Quaternion rot;
            Quaternion localRot;
            Quaternion refRot = (LocalRotationReference != null)
                ? LocalRotationReference.rotation
                : Quaternion.identity;
            
            Transform t = SharedTransformUtility.SharedTransform;

            if (t != null && LocalTarget != null)
            {
                //Debug.Log($"L");
                pos = LocalTarget.position;
                rot = LocalTarget.rotation;
                
                localPos = SharedTransformUtility.PositionLocalToGlobal(LocalPosition);
                localRot = Quaternion.LookRotation
                (
                    SharedTransformUtility.VectorLocalToGlobal(Vector3.forward),
                    t.up
                );

                LocalTarget.position = Vector3.Lerp
                (
                    pos,
                    localPos,
                    Time.deltaTime * Speed
                );
                
                LocalTarget.rotation = Quaternion.Slerp
                (
                    rot,
                    localRot * refRot,
                    Time.deltaTime * Speed
                );
            }
        }
        
        protected void UpdateGlobalTarget()
        {
            Vector3 pos;
            Vector3 globalPos;
            Quaternion rot;
            Quaternion globalRot;
            Quaternion refRot = (LocalRotationReference != null)
                ? GlobalRotationReference.rotation
                : Quaternion.identity;
            
            Transform t = SharedTransformUtility.SharedTransform;

            if (t != null && GlobalTarget != null)
            {
                //Debug.Log($"G");
                pos = GlobalTarget.localPosition;
                rot = GlobalTarget.localRotation;
                
                globalPos = SharedTransformUtility.PositionGlobalToLocal(GlobalPosition);
                
                globalRot = Quaternion.LookRotation
                (
                    SharedTransformUtility.VectorGlobalToLocal(GlobalRotationReference.forward),
                    SharedTransformUtility.VectorGlobalToLocal(GlobalRotationReference.up)
                );

                GlobalTarget.localPosition = Vector3.Lerp
                (
                    pos,
                    globalPos,
                    Time.deltaTime * Speed
                );
                
                GlobalTarget.localRotation = Quaternion.Slerp
                (
                    rot,
                    globalRot,
                    Time.deltaTime * Speed
                );
            }
        }

        private void OnDrawGizmos()
        {
            AssignSharedTransform();
            
            RenderSharedTransformTrident(0.25f, 0f, 1, CoordinateSystemIndicatorSize);
            
            DrawTrident(LocalRotationReference, 0.25f, 0f, 1f, 2f);
            DrawTrident(GlobalRotationReference, 0.25f, 0f, 1f, 2f);
        }

        private void OnDrawGizmosSelected()
        {
            AssignSharedTransform();
            
            RenderSharedTransformTrident(0.75f, 0.25f, 2, 2 * CoordinateSystemIndicatorSize);
            
            DrawTrident(LocalRotationReference, 0.5f, 0f, 3f, 3f);
            DrawTrident(GlobalRotationReference, 0.5f, 0f, 3f, 3f);
        }

        private void RenderSharedTransformTrident
        (
            float alpha = 1f, 
            float lightness = 0f,
            float thickness  = 1f,
            float length = 1f
        )
        {
            DrawTrident
            (
                SharedTransformUtility.SharedTransform,
                alpha,
                lightness,
                thickness,
                length
            );
        }

        private void DrawTrident
        (
            Transform targetTransform,
            float alpha = 1f,
            float lightness = 0f,
            float thickness = 1f,
            float length = 1f
        )
        {
            if (targetTransform == null)
            {
                return;
            }

            DrawTrident
            (
                targetTransform.position,
                targetTransform.rotation,
                alpha,
                lightness,
                thickness,
                length
            );
        }

        private void DrawTrident
        (
            Vector3 position,
            Quaternion rotation,
            float alpha = 1f, 
            float lightness = 0f,
            float thickness  = 1f,
            float length = 1f
        )
        {
            int i;
            int c;
            
            float a = alpha;
            float l = lightness;
            
            
            
            Color[] colors = new[]
            {
                new Color(1f, l, l, a),
                new Color(l, 1f, l, a),
                new Color(l, l, 1f, a)
            };

            Vector3[] dirs = new[]
            {
                (rotation * Vector3.right) * length,
                (rotation * Vector3.up) * length,
                (rotation * Vector3.forward) * length
            };

            if (colors.Length != dirs.Length)
            {
                return;
            }
            
            Vector3 start = Vector3.zero;
            Vector3 end = Vector3.zero;
            Quaternion rot = Quaternion.identity;
            
            start = position;

            c = dirs.Length;

            for (i = 0; i < c; i++)
            {
                end = start + dirs[i];
                #if UNITY_EDITOR
                Handles.color = colors[i];
                Handles.DrawLine(start, end, thickness);
                #endif
            }
        }
    }
}