/*
 * DirectUpdateEnums.cs - Enums for different DirectUpdate attributes
 *
 * (not yet in use)
 */

namespace SupaFabulus.Dev.Foundation.Core.Cycle.Enums
{
    public enum DirectUpdateType
    {
        FixedUpdate,
        Update,
        LateUpdate

    }

    public enum DirectUpdatePriority
    {
        None,
        Low,
        Moderate,
        High
    }
}