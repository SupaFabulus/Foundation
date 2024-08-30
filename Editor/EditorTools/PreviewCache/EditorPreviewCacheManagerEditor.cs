#if UNITY_EDITOR
using System;
using SupaFabulus.Dev.Foundation.EditorTools.Common;
using UnityEditor;
using UnityEngine;

namespace SupaFabulus.Dev.Foundation.EditorTools.PreviewCache
{
    [Serializable]
    [CustomEditor(typeof(EditorPreviewCacheManager))]
    public class EditorPreviewCacheManagerEditor : EditorBase
    {

        private EditorPreviewCacheManager _manager;
        private EditorPreviewCacheManager _instance;
        
        public override void OnInspectorGUI()
        {
            _manager = target as EditorPreviewCacheManager;
            
            DrawDefaultInspector();
            SaveChanges();

            _instance = EditorPreviewCacheManager.ß;
            bool valid = _instance != null && _instance == _manager;

            if (valid)
            {
                SuccessLabel($"Registered Instance: [{_instance.name}]");
                ColorLabel(Color.white, $"({AssetDatabase.GetAssetPath(_instance)})");
            }
            else
            {
                if (_instance == null)
                {
                    ErrorLabel($"Manager Not Registered!");
                }
                else if (_instance != _instance)
                {
                    ErrorLabel($"An existing Manager is already Registered!");
                    ColorLabel(Color.white, $"Existing Manager:({AssetDatabase.GetAssetPath(_instance)})");
                    ColorLabel(Color.yellow, $"THIS Manager:({AssetDatabase.GetAssetPath(_manager)})");
                }
            }
        }
    }
}
#endif