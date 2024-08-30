
using UnityEngine;

namespace SupaFabulus.Dev.Foundation.Utils
{
    /*
    public static class TextureUtils
    {
        private static readonly string _BLEND_MAT_NAME = "Compositing/Blending";
        private static Material _blendMat;
        private static RenderTexture _activeTemp;
        private static RenderTexture _buffer;

        private static bool InitBlendingMaterial()
        {
            Shader s = Shader.Find(_BLEND_MAT_NAME);
            if (s != null)
            {
                _blendMat = new Material(s);
            }
            else
            {
                Debug.LogError($"Unable to find shader [{_BLEND_MAT_NAME}] for Blend Mode Operations!");
            }

            return _blendMat != null;
        }

        private static Material Blender
        {
            get
            {
                if (_blendMat == null)
                {
                    if (!InitBlendingMaterial())
                    {
                        Debug.LogError($"Unable to initialize Blending Material!");
                        return null;
                    }
                }

                return _blendMat;
            }
        }

        public static void Blend
        (
            Texture2D topInputLayer,
            RenderTexture baseTargetLayer,
            BlendMode mode = BlendMode.None
        )
        {
            RenderTexture r = RenderTexture.GetTemporary(topInputLayer.width, topInputLayer.height);
            r.enableRandomWrite = true;
            r.Create();
            SafeBlit(topInputLayer, r);
            Blend(r, baseTargetLayer, mode);
            RenderTexture.ReleaseTemporary(r);
        }

        public static void Blend
        (
            RenderTexture topInputLayer,
            RenderTexture baseTargetLayer,
            BlendMode mode = BlendMode.None
        )
        {
            Material m = Blender;
            if (m == null)
            {
                Debug.LogError($"Unable to Blend because the Blending Material was not found.");
            }

            if (_buffer != null)
            {
                _buffer.DiscardContents(true, true);
                RenderTexture.ReleaseTemporary(_buffer);
                _buffer = null;
            }

            if (_buffer == null)
            {
                _buffer = RenderTexture.GetTemporary(baseTargetLayer.width, baseTargetLayer.height, 0);
            }
            
            _buffer.enableRandomWrite = true;
            _buffer.Create();

            m.SetFloat("_Alpha", 1f);
            m.SetInt("_BlendMode", (int)mode);
            m.SetTexture("_TargetTex", baseTargetLayer);
            SafeBlit(topInputLayer, _buffer, m);
            SafeBlit(_buffer, baseTargetLayer);
            
        }

        public static void SafeBlit(RenderTexture src, RenderTexture dst)
        {
            _activeTemp = RenderTexture.active;
            Graphics.Blit(src, dst);
            RenderTexture.active = _activeTemp;
        }
        
        public static void SafeBlit(Texture2D src, RenderTexture dst)
        {
            _activeTemp = RenderTexture.active;
            Graphics.Blit(src, dst);
            RenderTexture.active = _activeTemp;
        }
        
        public static void SafeBlit(RenderTexture src, RenderTexture dst, Material mat)
        {
            _activeTemp = RenderTexture.active;
            Graphics.Blit(src, dst, mat);
            RenderTexture.active = _activeTemp;
        }
        
        public static void SafeBlit(Texture2D src, RenderTexture dst, Material mat)
        {
            _activeTemp = RenderTexture.active;
            Graphics.Blit(src, dst, mat);
            RenderTexture.active = _activeTemp;
        }

        public static Color2 GetColorRangeForTexture(CustomRenderTexture t)
        {
            return GetColorRangeForTexture(t as RenderTexture);
        }

        public static Color2 GetColorRangeForTexture(RenderTexture t)
        {
            Texture2D txt = new Texture2D(t.width, t.height);
            Rect rectReadPicture = new Rect(0, 0, t.width, t.height);

            RenderTexture.active = t;

            // Read pixels
            txt.ReadPixels(rectReadPicture, 0, 0);
            txt.Apply();

            RenderTexture.active = null; // added to avoid errors 
            
            return GetColorRangeForTexture(txt);
        }

        public static Color2 GetColorRangeForTexture(Texture t)
        {
            return GetColorRangeForTexture(t as Texture2D);
        }

        public static Color2 GetColorRangeForTexture(Texture2D t)
        {
            int i, l;
            Color c;
            ColorUC _c;
            Color[] colors;
            Color2 r = new Color2
            (
                Color.clear, 
                Color.clear
            );
            
            
            
            colors = t.GetPixels();
            l = colors.Length;
            
            for (i = 0; i < l; i++)
            {
                c = colors[i];
                
                _c = r.a;
                r.a.r = Mathf.Min(c.r, _c.r);
                r.a.g = Mathf.Min(c.g, _c.g);
                r.a.b = Mathf.Min(c.b, _c.b);
                r.a.a = Mathf.Min(c.a, _c.a);
                
                _c = r.b;
                r.b.r = Mathf.Max(c.r, _c.r);
                r.b.g = Mathf.Max(c.g, _c.g);
                r.b.b = Mathf.Max(c.b, _c.b);
                r.b.a = Mathf.Max(c.a, _c.a);
            }

            return r;
        }


        public static Texture2D VerticalRamp2D(int height, Color tint, Color[] colors, float[] alphas, bool reverse = false)
        {
            if (height < 2 || height >= Mathf.Pow(2, 16))
            {
                Debug.LogError($"Invalid Ramp Height: {height}");
                return null;
            }
            
            int count = colors.Length;
            int cl = colors.Length;
            int al = alphas.Length;
            
            if (cl < 2 || cl > 16 || al < 2 || al > 16)
            {
                Debug.LogError($"Invalid colors or aphas count: {cl}, {al}");
                return null;
            }
            
            int i;
            GradientColorKey ck;
            GradientAlphaKey ak;
            
            Gradient g = new Gradient();
            
            for (i = 0; i < colors.Length; i++)
            {
                                    
            }

            float pct;
            int l = Mathf.Max(cl, al);
            GradientColorKey[] _ck = new GradientColorKey[cl];
            GradientAlphaKey[] _ak = new GradientAlphaKey[al];

            for (i = 0; i < l; i++)
            {
                if (i < cl)
                {
                    pct = (float)i / ((float)cl - 1f);
                    _ck[i] = new GradientColorKey(colors[i] * tint, pct);
                }
                
                if (i < al)
                {
                    pct = (float)i / ((float)al - 1f);
                    _ak[i] = new GradientAlphaKey(alphas[i], pct);
                }
            }

            g.colorKeys = _ck;
            g.alphaKeys = _ak;
            
            Color c;
            Color[] stream = new Color[height];
            Texture2D ramp = new Texture2D(1, height);
            
            for (i = 0; i < height; i++)
            {
                pct = ((float)i / ((float)height - 1f));
                pct = reverse ? pct : 1f - pct;
                c = g.Evaluate(pct);
                stream[i] = c;
            }
            
            ramp.SetPixels(stream);
            ramp.Apply(true);
            
            return ramp;
        }
        
        public static Texture2D VerticalRamp2D(int height, Color baseColor, float[] intensity, bool reverse = false)
        {
            if (height < 2 || height >= Mathf.Pow(2, 16))
            {
                Debug.LogError($"Invalid Ramp Height: {height}");
                return null;
            }
            
            int count = intensity.Length;
            
            if (count < 2 || count > 16)
            {
                Debug.LogError($"Invalid intensity count: {count}");
                return null;
            }
            
            AnimationCurve curve = new AnimationCurve();

            int i;

            float pct;

            for (i = 0; i < count; i++)
            {
                pct = (float)i / ((float)count - 1f);
                curve.AddKey(pct, intensity[i]);
            }
            Color c;
            Color[] stream = new Color[height];
            Texture2D ramp = new Texture2D(1, height);
            
            for (i = 0; i < height; i++)
            {
                pct = ((float)i / ((float)height - 1f));
                pct = reverse ? pct : 1f - pct;
                c = baseColor * curve.Evaluate(pct);
                c.a = 1;
                stream[i] = c;
            }
            
            ramp.SetPixels(stream);
            ramp.Apply(true);
            
            return ramp;
        }

        public static Texture2D VerticalRamp2D(int height, Color topColor, Color bottomColor, bool reverse = false)
        {
            if (height < 2 || height >= Mathf.Pow(2, 16))
            {
                Debug.LogError($"Invalid Ramp Height: {height}");
                return null;
            }

            int i;
            float pct;
            Color[] stream = new Color[height];
            Texture2D ramp = new Texture2D(1, height);
            
            for (i = 0; i < height; i++)
            {
                pct = ((float)i / ((float)height - 1f));
                pct = reverse ? pct : 1f - pct;
                stream[i] = Color.Lerp(bottomColor, topColor, pct);
            }
            
            ramp.SetPixels(stream);
            ramp.Apply(true);

            return ramp;
        }
        
        public static void RenderRampFromCurves
        (
            ColorCurves curves,
            ref RenderTexture target
        )
        {
            int i;
            float v;
            Color c;
            Color[] colors;
            int size = target.height;

            bool invertRed = curves.ReverseRed;
            bool invertGreen = curves.ReverseGreen;
            bool invertBlue = curves.ReverseBlue;
            bool invertAlpha = curves.ReverseAlpha;
            
            colors = new Color[size];

            Texture2D t = new Texture2D(1, size);
		        
            for (i = 0; i < size; i++)
            {
                v = (float)(i) / (float)(size - 1);
		        
                c = new Color
                (
                    curves.Red.Evaluate(invertRed ? (1 - v) : v),
                    curves.Green.Evaluate(invertGreen ? (1 - v) : v),
                    curves.Blue.Evaluate(invertBlue ? (1 - v) : v),
                    curves.Alpha.Evaluate(invertAlpha ? (1 - v) : v)
                );
		        
                colors[i] = c;
            }
            t.SetPixels(colors);
            t.Apply(true);
            
            Graphics.Blit(t, target);
        }
    }
    */
}