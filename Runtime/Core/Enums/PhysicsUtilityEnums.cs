using System;

namespace SupaFabulus.Dev.Foundation.Core.Enums
{
    [Flags]
    public enum BoundarySide
    {
        Right = (1 << 0),
        Left = (1 << 1),
        Top = (1 << 2),
        Bottom = (1 << 3),
        Front = (1 << 4),
        Back = (1 << 5)
    }

    public enum BoundaryCenterAlignment
    {
        Inner = -1,
        Center = 0,
        Outer = 1
    }
}