#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;

namespace SupaFabulus.Dev.Foundation.EditorTools.Common
{
    [Serializable]
    public class EditorStyling
    {
        
        public const int DEFAULT_LEFT_COLUMN_LABEL_WIDTH = 116;
        public const int DEFAULT_LEFT_COLUMN_MARGIN = 4;
        public const int DEFAULT_POPUP_STATIC_WIDTH = 128;
        public const int DEFAULT_PREVIEW_MAX_SIZE = 256;
        
        [SerializeField]
        public int LeftColumnWidth = DEFAULT_LEFT_COLUMN_LABEL_WIDTH;
        [SerializeField]
        public int LeftColumnMargin = DEFAULT_LEFT_COLUMN_MARGIN;
        [SerializeField]
        public int PopupStaticWidth = DEFAULT_POPUP_STATIC_WIDTH;
        [SerializeField]
        public int PreviewMaxSize = DEFAULT_PREVIEW_MAX_SIZE;
    
        [SerializeField]
        public Color DefaultLabelColor = new Color(0.8f, 0.8f, 0.8f);
        [SerializeField]
        public Color DefaultLabelColorSelected = new Color(0.9f, 0.9f, 0.9f);

        [SerializeField]
        public static readonly GUILayoutOption[] NO_LAYOUT = new GUILayoutOption[0];

        [SerializeField]
        public GUIStyle Style_LeftColumn;
        [SerializeField]
        public GUILayoutOption[] Layout_LeftColumn;

        [SerializeField]
        public GUIStyle Style_Header;
        [SerializeField]
        public GUILayoutOption[] Layout_Header;

        [SerializeField]
        public GUIStyle Style_PopupStatic;
        [SerializeField]
        public GUILayoutOption[] Layout_PopupStatic;

        [SerializeField]
        public float[] RampValues_grayGlossNormal = new[]
        {
            0.4f,
            0.8f,
            0.7f,
            0.6f,
            0.6f,
            0.6f,
            0.5f,
            0.3f
        };

        [SerializeField]
        public float[] RampValues_grayGlossLit = new[]
        {
            0.4f,
            0.8f,
            0.7f,
            0.6f,
            0.65f,
            0.75f,
            0.65f,
            0.4f
        };

        [SerializeField]
        public Color[] RampColors_glassButtonNormal = new[]
        {
            (Color.gray+Color.white)/2,
            Color.white,
            Color.gray,
            Color.gray,
            Color.gray,
            (Color.gray+Color.black)/2,
            (Color.gray+Color.black)/3,
            Color.gray
        };

        [SerializeField]
        public float[] RampAlphas_glassButtonNormal = new[]
        {
            1f,
            0.7f,
            0.4f,
            0.2f,
            0.1f,
            0.3f,
            0.5f,
            1f
        };

        [SerializeField]
        public Texture2D Ramp_grayGlossNormal22;
        [SerializeField]
        public Texture2D Ramp_grayGlossPressed22;

        [SerializeField]
        public Texture2D Ramp_grayGlossNormalLit22;
        [SerializeField]
        public Texture2D Ramp_grayGlossPressedLit22;
        
        [SerializeField]
        public Texture2D Ramp_GlassButtonNormal22;
        [SerializeField]
        public Texture2D Ramp_GlassButtonHover22;
        [SerializeField]
        public Texture2D Ramp_GlassButtonActive22;
        
        
        [SerializeField]
        public Texture2D Ramp_GlassButton128_Normal;
        [SerializeField]
        public Texture2D Ramp_GlassButton128_Active;
        
        
        protected GUISkin _skin;
        
        

        protected GUIStyle _btnStyle_glass;
        protected GUIStyle _btnStyle_glassLit;
        protected GUIStyle _btnStyle_greyGloss;
        protected GUIStyle _btnStyle_greyGlossLit;
        public GUIStyle ButtonStyle_Glass => _btnStyle_glass;
        public GUIStyle ButtonStyle_GlassLit => _btnStyle_glassLit;
        public GUIStyle ButtonStyle_GreyGloss => _btnStyle_greyGloss;
        public GUIStyle ButtonStyle_GreyGlossLit => _btnStyle_greyGlossLit;

        public void Reset()
        {
            
            _skin = AssetDatabase.LoadAssetAtPath<GUISkin>(
                "Assets/_INTERNAL/_Source/MainFrame/Assets/_INTERNAL/UI/SBF.guiskin");
            
            
            Style_Header = new GUIStyle();
            Style_Header.fontStyle = FontStyle.Bold;
            Style_Header.normal.textColor = DefaultLabelColor;
            
            Style_LeftColumn = new GUIStyle();
            Style_LeftColumn.alignment = TextAnchor.MiddleRight;
            Style_LeftColumn.normal.textColor = DefaultLabelColor;
            
            Style_PopupStatic = new GUIStyle();

            Layout_Header = new[]
            {
                GUILayout.MinWidth(LeftColumnWidth)
            };

            Layout_LeftColumn = new[]
            {
                GUILayout.Width(DEFAULT_LEFT_COLUMN_LABEL_WIDTH)
            };

            Layout_PopupStatic = new[]
            {
                GUILayout.Width(DEFAULT_POPUP_STATIC_WIDTH)
            };

            /*
            Ramp_grayGlossNormal22 = TextureUtils.VerticalRamp2D(22, Color.gray, RampValues_grayGlossNormal);
            Ramp_grayGlossPressed22 = TextureUtils.VerticalRamp2D(22, Color.gray * 0.75f, RampValues_grayGlossNormal, true);

            Ramp_grayGlossNormalLit22 = TextureUtils.VerticalRamp2D(22, Color.white, RampValues_grayGlossLit);
            Ramp_grayGlossPressedLit22 = TextureUtils.VerticalRamp2D(22, Color.white * 0.75f, RampValues_grayGlossLit, true);

            
            Ramp_GlassButtonNormal22 = TextureUtils.VerticalRamp2D(22, Color.white, RampColors_glassButtonNormal, RampAlphas_glassButtonNormal);
            Ramp_GlassButtonHover22 = TextureUtils.VerticalRamp2D(22, (Color.white + Color.gray)/2f, RampColors_glassButtonNormal, RampAlphas_glassButtonNormal);
            Ramp_GlassButtonActive22 = TextureUtils.VerticalRamp2D(22, (Color.white + Color.gray)/2f, RampColors_glassButtonNormal, RampAlphas_glassButtonNormal, true);
            */
            
            Ramp_GlassButton128_Normal =
                AssetDatabase.LoadAssetAtPath<Texture2D>(EditorGUIUtils.IconPathBase + "UI/glassButton128_normal.png");
            Ramp_GlassButton128_Active =
                AssetDatabase.LoadAssetAtPath<Texture2D>(EditorGUIUtils.IconPathBase + "UI/glassButton128_active.png");

            _btnStyle_glass = new GUIStyle(GUI.skin.button);
            _btnStyle_glass.normal.background = Ramp_GlassButtonNormal22;
            _btnStyle_glass.hover.background = Ramp_GlassButtonHover22;
            _btnStyle_glass.active.background = Ramp_GlassButtonActive22;
            _btnStyle_glass.normal.textColor =
                _btnStyle_glass.hover.textColor =
                    _btnStyle_glass.active.textColor = Color.white;
            
            
            _btnStyle_glassLit = new GUIStyle(GUI.skin.button);
            _btnStyle_glassLit.normal.background = Ramp_GlassButton128_Normal;
            _btnStyle_glassLit.hover.background = Ramp_GlassButton128_Normal;
            _btnStyle_glassLit.active.background = Ramp_GlassButton128_Active;
            _btnStyle_glassLit.normal.textColor =
                _btnStyle_glassLit.hover.textColor =
                    _btnStyle_glassLit.active.textColor = Color.white;
            
            
            _btnStyle_greyGloss = new GUIStyle(GUI.skin.button);
            _btnStyle_greyGloss.normal.background = Ramp_grayGlossNormal22;
            _btnStyle_greyGloss.hover.background = Ramp_grayGlossNormal22;
            _btnStyle_greyGloss.active.background = Ramp_grayGlossPressed22;
            _btnStyle_greyGloss.normal.textColor =
                _btnStyle_greyGloss.hover.textColor =
                    _btnStyle_greyGloss.active.textColor = Color.white;
            
            
            _btnStyle_greyGlossLit = new GUIStyle(GUI.skin.button);
            _btnStyle_greyGlossLit.normal.background = Ramp_grayGlossNormalLit22;
            _btnStyle_greyGlossLit.hover.background = Ramp_grayGlossNormalLit22;
            _btnStyle_greyGlossLit.active.background = Ramp_grayGlossPressedLit22;
            _btnStyle_greyGlossLit.normal.textColor =
                _btnStyle_greyGlossLit.hover.textColor =
                    _btnStyle_greyGlossLit.active.textColor = Color.white;

            const int fontSize = 14;
            bool hasFont = _skin != null && _skin.font != null;
            
            _btnStyle_glass.font = hasFont
                ? _skin.font : _btnStyle_glass.font;
            _btnStyle_glass.fontSize = hasFont
                ? fontSize : _btnStyle_glass.fontSize;
            
            _btnStyle_glassLit.font = hasFont
                ? _skin.font : _btnStyle_glassLit.font;
            _btnStyle_glassLit.fontSize = hasFont
                ? fontSize : _btnStyle_glassLit.fontSize;
            
            _btnStyle_greyGloss.font = hasFont
                ? _skin.font : _btnStyle_greyGloss.font;
            _btnStyle_greyGloss.fontSize = hasFont
                ? fontSize : _btnStyle_greyGloss.fontSize;
            
            _btnStyle_greyGlossLit.font = hasFont
                ? _skin.font : _btnStyle_greyGlossLit.font;
            _btnStyle_greyGlossLit.fontSize = hasFont
                ? fontSize : _btnStyle_greyGlossLit.fontSize;
            //_btnStyle_greyGlossLit.fontStyle = FontStyle.Bold;



        }

        public EditorStyling()
        {
            
        }
        
    }
}
#endif