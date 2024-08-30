using System;
using UnityEditor;
using UnityEngine;

namespace SupaFabulus.Dev.Definitions
{
    [Serializable]
    [CreateAssetMenu(
        fileName = "SceneDefinition",
        menuName = "SupaFabulus/Dev/Definitions/Scene Definition"
    )]
    public class SceneDefinition : AbstractDefinition<string>
    {
#if UNITY_EDITOR
        [SerializeField]
        protected SceneAsset _sceneAsset;
        public SceneAsset Scene => _sceneAsset;
        
        protected void OnValidate() { UpdateContentFromSceneAsset(); }
        protected void UpdateContentFromSceneAsset()
        {
            if (_autoValidateContent && _sceneAsset != null)
                _content = _sceneAsset.name;
        }
#endif
    }
}