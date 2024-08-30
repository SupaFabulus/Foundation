using System;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using SupaFabulus.Dev.Foundation.Core.Singletons;

namespace SupaFabulus.Dev.Foundation.Utils
{
    public static class AssetUtils
    {
        private const string DEFAULT_ASSET_NAME = "New Asset";

#if UNITY_EDITOR

        

        public static string GetAssetGUID(UnityEngine.Object asset)
        {
            return AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(asset));
        }
        
        public static bool IsFolder(DefaultAsset asset)
        {
            if (asset == null || asset == default) return false;
            return AssetDatabase.IsValidFolder(AssetDatabase.GetAssetPath(asset));
        }
        
#endif
        public static void SaveAsset(UnityEngine.Object assetObj, string assetBaseName, string defaultName = DEFAULT_ASSET_NAME)
        {
            if (assetObj == null) return;
            
            bool validLayerName =
            (
                assetBaseName != default &&
                !string.IsNullOrEmpty(assetBaseName) &&
                !string.IsNullOrWhiteSpace(assetBaseName)
            );
                    
            string dirPath = AssetUtils.CurrentFocusedAssetDirectory;
            string assetPath = dirPath + "/" + (validLayerName ? assetBaseName : defaultName) + ".asset";

#if UNITY_EDITOR
            if (File.Exists(assetPath))
            {
                Debug.LogError($"{assetPath} exists! Please choose a different Asset name, or rename the existing asset.");
            }
            else
            {
                //Debug.Log($"{assetPath} does NOT EXIST");
                AssetDatabase.CreateAsset(assetObj, assetPath);
                ProjectWindowUtil.ShowCreatedAsset(assetObj);
            }
            #endif
        }

#if UNITY_EDITOR
        public static TSingletonAsset GetSingletonAsset<TSingletonAsset>()
            where TSingletonAsset : ScriptableObject, ISingleton<TSingletonAsset>
        {
            TSingletonAsset asset = null;
            string[] paths = AssetDatabase.FindAssets($"t:{typeof(TSingletonAsset).FullName}");
            if (paths != null && paths.Length > 0)
            {
                int c = paths.Length;
                if (c > 1)
                {
                    string n = nameof(TSingletonAsset);
                    Debug.LogWarning($"WARNING: {c} instances of {n} found!");
                    for (int i = 0; i < c; i++)
                    {
                        Debug.LogWarning($"{i}: {n} asset at >> [{paths[i]}]");
                    }
                }

                asset = AssetDatabase.LoadAssetAtPath<TSingletonAsset>(paths[0]);
            }

            return asset;
        }
#endif

        public static string CurrentFocusedAssetDirectory
        {
            get
            {
                string pathToCurrentFolder = null;
#if UNITY_EDITOR
                Type projectWindowUtilType = typeof(ProjectWindowUtil);
                MethodInfo getActiveFolderPath = projectWindowUtilType.GetMethod("GetActiveFolderPath", BindingFlags.Static | BindingFlags.NonPublic);
                object obj = getActiveFolderPath.Invoke(null, new object[0]);
                pathToCurrentFolder = obj.ToString();
                #endif
                //Debug.Log(pathToCurrentFolder);

                return pathToCurrentFolder;
            }
        }
    }
}
