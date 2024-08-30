using System;
using UnityEngine;

namespace SupaFabulus.Dev.Foundation.Common.DataModels
{
    [Serializable]
    public struct ChannelOverride
    {
        [SerializeField]
        public bool Enabled;
        [SerializeField]
        [Range(0f, 1f)]
        public float Value;

        public ChannelOverride
        (
            bool enabled = false,
            float value = 1f
        )
        {
            Enabled = enabled;
            Value = value;
        }
    }

    [Serializable]
    public struct ColorChannelOverrides
    {
        [SerializeField]
        public ChannelOverride Red;
        [SerializeField]
        public ChannelOverride Green;
        [SerializeField]
        public ChannelOverride Blue;
        [SerializeField]
        public ChannelOverride Alpha;
    }
}