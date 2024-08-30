using UnityEngine.Device;

namespace SupaFabulus.Dev.Foundation.Utils
{
    public static class Safe
    {
        public static void Destroy(UnityEngine.Object target)
        {
            if(target == null) return;
            #if UNITY_EDITOR
            if (Application.isPlaying)
            {
                UnityEngine.Object.Destroy(target);
            }
            else
            {
                UnityEngine.Object.DestroyImmediate(target);
            }
            #else
            UnityEngine.Object.Destroy(target);
            #endif
        }
    }
}