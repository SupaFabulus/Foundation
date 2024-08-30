#if UNITY_EDITOR
using UnityEditor;
#endif
using SupaFabulus.Dev.Foundation.Core.Singletons;
using SupaFabulus.Dev.Foundation.Utils;
using UnityEngine;

namespace SupaFabulus.Dev.Foundation
{
    public abstract class SettingsAsset<TUtilitySettings> : 
        AbstractSingletonAsset<TUtilitySettings>, 
        ISingleton<TUtilitySettings>
        where TUtilitySettings : 
            ScriptableObject, 
            ISingleton<TUtilitySettings>
    {
        public virtual void SetAssetsRootFromSelection()
        {
#if UNITY_EDITOR
            string currentDir = AssetUtils.CurrentFocusedAssetDirectory;
            bool validDir = AssetDatabase.IsValidFolder(currentDir);

            if (validDir)
            {
                _rootAssetsTarget = currentDir;
            }
#endif
        }

        [SerializeField]
        protected string _rootAssetsTarget;
        public string RootAssetsTargetFolderPath => _rootAssetsTarget;

        protected override bool InitInstance()
        {
            Validate();
            return true;
        }

    }
}