using System;

namespace SupaFabulus.Dev.Foundation.Core.Enums
{
    [Serializable]
    public enum MonoBehaviourInitPhase
    {
        None,
        Awake,
        Start,
        FrameNumber
    }
}