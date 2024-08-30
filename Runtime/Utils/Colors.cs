using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace SupaFabulus.Dev.Foundation.Utils
{
    public static class ColorExtensions
    {

        public static Color Mix(this Color thisColor, Color thatColor)
        {
            thisColor = (thisColor + thatColor) / 2;
            return thisColor;
        }
        
        public static Color GetShade(this Color thisColor, float intensity, bool shadeAlpha = false)
        {
            Color c = (thisColor * intensity);
            if (!shadeAlpha) c.a = thisColor.a;
            return c;
        }
        
        public static Color GetShade(this Color thisColor, Shade intensity, bool shadeAlpha = false)
        {
            Color c = (thisColor * Colors.ShadeValue(intensity));
            if (!shadeAlpha) c.a = thisColor.a;
            return c;
        }
        
        public static string ToStringHex(this Color thisColor)
        {
            return $"{(byte)(thisColor.r*255f):X2}{(byte)(thisColor.g*255f):X2}{(byte)(thisColor.b*255f):X2}";
        }
    }
    
    
    public enum Hue
    {
        White,
        Red,
        RedOrange,
        Orange,
        Gold,
        Yellow,
        Lime,
        Green,
        Aqua,
        Cyan,
        Turquoise,
        Sky,
        Blue,
        Indigo,
        Purple,
        Violet,
        Magenta,
        Fuscia
    }
        
    public enum Shade : ushort
    {
        Black = 0,
        GrayExtraDark = 1,
        GrayDark = 2,
        GraySemiDark = 3,
        GrayMedium = 4,
        GraySemiLight = 5,
        GrayLight = 6,
        GrayExtraLight = 7,
        White = 8
    }


    public class Colors
    {

        public static float ShadeValue(Shade shade) => (float)shade / 8f;

        private static Dictionary<Hue, Color> __hueTable = new Dictionary<Hue, Color>{
            { Hue.White,         new Color( 1f , 1f , 1f, 1f ) },
            { Hue.Red,           new Color( 1f , 0f , 0f, 1f ) },
            { Hue.RedOrange,     new Color( 1f , 0.25f , 0f, 1f ) },
            { Hue.Orange,        new Color( 1f , 0.5f , 0f, 1f ) },
            { Hue.Gold,          new Color( 1f , 0.75f , 0f, 1f ) },
            { Hue.Yellow,        new Color( 1f , 1f , 0f, 1f ) },
            { Hue.Lime,          new Color( 0.75f , 1f , 0f, 1f ) },
            { Hue.Green,         new Color( 0f , 1f , 0f, 1f ) },
            { Hue.Aqua,          new Color( 0f , 1f , 0.75f, 1f ) },
            { Hue.Cyan,          new Color( 0f , 1f , 1f, 1f ) },
            { Hue.Turquoise,     new Color( 0f , 0.75f , 1f, 1f ) },
            { Hue.Sky,           new Color( 0f , 0.5f , 1f, 1f ) },
            { Hue.Blue,          new Color( 0f , 0f , 1f, 1f ) },
            { Hue.Indigo,        new Color( 0.33f , 0f , 1f, 1f ) },
            { Hue.Purple,        new Color( 0.5f , 0f , 1f, 1f ) },
            { Hue.Violet,        new Color( 0.75f , 0f , 1f, 1f ) },
            { Hue.Magenta,       new Color( 1f , 0f , 1f, 1f ) },
            { Hue.Fuscia,        new Color( 1f , 0f , 0.5f, 1f ) }
        };
        
        private static Dictionary<Shade, Color> __shadeTable = new Dictionary<Shade, Color>{
            { Shade.Black,          Color.white.GetShade(Shade.Black) },
            { Shade.GrayExtraDark,  Color.white.GetShade(Shade.GrayExtraDark) },
            { Shade.GrayDark,       Color.white.GetShade(Shade.GrayDark) },
            { Shade.GraySemiDark,   Color.white.GetShade(Shade.GraySemiDark) },
            { Shade.GrayMedium,     Color.white.GetShade(Shade.GrayMedium) },
            { Shade.GraySemiLight,  Color.white.GetShade(Shade.GraySemiLight) },
            { Shade.GrayLight,      Color.white.GetShade(Shade.GrayLight) },
            { Shade.GrayExtraLight, Color.white.GetShade(Shade.GrayExtraLight) },
            { Shade.White,          Color.white.GetShade(Shade.White) },
        };

        public static string DebugString(string message, Color c)
        {
            StringBuilder _sb = new();
            //_sb.Clear();
            
            _sb.Append("<color=#")
                .Append(c.ToStringHex())
                .Append(">")
                .Append(message)
                .Append("</color>");

            return _sb.ToString();
        }

        public static Color GetColor( Hue hue, Shade shade, bool shadeAlpha = false )
        {
            //StringBuilder _sb = new();
            Color c = (Color) __hueTable[hue];
            
            /*
            float a = c.a;

            Color w = Color.white;

            _sb.Clear()
                .Append(DebugString("Color", c))
                .Append(" * ")
                .Append(DebugString("Shade", w.GetShade(shade)))
                .Append(" = ")
                .Append(DebugString("Mix", c.GetShade(shade)));

            //Debug.Log($"{c} : {c.ToStringHex()}");
            //Debug.Log($"{w.GetShade(shade)} : {w.GetShade(shade).ToStringHex()}");
            //Debug.Log($"{c.GetShade(shade)} : {c.GetShade(shade).ToStringHex()}");
            //Debug.Log(_sb.ToString());
            
            c.a = shadeAlpha ? c.a : a;
            */
                
            return c.GetShade(shade, shadeAlpha);
        }
        
        public static Color GetColor( Hue hue )
        {
            return (Color) __hueTable [hue];
        }
 
        public static Color GetMultipliedColor( Hue hue, float multiplier = 1f)
        {
            return __hueTable[hue] * multiplier;
        }
        
        public static Color32 GetMultipliedTransparentColor( Hue hue, float opacity = 1f, float multiplier = 1f)
        {
            Color c = GetMultipliedColor(hue);
            c.a = opacity;
            return c;
        }
        
        public static Color32 GetMultipliedTransparentColor( Color color, float opacity = 1f, float multiplier = 1f)
        {
            Color c = color;
            c.a = opacity;
            return c;
        }
        
        public static Color32 GetTransparentColor( Hue hue, float opacity = 1f)
        {
            Color c = __hueTable[hue];
            c.a = opacity;
            return c;
        }
        
        
    }
}