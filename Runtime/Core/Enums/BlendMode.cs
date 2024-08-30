using System;

namespace SupaFabulus.Dev.Foundation.Core.Enums
{
    [Serializable]
    public enum BlendMode
    {
        None,
        Add,
        Subtract,
        InverseSubtract,
        Multiply,
        Divide,
        InverseDivide,
        Overlay,
        HardLight,
        Screen,
        ColorDodge,
        ColorBurn,
        LinearBurn,
        LinearLight,
        VividLight,
        Difference,
        Average,
        SoftLight,
        PinLight,
        HardMix,
        Exclusion,
        Color,
        Saturation,
        Luminosity,
        Exponential,
        InverseExponential,
    }
}