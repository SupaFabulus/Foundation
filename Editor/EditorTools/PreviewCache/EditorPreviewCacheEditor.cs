#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using SupaFabulus.Dev.Foundation.EditorTools.Common;
using UnityEditor;
using UnityEngine;

namespace SupaFabulus.Dev.Foundation.EditorTools.PreviewCache
{
    [Serializable]
    [CustomEditor(typeof(EditorPreviewCache))]
    public class EditorPreviewCacheEditor : EditorBase
    {


        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            EditorPreviewCache cache = target as EditorPreviewCache;

            if (cache != null)
            {
                Rect r = EditorGUILayout.GetControlRect(false, 24, GUI.skin.button, Array.Empty<GUILayoutOption>());

                if (GUI.Button(r, "Clear Cache"))
                {
                    cache.ResetCache();
                }

                Dictionary<long, RenderTexture> entries = cache.Entries;
                int c = entries.Count;
                long i = 0;
                ColorLabel(Color.white, $"{c} Entr{((c == 1) ? "y" : "ies")}:");
                RenderTexture t;
                
                foreach (KeyValuePair<long, RenderTexture> entry in entries)
                {
                    t = entry.Value;

                    if (t == null)
                    {
                        entries.Remove(entry.Key);
                        continue;
                    }

                    ColorLabel(Color.gray, $"{i}: {entry.Value.name}) ID:[{entry.Key}] - ({entry.Value.width}x{entry.Value.height})");
                    i++;
                }
            }
        }
    }
}
#endif