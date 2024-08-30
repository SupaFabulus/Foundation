using System;
using UnityEngine;

namespace SupaFabulus.Dev.Foundation.Common.DataModels
{
    [Serializable]
    public class ColorCurves
    {
        [SerializeField]
        private AnimationCurve _red;
        [SerializeField]
        private bool _reverseRed;
        [SerializeField]
        private AnimationCurve _green;
        [SerializeField]
        private bool _reverseGreen;
        [SerializeField]
        private AnimationCurve _blue;
        [SerializeField]
        private bool _reverseBlue;
        [SerializeField]
        private AnimationCurve _alpha;
        [SerializeField]
        private bool _reverseAlpha;

        public AnimationCurve Red 
        {
            get => _red;
            set => _red = value;
        }
        public AnimationCurve Green 
        {
            get => _green;
            set => _green = value;
        }
        public AnimationCurve Blue 
        {
            get => _blue;
            set => _blue = value;
        }
        public AnimationCurve Alpha
        {
            get => _alpha;
            set => _alpha = value;
        }

        public bool ReverseRed
        {
            get => _reverseRed;
            set => _reverseRed = value;
        }
        public bool ReverseGreen
        {
            get => _reverseGreen;
            set => _reverseGreen = value;
        }
        public bool ReverseBlue
        {
            get => _reverseBlue;
            set => _reverseBlue = value;
        }
        public bool ReverseAlpha
        {
            get => _reverseAlpha;
            set => _reverseAlpha = value;
        }

        public AnimationCurveData RedData => new AnimationCurveData(_red);
        public AnimationCurveData GreenData => new AnimationCurveData(_green);
        public AnimationCurveData BlueData => new AnimationCurveData(_blue);
        public AnimationCurveData AlphaData => new AnimationCurveData(_alpha);

        public ColorCurves
        (
            AnimationCurve red,
            AnimationCurve green,
            AnimationCurve blue,
            AnimationCurve alpha    
        )
        {
            _red = red;
            _green = green;
            _blue = blue;
            _alpha = alpha;

            _reverseRed = false;
            _reverseGreen = false;
            _reverseBlue = false;
            _reverseAlpha = false;
        }
        
        public ColorCurves
        (
            float redStart = 0f,
            float greenStart = 0f,
            float blueStart = 0f,
            float alphaStart = 1f,
            float redEnd = 1f,
            float greenEnd = 1f,
            float blueEnd = 1f,
            float alphaEnd = 1f
        )
        {
            _red = new AnimationCurve
            (
                new Keyframe[]
                {
                    new Keyframe(0f, redStart), 
                    new Keyframe(1f, redEnd)
                }
            );
            _green = new AnimationCurve
            (
                new Keyframe[]
                {
                    new Keyframe(0f, greenStart), 
                    new Keyframe(1f, greenEnd)
                }
            );
            _blue = new AnimationCurve
            (
                new Keyframe[]
                {
                    new Keyframe(0f, blueStart), 
                    new Keyframe(1f, blueEnd)
                }
            );
            _alpha = new AnimationCurve
            (
                new Keyframe[]
                {
                    new Keyframe(0f, alphaStart), 
                    new Keyframe(1f, alphaEnd)
                }
            );

            _reverseRed = false;
            _reverseGreen = false;
            _reverseBlue = false;
            _reverseAlpha = false;
        }

    }
}