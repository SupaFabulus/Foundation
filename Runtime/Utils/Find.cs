using System.Collections.Generic;
using UnityEngine;

namespace SupaFabulus.Dev.Foundation.Utils
{
    public static class Find
    {
        
        public static TComponent[] ArrayOf<TComponent>(bool excludeInactive = true)
            where TComponent : MonoBehaviour
        {
            FindObjectsInactive inactive = excludeInactive
                ? FindObjectsInactive.Exclude
                : FindObjectsInactive.Include;

            return Object.FindObjectsByType<TComponent>
                (inactive, FindObjectsSortMode.InstanceID);
        }
        
        public static List<TComponent> ListOf<TComponent>(bool excludeInactive = true)
            where TComponent : MonoBehaviour
        {
            TComponent[] instances = ArrayOf<TComponent>(excludeInactive);
            if (instances != null)
            {
                TComponent inst;
                List<TComponent> result = new();
                int c = instances.Length;

                for (int i = 0; i < c; i++) 
                { result.Add(instances[i]); }

                return result;
            }

            return null;
        }
        
        public static bool AddToListOf<TComponent>(in List<TComponent> targetList, bool excludeInactive = true)
            where TComponent : MonoBehaviour
        {
            if (targetList == null) return false;
            TComponent[] instances = ArrayOf<TComponent>(excludeInactive);
            if (instances != null)
            {
                int c = instances.Length;
                for (int i = 0; i < c; i++) 
                { targetList.Add(instances[i]); }

                return true;
            }

            return false;
        }        
    }
}