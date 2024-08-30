#if UNITY_EDITOR
using System;
using SupaFabulus.Dev.Foundation.Utils;
using UnityEditor;
using UnityEngine;

namespace SupaFabulus.Dev.Foundation.EditorTools.Common
{
    public abstract class AbstractUtilityWindow<TSettings, TSettingsEditor> : EditorWindowBase
        where TSettings : SettingsAsset<TSettings>
        where TSettingsEditor : EditorBase
    {
        protected TSettings _settings;
        protected bool _settingsIsOpen = false;



        protected abstract void CacheSettingsReference();
        
        protected virtual void OnGUI()
        {
            InitEditorStyling();
            InitDefaults();
            CacheSettingsReference();
            if (_settings != null)
            {
                DrawInlineSettingsUIFoldout();
                //Debug.Log($"Settings: {_settings.GetType().Name}");
            }
            else
            {
                Debug.Log($"Settings not found!");
            }
        }

        protected virtual void Test()
        {
            Debug.Log($"Testing: {_settings}");
        }

        protected void DrawInlineSettingsEditor()
        {
            if (_settings != null)
            {
                Editor e = Editor.CreateEditor(_settings, typeof(TSettingsEditor));
                if (e != null)
                {
                    e.OnInspectorGUI();
                }
                else
                {
                    Debug.Log($"Unable to create editor for Settings!");
                }
                //Debug.Log($"Settings Default Path: {settings.RootMakerAssetsFolderPath}");
            }
            else
            {
                Debug.Log($"Settings Not Found!");
            }
        }
        
        

        protected void DrawInlineSettingsUIFoldout()
        {
            if (_settings == null)
            {
                Debug.LogWarning("Settings not found!");
                return;
            }

            _settingsIsOpen = EditorGUILayout.BeginFoldoutHeaderGroup(_settingsIsOpen, $"Settings Asset ({typeof(TSettings).Name})");
            if (_settingsIsOpen)
            {
                DrawInlineSettingsEditor();
            }
            EditorGUILayout.EndFoldoutHeaderGroup();
        }

        protected bool DrawFoldoutEditor<TEditor>
        (
            bool isOpen,
            string title,
            UnityEngine.Object target,
            GUILayoutOption[] layoutOptions = default
        )
            where TEditor : Editor
        {
            Texture2D t = AssetDatabase.GetBuiltinExtraResource<Texture2D>("UI/Skin/DropdownArrow.psd");

            GUILayoutOption[] o = new[]
            {
                GUILayout.ExpandWidth(true),
                GUILayout.MinWidth(64),
                GUILayout.MaxHeight(22)
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
            
            s.fixedHeight = 22;
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
            
            

            if (isOpen && target != null)
            {
                s = new GUIStyle(GUI.skin.box);
                s.stretchWidth = true;

                EditorGUILayout.BeginVertical(s, layoutOptions);
                
                TEditor e = (TEditor)Editor.CreateEditor(target, typeof(TEditor));
                if (e != null)
                {
                    e.OnInspectorGUI();
                }

                EditorGUILayout.EndVertical();
            }


            return isOpen;
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

            GUILayoutOption[] o = new[]
            {
                GUILayout.ExpandWidth(true),
                GUILayout.MinWidth(64),
                GUILayout.MaxHeight(22)
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
            
            s.fixedHeight = 22;
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
#endif