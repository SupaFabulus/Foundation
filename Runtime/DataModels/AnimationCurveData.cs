using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace SupaFabulus.Dev.Foundation.Common.DataModels
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct AnimationCurveData
    {
        private const byte _kMAX_KEYS = 5;


        [SerializeField]
        [MarshalAs(UnmanagedType.I1, SizeConst = 1)]
        public byte _presetID;

        [SerializeField]
        [MarshalAs(UnmanagedType.Bool, SizeConst = 1)]
        public bool UseSourceCurve;
        
        [SerializeField]
        private AnimationCurve _source;
        
        
        [SerializeField][HideInInspector]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 28, ArraySubType = UnmanagedType.IUnknown)]
        public KeyFrameData[] KeyFrames;

        [SerializeField][HideInInspector]
        [MarshalAs(UnmanagedType.R4, SizeConst = 4)]
        public float Length;

        [SerializeField][HideInInspector]
        [MarshalAs(UnmanagedType.I1, SizeConst = 1)]
        public WrapMode PreWrapMode;

        [SerializeField][HideInInspector]
        [MarshalAs(UnmanagedType.I1, SizeConst = 1)]
        public WrapMode PostWrapMode;

        public AnimationCurveData
        (
            KeyFrameData[] keyFrameData,
            float length,
            WrapMode preWrapMode,
            WrapMode postWrapMode
        )
        {
            KeyFrames = default;
            Array.Copy
            (
                keyFrameData,
                KeyFrames,
                Math.Min(keyFrameData.Length, _kMAX_KEYS)
            );
            
            Length = length;
            PreWrapMode = preWrapMode;
            PostWrapMode = postWrapMode;
            _source = default;
            _presetID = default;
            UseSourceCurve = false;
        }

        public AnimationCurveData
        (
            Keyframe[] keyFrames,
            float length,
            WrapMode preWrapMode,
            WrapMode postWrapMode
        )
        {
            KeyFrames = ConvertSourceKeyframes(keyFrames);
            Length = length;
            PreWrapMode = preWrapMode;
            PostWrapMode = postWrapMode;
            _source = default;
            _presetID = default;
            UseSourceCurve = false;
        }

        public AnimationCurveData
        (
            AnimationCurve sourceCurve
        )
        {
            _source = sourceCurve;
            KeyFrames = ConvertSourceKeyframes(sourceCurve.keys);
            Length = sourceCurve.length;
            PreWrapMode = sourceCurve.preWrapMode;
            PostWrapMode = sourceCurve.postWrapMode;
            _source = default;
            _presetID = default;
            UseSourceCurve = true;
        }

        private static KeyFrameData[] ConvertSourceKeyframes(Keyframe[] sourceKeyframes)
        {
            int i;
            int c;
            Keyframe k;
            KeyFrameData data;
            KeyFrameData[] result;
            
            c = Math.Min(sourceKeyframes.Length, _kMAX_KEYS);
            result = new KeyFrameData[c];

            for (i = 0; i < c; i++)
            {
                k = sourceKeyframes[i];
                data = new KeyFrameData(k);
                result[i] = data;
            }

            return result;
        }

        private static Keyframe[] ConvertDataToKeyframes(KeyFrameData[] keys)
        {
            int i;
            int c = Math.Min(keys.Length, _kMAX_KEYS);
            Keyframe[] result = new Keyframe[c];

            for (i = 0; i < c; i++)
            {
                result[i] = keys[i].Keyframe;
            }

            return result;
        }

        public void UpdateFromSource()
        {
            KeyFrames = ConvertSourceKeyframes(_source.keys);
            Length = _source.length;
            PreWrapMode = _source.preWrapMode;
            PostWrapMode = _source.postWrapMode;
        }

        public AnimationCurve AnimationCurve
        {
            get
            {
                AnimationCurve c = new AnimationCurve();
                c.keys = ConvertDataToKeyframes(KeyFrames);
                c.preWrapMode = PreWrapMode;
                c.postWrapMode = PostWrapMode;

                return c;
            }
        }
    }
}