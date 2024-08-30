using System;

namespace SupaFabulus.Dev.Foundation.Core.Enums
{
    [Serializable]
    public enum BasicValueType : byte
    {
        Unknown,
        Byte,
        Short,
        UShort,
        Int,
        UInt,
        Long,
        Float,
        Double,
        String,
        Bool,
        Vector2,
        Vector3,
        Vector2Int,
        Vector3Int,
        Quaternion,
        Color
    }
}