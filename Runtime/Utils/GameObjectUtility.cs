using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace SupaFabulus.Dev.Foundation.Utils
{
    public static class GameObjectUtility
    {
        public static List<TComponent> GetComponentsFrom<TComponent>
        (
            GameObject source,
            bool recursive = true,
            List<TComponent> existingSet = null,
            Func<TComponent, bool> filter = null
        )
        {
            return null;
        }
        
        public static void DestroyAllChildren(GameObject source)
        {
            if (source == null) return;
            Transform src = source.transform;
            int i;
            int c = src.childCount;
            Transform t;
            for (i = c - 1; i >= 0; i--)
            {
                t = src.GetChild(i);
                t.SetParent(null);
                Safe.Destroy(t.gameObject);
            }
        }

    }
}