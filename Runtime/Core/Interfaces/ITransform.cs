using UnityEngine;

namespace SupaFabulus.Dev.Foundation.Core.Interfaces
{
    public interface ITransform
    {
        Vector3 Position { get; set; }
        Quaternion Rotation { get; set; }
        Vector3 LocalPosition { get; set; }
        Quaternion LocalRotation { get; set; }
        Vector3 Scale { get; set; }
    }
}