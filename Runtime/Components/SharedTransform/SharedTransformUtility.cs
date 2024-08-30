using UnityEngine;

namespace SupaFabulus.Dev.Foundation.Components.ShadedTransform
{
    public static class SharedTransformUtility
    {
        public static Transform SharedTransform;

        public static Vector3 PositionLocalToGlobal(Vector3 position)
        {
            if (SharedTransform == null)
            {
                return Vector3.zero;
            }

            return SharedTransform.TransformPoint(position);
        }

        public static Vector3 DirectionLocalToGlobal(Vector3 position)
        {
            if (SharedTransform == null)
            {
                return Vector3.zero;
            }

            return SharedTransform.TransformDirection(position);
        }

        public static Vector3 VectorLocalToGlobal(Vector3 position)
        {
            if (SharedTransform == null)
            {
                return Vector3.zero;
            }

            return SharedTransform.TransformVector(position);
        }

        public static Vector3 PositionGlobalToLocal(Vector3 position)
        {
            if (SharedTransform == null)
            {
                return Vector3.zero;
            }

            return SharedTransform.InverseTransformPoint(position);
        }

        public static Vector3 DirectionGlobalToLocal(Vector3 position)
        {
            if (SharedTransform == null)
            {
                return Vector3.zero;
            }

            return SharedTransform.InverseTransformDirection(position);
        }

        public static Vector3 VectorGlobalToLocal(Vector3 position)
        {
            if (SharedTransform == null)
            {
                return Vector3.zero;
            }

            return SharedTransform.InverseTransformVector(position);
        }
    }
}

