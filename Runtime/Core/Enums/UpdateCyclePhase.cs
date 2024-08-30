using System;

namespace SupaFabulus.Dev.Foundation.Core.Enums
{
    [Serializable]
    public enum UpdateCyclePhase
    {
        FixedUpdate,
        Update,
        LateUpdate
    }
    
    [Serializable]
    [Flags]
    public enum UpdateCyclePhases
    {
        None = 0,
        FixedUpdate = 1,
        Update = 2,
        LateUpdate = 4,
        AllUpdatePhases = 7
    }
    
}