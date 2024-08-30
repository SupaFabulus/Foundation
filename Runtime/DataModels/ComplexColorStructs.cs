using System;
using UnityEngine;

namespace SupaFabulus.Dev.Foundation.Common.DataModels
{
    [Serializable]
    public struct ColorUC
    {
        private const float _kINIT_RGB = 0f;
        private const float _kINIT_A = 1f;
        
        [SerializeField]
        public float r;
        [SerializeField]
        public float g;
        [SerializeField]
        public float b;
        [SerializeField]
        public float a;

        public ColorUC
        (
            float red = _kINIT_RGB, 
            float green = _kINIT_RGB, 
            float blue = _kINIT_RGB, 
            float alpha = _kINIT_A
        )
        {
            r = red;
            g = green;
            b = blue;
            a = alpha;
        }

        public ColorUC(Color c)
        {
            r = c.r;
            g = c.g;
            b = c.b;
            a = c.a;
        }

        public Color Color => new Color(r, g, b, a);
    }

    [Serializable]
    public struct Color2
    {
        [SerializeField]
        public ColorUC a;
        [SerializeField]
        public ColorUC b;

        public Color2
        (
            Color colorA = default,
            Color colorB = default
        )
        {
            a = new ColorUC(colorA);
            b = new ColorUC(colorB);
        }

        public Color2
        (
            ColorUC colorA = default,
            ColorUC colorB = default
        )
        {
            a = colorA;
            b = colorB;
        }
    }
    
    [Serializable]
    public struct Color3
    {
        [SerializeField]
        public ColorUC a;
        [SerializeField]
        public ColorUC b;
        [SerializeField]
        public ColorUC c;

        public Color3
        (
            Color colorA = default,
            Color colorB = default,
            Color colorC = default
        )
        {
            a = new ColorUC(colorA);
            b = new ColorUC(colorB);
            c = new ColorUC(colorC);
        }

        public Color3
        (
            ColorUC colorA = default,
            ColorUC colorB = default,
            ColorUC colorC = default
        )
        {
            a = colorA;
            b = colorB;
            c = colorC;
        }
    }
    
    [Serializable]
    public struct Color4
    {
        [SerializeField]
        public ColorUC a;
        [SerializeField]
        public ColorUC b;
        [SerializeField]
        public ColorUC c;
        [SerializeField]
        public ColorUC d;

        public Color4
        (
            Color colorA = default,
            Color colorB = default,
            Color colorC = default,
            Color colorD = default
        )
        {
            a = new ColorUC(colorA);
            b = new ColorUC(colorB);
            c = new ColorUC(colorC);
            d = new ColorUC(colorD);
        }

        public Color4
        (
            ColorUC colorA = default,
            ColorUC colorB = default,
            ColorUC colorC = default,
            ColorUC colorD = default
        )
        {
            a = colorA;
            b = colorB;
            c = colorC;
            d = colorD;
        }
    }
}