using UnityEngine;

namespace SupaFabulus.Dev.Foundation.ContainerAssets.Definitions
{
    public interface IPrefabDefinition
    {
        string SourceID { get; }
        GameObject GameObjectSource { get; }
    }
}