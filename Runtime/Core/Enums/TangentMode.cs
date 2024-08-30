/*
 * Copied from UnityEngine.Internal (or whatever, it's not exposed) Because it's useful at runtime, duh.
 */


using System;

namespace SupaFabulus.Dev.Foundation.Core.Enums
{
    [Serializable]
    public enum TangentMode
    {
        /// <summary>
        ///   <para>The tangent can be freely set by dragging the tangent handle.</para>
        /// </summary>
        Free,
        /// <summary>
        ///   <para>The tangents are automatically set to make the curve go smoothly through the key.</para>
        /// </summary>
        Auto,
        /// <summary>
        ///   <para>The tangent points towards the neighboring key.</para>
        /// </summary>
        Linear,
        /// <summary>
        ///   <para>The curve retains a constant value between two keys.</para>
        /// </summary>
        Constant,
        /// <summary>
        ///   <para>The tangents are automatically set to make the curve go smoothly through the key.</para>
        /// </summary>
        ClampedAuto,
    }
}