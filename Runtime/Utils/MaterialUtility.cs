using System;
using System.Collections.Generic;
using UnityEngine;

namespace SupaFabulus.Dev.Foundation.Utils
{
    public static class MaterialUtility
    {
        public static void CloneMaterial(Renderer source, out Material clone)
        {
            if (source != null && source.sharedMaterial != null)
            {
                CloneMaterial(source.sharedMaterial, out clone);
                source.material = clone;
                return;
            }

            clone = null;
        }
        
        public static void CloneMaterial(Material source, out Material clone)
        {
            if (source != null)
            {
                clone = new Material(source.shader);
                clone.CopyPropertiesFromMaterial(source);
                return;
            }

            clone = null;
        }

        public static void CopyAllMaterials(GameObject from, GameObject to, bool clone = false)
        {
            if (from == null || to == null) return;
            CopyMaterials(from, to);
            CopyMaterials(from, to, true);
        }

        public static void CopyMaterials(GameObject from, GameObject to, bool fromChildren = false)
        {
            int i;
            int c;
            Renderer a;
            Renderer b;
            Renderer[] fromRenderers;
            Renderer[] toRenderers;

            if (fromChildren)
            {
                fromRenderers = from.GetComponentsInChildren<Renderer>();
                toRenderers = to.GetComponentsInChildren<Renderer>();
            }
            else
            {
                fromRenderers = from.GetComponents<Renderer>();
                toRenderers = to.GetComponents<Renderer>();
            }

            c = Math.Min(fromRenderers.Length, toRenderers.Length);
            for (i = 0; i < c; i++)
            {
                a = fromRenderers[i];
                b = toRenderers[i];

                if (a != null && b != null)
                {
                    b.sharedMaterials = a.sharedMaterials;
                }
            }
        }
        
        
        
        public static List<Material> CollectMaterialsFrom
        (
            GameObject source, 
            bool includeChildren = false, 
            List<Material> existingList = null,
            Shader[] limitToTheseShaders = null
        )
        {
            if(source == null) return null;
            
            List<Material> result = existingList != null 
                ? existingList
                : new List<Material>();

            GetMaterialsFromGameObject(source, result, limitToTheseShaders);
            if (!includeChildren) return result;

            GetMaterialsFromChildren(source, result, limitToTheseShaders);
            return result;
        }

        public static List<Material> GetMaterialsFromGameObject
        (
            GameObject source, 
            List<Material> existingList = null, 
            Shader[] limitToTheseShaders = null
        )
        {
            if(source == null) return null;
            
            List<Material> result = existingList != null 
                ? existingList
                : new List<Material>();
            
            GetMaterialsFromRenderers(source.GetComponents<Renderer>(), result);

            return result;
        }

        public static List<Material> GetMaterialsFromChildren
        (
            GameObject parent, 
            List<Material> existingList = null, 
            Shader[] limitToTheseShaders = null
        )
        {
            if(parent == null) return null;
            
            List<Material> result = existingList != null 
                ? existingList
                : new List<Material>();
            
            GetMaterialsFromRenderers(parent.GetComponentsInChildren<Renderer>(), result, limitToTheseShaders);

            return result;
        }
        
        public static List<Material> GetMaterialsFromRenderers
        (
            Renderer[] renderers, 
            List<Material> existingList = null, 
            Shader[] limitToTheseShaders = null
        )
        {
            if(renderers == null) return null;
            
            int i;
            int c;
            Renderer r;
            
            List<Material> result = existingList != null 
                ? existingList
                : new List<Material>();
            
            c = renderers.Length;
            for (i = 0; i < c; i++)
            {
                r = renderers[i];
                if (r != null)
                {
                    GetMaterialsFromRenderer(r, result, limitToTheseShaders);
                }
            }

            return result;
        }

        public static List<Material> GetMaterialsFromRenderer
        (
            Renderer source, 
            List<Material> existingList = null, 
            Shader[] limitToTheseShaders = null
        )
        {
            if(source == null) return null;
            
            int i;
            int c;
            Material m;
            Material[] mats;
            
            List<Material> result = existingList != null 
                ? existingList
                : new List<Material>();

            mats = source.sharedMaterials;
            
            c = mats.Length;
            for (i = 0; i < c; i++)
            {
                m = mats[i];
                if (m != null && !result.Contains(m))
                {
                    if
                    (
                        limitToTheseShaders == null ||
                        limitToTheseShaders.Length < 1 ||
                        Array.IndexOf(limitToTheseShaders, m.shader) >= 0
                    )
                    {
                        result.Add(m);
                    }
                }
            }

            return result;
        }
        
        
    }
}