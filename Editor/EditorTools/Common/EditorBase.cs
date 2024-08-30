using System;
using SupaFabulus.Dev.Foundation.Core.Interfaces;
using SupaFabulus.Dev.Foundation.Utils;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace SupaFabulus.Dev.Foundation.EditorTools.Common
{
    public class EditorBase : Editor
    {
        
        
        protected static EditorStyling _styling;
        protected static bool _valid = false;

        public void SaveChanges()
        {
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssetIfDirty(this);
        }
        
        protected void Label
        (
            string msg, 
            Hue hue = Hue.White, 
            Shade shade = Shade.GrayExtraLight, 
            int minWidth = 64,
            int maxWidth = 128,
            bool bold = false, 
            GUIStyle customStyle = default
        )
        {
            GUIStyle s = new GUIStyle(customStyle != default ? customStyle : GUI.skin.label);
            s.normal.textColor = Colors.GetColor(hue, shade);
            s.fontStyle = bold ? FontStyle.Bold : s.fontStyle;
            GUILayoutOption[] o = new[]
            {
                GUILayout.MinWidth(minWidth),
                GUILayout.MaxWidth(maxWidth)
            };

            EditorGUILayout.LabelField(msg, s, o);
        }
        
        protected void Label(string msg, Color color, int width, bool bold = false, GUIStyle customStyle = default)
        {
            GUIStyle s = new GUIStyle(customStyle != default ? customStyle : GUI.skin.label);
            s.normal.textColor = color;
            s.fontStyle = bold ? FontStyle.Bold : s.fontStyle;
            GUILayoutOption[] o = new[]
            {
                GUILayout.Width(width)
            };

            EditorGUILayout.LabelField(msg, s, o);
        }
        
        protected virtual void ControlRowButton
        (
            string l,
            Texture2D i,
            Action a, 
            GUIStyle s, 
            int h,
            bool truncateLabel = false,
            int truncateThreshold = 64,
            bool isActive = false
        )
        {
            int widthForLabel = h + Mathf.FloorToInt(l.Length * s.fontSize * 0.5f);
            bool doTruncate = truncateLabel && widthForLabel > truncateThreshold;
            GUIContent g = new GUIContent
                (doTruncate ? string.Empty : $" {l}", i, l);
            //s.alignment = TextAnchor.MiddleCenter;
            GUILayoutOption[] o = new[]
            {
                GUILayout.MinWidth
                (
                    doTruncate
                    ? h
                    : truncateThreshold
                )
            };
            //GUILayoutOption[] o = new[] {GUILayout.MinWidth(h), GUILayout.MaxWidth(h)};
            Button(g, a, s, h, o);
        }

        protected void ColorLabel(Color color, string msg)
        {
            GUIStyle s = new GUIStyle(GUI.skin.label);
            s.normal.textColor = color;
            s.fontStyle = FontStyle.Bold;
            GUILayoutOption[] o = Array.Empty<GUILayoutOption>();
                
            EditorGUILayout.LabelField(msg, s, o);
        }

        protected void ErrorLabel(string msg)
        {
            ColorLabel(Color.red, msg);
        }

        protected void SuccessLabel(string msg)
        {
            ColorLabel(Color.green, msg);
        }
        

        protected static void InitEditorStyling()
        {
            if (_styling == null)
            {
                _styling = new EditorStyling();
                _styling.Reset();
            }
        }

        protected virtual void DrawToggle
        (
            string label, 
            ref SerializedProperty property, 
            bool closeRow = true, 
            bool dontOpen = false, 
            int labelWidth = EditorStyling.DEFAULT_LEFT_COLUMN_LABEL_WIDTH
        )
        {
            Rect rowRect;
            if (!dontOpen)
            {
                rowRect = EditorGUILayout.BeginHorizontal();
            }

            LeftColumnLabel(label, true);
            property.boolValue = EditorGUILayout.Toggle
            (
                property.boolValue,
                new []{GUILayout.Width(16), GUILayout.ExpandWidth(false)}
            );
            

            if (closeRow)
            {
                EditorGUILayout.EndHorizontal();
            }
        }
        
        
        
        protected void DrawTexturePreview
        (
            ref Texture2D texture,
            Color[] colorStream,
            int maxSize = EditorStyling.DEFAULT_PREVIEW_MAX_SIZE,
            float previewScale = 1f
        )
        {

            const int RIGHT_MARGIN = 12;
            
            Rect previewRect = EditorGUILayout.BeginHorizontal();
            EditorGUILayout.BeginVertical();
            int size = Mathf.FloorToInt(maxSize * previewScale);
            size = Mathf.Min(size, maxSize);
            previewRect.width -= RIGHT_MARGIN;
            previewRect.height = previewRect.width;

            if (size < 1)
            {
                EditorGUILayout.EndVertical();
                EditorGUILayout.EndHorizontal();
                return;
            }

            if (texture == null)
            {
                texture = new Texture2D(size, size);
            }
            else if(texture.width != size || texture.height != size)
            {
                DestroyImmediate(texture);
                texture = new Texture2D(size, size);
            }
            
            
            texture.SetPixels(colorStream);
            texture.Apply(true);
            
            EditorGUI.DrawPreviewTexture(previewRect, texture);
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();
            
        }
        
        protected virtual void DrawStringField
        (
            string label, 
            ref SerializedProperty property,
            bool closeRow = true
        )
        {
            Rect rowRect = EditorGUILayout.BeginHorizontal();
            LeftColumnLabel(label);
            property.stringValue = EditorGUILayout.TextField
            (
                property.stringValue,
                new[] { GUILayout.ExpandWidth(true) }
            );

            if (closeRow)
            {
                EditorGUILayout.EndHorizontal();
            }
        }
        
        protected virtual void DrawFloatSlider
        (
            string label, 
            ref SerializedProperty property,
            float min = 0f,
            float max = 10f,
            bool closeRow = true
        )
        {
            Rect rowRect = EditorGUILayout.BeginHorizontal();
            LeftColumnLabel(label);
            property.floatValue = EditorGUILayout.Slider
            (
                property.floatValue,
                min, 
                max, 
                new[] { GUILayout.ExpandWidth(true) }
            );

            if (closeRow)
            {
                EditorGUILayout.EndHorizontal();
            }
        }
        
        protected virtual void DrawIntSlider
        (
            string label, 
            ref SerializedProperty property,
            int min = 0,
            int max = 10,
            bool closeRow = true,
            bool dontOpen = false
        )
        {
            Rect rowRect;

            if (!dontOpen)
            {
                rowRect = EditorGUILayout.BeginHorizontal();
            }

            LeftColumnLabel(label);
            property.intValue = EditorGUILayout.IntSlider
            (
                property.intValue,
                min, 
                max, 
                new[] { GUILayout.ExpandWidth(true) }
            );

            if (closeRow)
            {
                EditorGUILayout.EndHorizontal();
            }
        }
        
        public static void HardHorizontalSpace(int width)
        {
            EditorGUILayout.LabelField("",new []{GUILayout.Width(width), GUILayout.ExpandWidth(false)});
        }

        public static void HardVerticalSpace(int height)
        {
            EditorGUILayout.BeginVertical();
            EditorGUILayout.Space(height);
            EditorGUILayout.EndVertical();
        }
        
        public static void LeftColumnLabel(string text, bool useColon = true, bool closeRow = true, bool dontOpen = false)
        {

            if (!dontOpen)
            {
                EditorGUILayout.BeginHorizontal();
            }

            EditorGUILayout.LabelField(text + (useColon ? ":" : ""), _styling.Style_LeftColumn, _styling.Layout_LeftColumn);
            HardHorizontalSpace(_styling.LeftColumnMargin);

            if (closeRow)
            {
                EditorGUILayout.EndHorizontal();
            }
        }
        
        protected virtual void HeaderLabel(string text, bool useColon = true, int initialSpacing = 4, bool closeRow = true)
        {
            HardVerticalSpace(initialSpacing);
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(text + (useColon ? ":" : ""), _styling.Style_Header, _styling.Layout_Header);

            if (closeRow)
            {
                EditorGUILayout.EndHorizontal();
            }
        }

        public static void DrawEnumPopup<TEnumType>
        (
            ref SerializedProperty property,
            bool closeRow = true
        )
            where TEnumType : struct, Enum
        {
            DrawEnumPopup<TEnumType>(ref property, null, closeRow);
        }

        public static void DrawEnumPopup<TEnumType>
        (
            ref SerializedProperty property,
            string label = null,
            bool closeRow = true
        )
            where TEnumType : struct, Enum
        {
            InitEditorStyling();
            // Get the natural enum from the prop
            int enumIndex = property.enumValueIndex;
            TEnumType enumVal = (TEnumType)Enum.ToObject(typeof(TEnumType), enumIndex);
            // Parse into Enum container
            Enum enumContainer = Enum.Parse<TEnumType>(enumVal.ToString());
            // Begin the layout row
            Rect rowRect = EditorGUILayout.BeginHorizontal();
            // Add the left column label
            if (label != null)
            {
                LeftColumnLabel(label);
            }

            // Add the Enum Popup Control using the Enum container
            enumContainer = EditorGUILayout.EnumPopup(enumContainer, _styling.Layout_PopupStatic);
            // End the layout Row
            if (closeRow)
            {
                EditorGUILayout.EndHorizontal();
            }

            // Get the name of the enum from the Enum container
            string typeEnumName = enumContainer.ToString();
            // Parse the enum name back into the natural enum type
            enumIndex = (int)Enum.Parse(typeof(TEnumType), typeEnumName);
            // Assign the natural enum value back to the prop, cast as int
            property.enumValueIndex = enumIndex;
        }

        protected void DrawCustomFoldoutEditor
        (
            ref SerializedProperty propBool_isOpen, 
            string title, 
            Object target,
            GUILayoutOption[] layoutOptions = default
        )
        {
            DrawCustomFoldout
            (
                ref propBool_isOpen,
                title,
                () =>
                {
                    HardVerticalSpace(2);
                    Editor e = CreateEditor(target);
                    //Debug.Log("Drawing Nested Foldout");
                    e.OnInspectorGUI();
                    (target as IValidatable<object>).Validate();
                },
                layoutOptions
            );
        }
        
        protected void DrawCustomFoldout
        (
            ref SerializedProperty propBool_isOpen, 
            string title, 
            Action foldoutBody,
            GUILayoutOption[] layoutOptions = default
        )
        {
            if (propBool_isOpen == null)
            {
                Debug.Log("null prop");
                return;
            }

            propBool_isOpen.boolValue = DrawCustomFoldout
            (
                propBool_isOpen.boolValue,
                title,
                foldoutBody,
                layoutOptions
            );
        }

        
        
        protected bool Button
        (
            string label, 
            Action action = null, 
            GUIStyle s = default, 
            int height = _kDEFAULT_BUTTON_HEIGHT,
            GUILayoutOption[] layout = null,
            Texture2D icon = null
        )
        {
            return Button(new GUIContent(label), action, s, height, layout, icon);
        }
        
        

        public static int INDENT = 8;
        public static int IndentBy(int amount) => amount * INDENT;

        public static Rect IndentRectBy(Rect rect, int amount)
        {
            int i = IndentBy(amount);
            rect.width -= i;
            rect.x += i;
            return rect;
        }

        public static Rect InflateRectBy(Rect rect, Vector2 amount)
        {
            rect.width += amount.x * 2;
            rect.x -= amount.x;
            rect.height += amount.y * 2;
            rect.y -= amount.y;
            return rect;
        }

        public static Rect InflateRectBy(Rect rect, float width, float height)
        {
            rect.width += width * 2;
            rect.x -= width;
            rect.height += height * 2;
            rect.y -= height;
            return rect;
        }

        public static Rect InflateRectBy(Rect rect, int left, int right, int top, int bottom)
        {
            RectOffset o = new RectOffset(left, right, top, bottom);
            return o.Add(rect);
        }

        public static Rect InflateRectBy(Rect rect, RectOffset o)
        {
            return o.Add(rect);
        }
        

        protected bool ToggleButton
        (
            bool state,
            string label, 
            Func<bool, bool> action = null, 
            Func<Rect, Rect> customHeader = null, 
            int indentLevel = 0,
            GUIStyle s = default,
            Hue normalHue = Hue.White,
            Hue activeHue = Hue.White,
            Shade normalShade = Shade.GraySemiLight,
            Shade activeShade = Shade.GraySemiDark,
            int height = _kDEFAULT_BUTTON_HEIGHT,
            GUILayoutOption[] layout = null,
            Texture2D icon = null
        )
        {
            return ToggleButton
            (
                state, 
                new GUIContent(label), 
                Colors.GetColor(normalHue, normalShade),
                Colors.GetColor(activeHue, activeShade),
                action, 
                customHeader, 
                indentLevel, 
                s,
                height, 
                layout, 
                icon
            );
        }

        protected bool ToggleButton
        (
            bool state,
            GUIContent content, 
            Color normalColor,
            Color activeColor,
            Func<bool, bool> action = null, 
            Func<Rect, Rect> customHeader = null, 
            int indentLevel = 0,
            GUIStyle s = default,
            int height = _kDEFAULT_BUTTON_HEIGHT,
            GUILayoutOption[] layout = null,
            Texture2D customBackground = null
        )
        {
            if (layout == null) layout = Array.Empty<GUILayoutOption>();

            Color bg = GUI.backgroundColor;
            GUI.backgroundColor = !state ? normalColor : activeColor;
            bool result = state;

            Rect r;
            if (s != default)
            {
                r = EditorGUILayout.GetControlRect(false, height, s, layout);
                r = IndentRectBy(r, indentLevel); 
                if (customHeader != null)
                    r = customHeader(r);
                
                if (action != null)
                {
                    if (GUI.Button(r, content, s))
                    {
                        result = action(state);
                    }
                }
                else
                {
                    result = (GUI.Button(r, content, s)) ? !result : result;
                }
            }
            else
            {
                r = EditorGUILayout.GetControlRect(false, height, layout);
                r = IndentRectBy(r, indentLevel);
                if (customHeader != null)
                    r = customHeader(r);
                
                if (action != null)
                {
                    if (GUI.Button(r, content, s))
                    {
                        result = action(state);
                    }
                }
                else
                {
                    result = GUI.Button(r, content, s) ? !result : result;
                }
            }

            GUI.backgroundColor = bg;
            return result;
        }

        

        protected bool Button
        (
            GUIContent content,
            Action action = null,
            GUIStyle s = default,
            int height = _kDEFAULT_BUTTON_HEIGHT,
            GUILayoutOption[] layout = null,
            Texture2D icon = null,
            bool truncateLabel = false
        )
        {
            if (layout == null) layout = Array.Empty<GUILayoutOption>();

            Rect r;
            if (s != default)
            {
                r = EditorGUILayout.GetControlRect(false, height, s, layout);
                if (truncateLabel && r.width < height * 2f)
                {
                    content.text = string.Empty;
                    s.imagePosition = ImagePosition.ImageOnly;
                    r.width = height;
                }

                if (action != null)
                {
                    if (GUI.Button(r, content, s))
                    {
                        if (action != null) action();
                        return true;
                    }

                    return false;
                }

                return (GUI.Button(r, content, s));
            }
            else
            {
                r = EditorGUILayout.GetControlRect(false, height, layout);

                if (action != null)
                {
                    if (GUI.Button(r, content))
                    {
                        if (action != null) action();
                        return true;
                    }

                    return false;
                }
                return (GUI.Button(r, content));
            }
        }


        protected Rect _r;
        protected const int _kDEFAULT_BUTTON_HEIGHT = 24;
        protected GUIStyle _s;
        protected GUILayoutOption[] _o;
        protected Color _color_lightGray;



        protected virtual void InitDefaults()
        {
            _s = new GUIStyle(GUI.skin.button);
            _o = Array.Empty<GUILayoutOption>();
            _s.fixedHeight = _kDEFAULT_BUTTON_HEIGHT;
            _color_lightGray = (Color.white + Color.gray) * 0.5f;
        }


        protected bool DrawCustomFoldout
        (
            bool isOpen, 
            string title, 
            Action foldoutBody,
            GUILayoutOption[] layoutOptions = default
        )
        {
            Texture2D t = AssetDatabase.GetBuiltinExtraResource<Texture2D>("UI/Skin/DropdownArrow.psd");
            const int h = 22;
            GUILayoutOption[] o = new[]
            {
                GUILayout.ExpandWidth(true),
                GUILayout.MinWidth(64),
                GUILayout.MaxHeight(h)
            };

            if (layoutOptions == default)
            {
                layoutOptions = o;
            }

            GUIContent c = new GUIContent(title);
            //c.text = $"{(isOpen ? Emoji.TRIANGLE_DOWN : Emoji.TRIANGLE_RIGHT)}  {title}";
            c.text = $"{(isOpen ? "▽" : "▶︎")}  {title}";
            
            GUIStyle s = new GUIStyle(GUI.skin.button);
            s.alignment = TextAnchor.MiddleLeft;
            s.imagePosition = ImagePosition.ImageLeft;
            
            s.fixedHeight = h;
            if (isOpen)
            {
                s.normal = s.active;
                //s.normal.background = TextureUtils.VerticalRamp2D(22, Color.gray, Color.black);
                //s.border = GUI.skin.toggle.border;
                //GUI.skin.button.border.Remove(new Rect(-16f, -16f, 32f, 32f));
            }

            s.fontStyle = FontStyle.Bold;
            
            s.normal.textColor = isOpen ? _styling.DefaultLabelColorSelected : _styling.DefaultLabelColor;
            s.normal.background = isOpen ? _styling.Ramp_grayGlossPressed22 : _styling.Ramp_grayGlossNormal22;
            s.stretchWidth = true;
            
            
            bool clicked = false;
            clicked = GUILayout.Button(c, s, layoutOptions);

            if (clicked)
            {
                isOpen = !isOpen;
            }
            //s.fixedHeight = false;
            
            

            if (isOpen && foldoutBody != null)
            {
                s = new GUIStyle(GUI.skin.box);
                s.stretchWidth = true;

                //EditorGUILayout.BeginVertical(s, layoutOptions);
                foldoutBody();
                //EditorGUILayout.EndVertical();
            }


            return isOpen;
        }
    }
}