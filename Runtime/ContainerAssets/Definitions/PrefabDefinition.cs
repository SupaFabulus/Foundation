using System;
using UnityEngine;

namespace SupaFabulus.Dev.Foundation.ContainerAssets.Definitions
{
    [Serializable]
    public abstract class AbstractPrefabDefinition : AbstractDefinition<GameObject>
    {
        
    }
    
    
    [Serializable]
    [CreateAssetMenu(
        fileName = "PrefabDefinition",
        menuName = "SupaFabulus/Dev/Definitions/Generic Prefab Definition"
    )]
    public class PrefabDefinition : AbstractPrefabDefinition, IPrefabDefinition
    {
        public string SourceID => null;

        public GameObject GameObjectSource => null;
    }
    
    
    [Serializable]
    [CreateAssetMenu(
        fileName = "TypedPrefabDefinition",
        menuName = "SupaFabulus/Dev/Definitions/Component-Typed Prefab Definition"
    )]
    public class TypedPrefabDefinition<TComponent> : PrefabDefinition
    where TComponent : MonoBehaviour
    {
        [SerializeField]
        protected TComponent _typedComponentSource;
        public TComponent TypedComponentSource => _typedComponentSource;
    }
}