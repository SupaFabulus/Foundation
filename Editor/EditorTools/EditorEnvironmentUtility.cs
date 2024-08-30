#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace SupaFabulus.Dev.Foundation.EditorTools.Common
{
    public class EditorEnvironmentUtility : EditorWindowBase
    {
        private static bool _locked = false;
        private const string _kMENU_PATH = "/Buganamo/MainFrame/EditorEnvironmentUtility";
        private Action _assemblyLockToggleAction = null; 
        
        [MenuItem("Tools" + _kMENU_PATH)]
        [MenuItem("Window" + _kMENU_PATH)]
        public static void ShowWindow()
        {
            EditorWindow e = GetWindow(typeof(EditorEnvironmentUtility));
            e.titleContent = new GUIContent("EditorEnvironmentUtility");
            e.autoRepaintOnSceneChange = true;
            e.Show(true);
        }

        private GUISkin _skin;

        private void OnGUI()
        {
            _assemblyLockToggleAction = _locked ? UnlockAssemblies : LockAssemblies;
            InitDefaults();
            DrawMainControlStrip();
        }

        private void DrawMainControlStrip()
        {
            string lockLabel = (_locked ? "Unlock" : "Lock") + " Assemblies";
            EditorGUILayout.BeginHorizontal();
            Button("Collect Garbage", GC.Collect);
            Button(lockLabel, _assemblyLockToggleAction);
            EditorGUILayout.EndHorizontal();
            
        }

        private void DrawButtonStyleToggleUI()
        {
            Button("Toggle Button Style", ToggleButtonStyle);
            _skin = (GUISkin)EditorGUILayout.ObjectField
            (
                "Skin",
                _skin,
                typeof(GUISkin),
                this, 
                Array.Empty<GUILayoutOption>()
            );
            GUISkin s = GUI.skin;
            GUI.color = Color.black;
        }

        private void LockAssemblies()
        {
            _locked = true;
            EditorApplication.LockReloadAssemblies();
        }
        private void UnlockAssemblies()
        {
            _locked = false;
            EditorApplication.UnlockReloadAssemblies();
        }

        private GUIStyle ob = null;
        private void ToggleButtonStyle()
        {
            if (ob == null)
            {
                ob = GUI.skin.button;
            }

            if (_skin != null)
            {
                if (GUI.skin.button != _skin.button)
                {
                    GUI.skin.button = _skin.button;
                }
                else
                {
                    GUI.skin.button = ob;
                }
            }
        }
    }
}
#endif