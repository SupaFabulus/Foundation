using System;
using System.Runtime.InteropServices;
using SupaFabulus.Dev.Foundation.Core.Enums;
using UnityEditor;
using UnityEngine;

namespace SupaFabulus.Dev.Foundation.Common.DataModels
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct KeyFrameData
    {
        
        [SerializeField]
        [MarshalAs(UnmanagedType.R4, SizeConst = 4)]
        public float Time;
        [SerializeField]
        [MarshalAs(UnmanagedType.R4, SizeConst = 4)]
        public float Value;
        [SerializeField]
        [MarshalAs(UnmanagedType.R4, SizeConst = 4)]
        public float InTangent;
        [SerializeField]
        [MarshalAs(UnmanagedType.R4, SizeConst = 4)]
        public float OutTangent;
        [SerializeField]
        [MarshalAs(UnmanagedType.I1, SizeConst = 1)]
        public TangentMode LeftTangentMode;
        [SerializeField]
        [MarshalAs(UnmanagedType.I1, SizeConst = 1)]
        public TangentMode RightTangentMode;
        [SerializeField]
        [MarshalAs(UnmanagedType.I1, SizeConst = 1)]
        public WeightedMode WeightedMode;
        [SerializeField]
        [MarshalAs(UnmanagedType.R4, SizeConst = 4)]
        public float InWeight;
        [SerializeField]
        [MarshalAs(UnmanagedType.R4, SizeConst = 4)]
        public float OutWeight;

        public KeyFrameData(Keyframe key)
        {
            Time = key.time;
            Value = key.value;
            InTangent = key.inTangent;
            OutTangent = key.outTangent;
            LeftTangentMode = TangentMode.Auto;
            RightTangentMode = TangentMode.Auto;
            WeightedMode = key.weightedMode;
            InWeight = key.inWeight;
            OutWeight = key.outWeight;
        }

        public KeyFrameData(AnimationCurve curve, int keyIndex)
        {
            Keyframe k = curve[keyIndex];
            
            Time = k.time;
            Value = k.value;
            InTangent = k.inTangent;
            OutTangent = k.outTangent;
            LeftTangentMode = TangentMode.Auto;
            RightTangentMode = TangentMode.Auto;
            WeightedMode = k.weightedMode;
            InWeight = k.inWeight;
            OutWeight = k.outWeight;
        }

        public Keyframe Keyframe => new Keyframe
        (
            time:       Time,
            value:      Value,
            inTangent:  InTangent,
            outTangent: OutTangent,
            inWeight:   InWeight,
            outWeight:  OutWeight
        );
    }
}