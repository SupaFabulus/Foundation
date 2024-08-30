#if UNITY_EDITOR
using System;
using System.Linq.Expressions;
using UnityEditor;
using UnityEngine;

namespace SupaFabulus.Dev.Foundation.EditorTools.Common
{
    [Serializable]
    abstract public class EditorWindowBase : EditorWindow
    {
        public delegate void ButtonDelegate<TTargetType>(ref TTargetType target);
        
        protected const string _kHEADER = "Buganamo/";

        protected GUILayoutOption[] _noLayout = new[] {GUILayout.ExpandWidth(true)};
        protected Rect _r;
        protected const int _kDEFAULT_BUTTON_HEIGHT = 20;
        protected GUIStyle _s;
        protected GUILayoutOption[] _o;
        protected GUILayoutOption[] _oBtn;
        protected Color _color_lightGray;
        


        protected virtual void InitDefaults()
        {
            _s = new GUIStyle(GUI.skin.button);
            
            _o = Array.Empty<GUILayoutOption>();
            _oBtn = new[]
            {
                GUILayout.Height(_kDEFAULT_BUTTON_HEIGHT),
                GUILayout.MinWidth(64),
                GUILayout.MaxWidth(128),
                GUILayout.ExpandWidth(false)
            };
            _color_lightGray = (Color.white + Color.gray) * 0.5f;

        }
        

        protected static void InitEditorStyling()
        {
            if (_styling == null)
            {
                _styling = new EditorStyling();
                _styling.Reset();
            }
        }
        
        protected Rect LayoutRect(float height, Color textColor, FontStyle fontStyle = FontStyle.Normal)
        {
            GUIStyle s = new GUIStyle(GUI.skin.label);
            s.normal.textColor = textColor;
            s.fontStyle = fontStyle;
            return LayoutRect(height, s);
        }

        protected Rect LayoutRect(float height, GUIStyle style = null)
        {
            if (style == null)
            {
                return EditorGUILayout.GetControlRect(false, height, Array.Empty<GUILayoutOption>());
            }
            else
            {
                return EditorGUILayout.GetControlRect(false, height, style, Array.Empty<GUILayoutOption>());
            }
        }


        protected void Label(string label)
        {
            EditorGUILayout.LabelField(label, GUI.skin.label, _noLayout);
        }
        
        protected static EditorStyling _styling;
        protected static bool _valid = false;

        
        
        protected void ColorLabel(Color color, string msg)
        {
            GUIStyle s = new GUIStyle(GUI.skin.label);
            s.normal.textColor = color;
            s.fontStyle = FontStyle.Bold;
            s.wordWrap = true;
                
            EditorGUILayout.LabelField
            (
                msg,
                s, _o
            );
        }

        protected void ErrorLabel(string msg)
        {
            ColorLabel(Color.red, msg);
        }

        protected void SuccessLabel(string msg)
        {
            ColorLabel(Color.green, msg);
        }

        protected string TextField(string label, string text)
        {
            return EditorGUILayout.TextField(label, text, GUI.skin.textField, _noLayout);
        }

        protected string TextField(string label, string text, Color color)
        {
            GUIStyle s = new GUIStyle(GUI.skin.textField);
            s.normal.textColor = color;
            return EditorGUILayout.TextField(label, text, s, _noLayout);
        }
        
        
        
        

        protected bool Button
        (
            string label, 
            Func<bool> action,
            GUIStyle style = null,
            GUILayoutOption[] layout = null
        )
        {
            if (style == null) style = _s;
            if (layout == null) layout = _oBtn;
            
            Rect r;
            const bool makeRoomForLabel = false;
            const int h = _kDEFAULT_BUTTON_HEIGHT;
            
            r = EditorGUILayout.GetControlRect(makeRoomForLabel, h, style, layout);
            if (GUI.Button(r, label, style)) return action != null
                ? action()
                : true;

            return false;
        }
        
        
        protected bool Button
        (
            string label, 
            Action action, 
            GUIStyle style = null,
            GUILayoutOption[] layout = null
        )
        {
            if (style == null) style = _s;
            if (layout == null) layout = _oBtn;
            
            Rect r;
            const bool makeRoomForLabel = false;
            const int h = _kDEFAULT_BUTTON_HEIGHT;
            
            r = EditorGUILayout.GetControlRect(makeRoomForLabel, h, style, layout);
            if (GUI.Button(r, label, style))
            {
                action();
                return true;
            }

            return false;
        }

        protected void Button<TTarget>
        (
            string label, 
            ButtonDelegate<TTarget> action, 
            ref TTarget value, 
            GUIStyle style = null,
            GUILayoutOption[] layout = null
        )
        {
            if (style == null) style = _s;
            if (layout == null) layout = _oBtn;
            if (GUILayout.Button(label, style, layout)) action(ref value);
        }

        protected void Button<TTarget>
        (
            string outerLabel, 
            string buttonlabel, 
            ButtonDelegate<TTarget> action, 
            ref TTarget value, 
            GUIStyle style = null,
            GUILayoutOption[] layout = null
        )
        {
            if (style == null) style = _s;
            if (layout == null) layout = _oBtn;
            StartH();
            EditorGUILayout.LabelField(outerLabel, GUI.skin.label, _noLayout);
            if (GUILayout.Button(buttonlabel)) action(ref value);
            EndH();
        }

        
        
        
        
        protected void Space(int space = 8)
        {
            EditorGUILayout.Space(space);
        }
        
        public static string GetMemberName<T>(Expression<Func<T>> memberExpression)
        {
            MemberExpression expressionBody = (MemberExpression)memberExpression.Body;
            return expressionBody.Member.Name;
        }
        
        protected void GetterSetter<TTarget>
        (
            ref TTarget target,
            string title,
            Color titleColor,
            ButtonDelegate<TTarget> onGet,
            ButtonDelegate<TTarget> onSet,
            string labelGet = "Get",
            string labelSet = "Set"
        )
        {
            string targetValue = (target != null) ? target.ToString() : "";
            
            StartV();

            StartH();
            TextField(title, targetValue, titleColor);
            EndH();
                
            Space(4);
                
            StartH();
            Button(labelGet, onGet, ref target);
            Button(labelSet, onSet, ref target);
            EndH();

            EndV();
        }
        
        protected virtual GameObject GetSelectedSceneGameObject()
        {
            GameObject gObj = Selection.activeGameObject;

            if (gObj == null)
            {
                //Debug.LogWarning($"Please Select an Unprepared GameObject from the Hierarchy to Prepar an Asset");
                return null;
            }

            string selectionAssetPath = AssetDatabase.GetAssetPath(gObj);

            if (!String.IsNullOrEmpty(selectionAssetPath))
            {
                //Debug.LogError($"This shit already exists at {selectionAssetPath}, you dumbass");
                return null;
            }

            return gObj;
        }
        
        protected bool IsValidString(string str)
        {
            return (str != null && !String.IsNullOrEmpty(str) && !String.IsNullOrWhiteSpace(str));
        }

        protected bool IsValidFolderPath(string path)
        {
            return IsValidString(path) && AssetDatabase.IsValidFolder(path);
        }

        protected bool CreateFolder(string parentFolderPath, string folderName)
        {
            if (!IsValidString(folderName))
            {
                Debug.Log($"Invalid Folder Name!");
                return false;
            }
            
            if (!AssetDatabase.IsValidFolder(parentFolderPath))
            {
                Debug.Log($"Invalid Parent Folder Path!");
                return false;
            }

            string result = AssetDatabase.CreateFolder(parentFolderPath, folderName);
            GUID g = new GUID(result);
            
            return (IsValidString(result) && g != null);
        }

        protected string[] GetSubFolderNames(string parentFolderPath)
        {
            if (!IsValidFolderPath(parentFolderPath))
            {
                Debug.LogError($"Invalid Parent Folder Path: {parentFolderPath}");
                return null;
            }

            string[] paths = AssetDatabase.GetSubFolders(parentFolderPath);
            if (paths == null)
            {
                Debug.LogError($"Unable to find any Subfolders in {parentFolderPath}");
                return null;
            }
            int i;
            int c = paths.Length;
            string p;
            string n;
            string[] names = new string[c];

            for (i = 0; i < c; i++)
            {
                p = paths[i];
                n = p.Substring(p.LastIndexOf('/') + 1);
                names[i] = n;
            }

            return names;
        }

        protected int DrawSubFolderPopupMenu
        (
            int currentIndex, 
            string parentFolderPath,
            string customLabel = "SubFolder"
        )
        {
            int resultIndex;
            int c;
            int lblWidth = 64;
            Rect r;
            string[] subFolderNames = GetSubFolderNames(parentFolderPath);
            c = subFolderNames.Length;
            GUIStyle style = GUI.skin.FindStyle("Popup");
            style = (style == null) ? GUI.skin.button : style;

            Rect row = EditorGUILayout.BeginHorizontal();
            
            Rect inner = EditorGUILayout.GetControlRect(false, 20, style, Array.Empty<GUILayoutOption>());
            r = inner;
            r.width = lblWidth;
            
            EditorGUI.LabelField(r, customLabel, GUI.skin.label);

            r = inner;
            r.x += lblWidth;
            r.width -= lblWidth;
            
            resultIndex = EditorGUI.Popup
            (
                r,
                currentIndex,
                subFolderNames
            );

            EditorGUILayout.EndHorizontal();
            
            return resultIndex;
            //return subFolderNames[resultIndex];
        }

        protected bool Folder(string p) => AssetDatabase.IsValidFolder(p);

        protected void Assign<T>(ref T target, Func<T, T> assigner)
        {
            //assigner(ref target);
        }
        
        protected string DrawSubFolderCreationUI
        (
            string parentFolderPath, 
            string newSubFolder,
            Action onSubFolderCreated = null,
            string subFolderDisplayName = "SubFolder"
        )
        {
            string modifiedSubFolderName;
            GUIStyle style;
            bool folderCreated = false;
            GUILayoutOption[] o = Array.Empty<GUILayoutOption>();

            style = GUI.skin.textField;
            modifiedSubFolderName = EditorGUILayout.TextField
            (
                $"New {subFolderDisplayName} Name:", 
                newSubFolder, 
                style, 
                o
            );

            
            string newDir = parentFolderPath + "/" + newSubFolder;
            //Debug.Log($"{subFolderDisplayName} to Create: {newDir}");
            bool exists = false;

            if (IsValidString(newSubFolder))
            {
                //Debug.Log($"Checking if {subFolderDisplayName} exists...");
                exists = AssetDatabase.IsValidFolder(newDir);
                //Debug.Log($"{subFolderDisplayName} {(exists ? "DOES" : "DOES NOT")} exist");
                
                if (exists)
                {
                    ErrorLabel($"{subFolderDisplayName} Already Exists!");
                }
                else
                {

                    if (GUILayout.Button($"Create New {subFolderDisplayName}"))
                    {
                        folderCreated = CreateFolder(parentFolderPath, newSubFolder);

                        if (folderCreated && onSubFolderCreated != null)
                        {
                            onSubFolderCreated();
                        }
                    }

                    //Debug.Log($"{subFolderDisplayName} was {(folderCreated ? "" : "NOT")} created");
                }
            }
            else
            {
                string lbl = $"Enter a Name to Create a new {subFolderDisplayName}";
                style = new GUIStyle(GUI.skin.label);
                style.alignment = TextAnchor.MiddleCenter;
                style.normal.textColor = Color.gray;
                EditorGUILayout.LabelField(lbl, style, o);
            }

            return modifiedSubFolderName;
        }
        
        protected TComponent FindSingleSceneInstance<TComponent>()
            where TComponent : Component
        {
            string n = nameof(TComponent);
            bool found = false;
            int count = 0;
            TComponent inst = null;
            TComponent[] instances = 
                FindObjectsByType<TComponent>(FindObjectsInactive.Exclude, FindObjectsSortMode.InstanceID);
            count = instances.Length;

            if (count > 0)
            {
                if (count > 1)
                {
                    Debug.LogWarning($"More than one active {n} instance in the current scene!\n" +
                                     $"Please ensure all other {n} instances in the current scene" +
                                     $"(excluding one instance intended for use) are disabled a {n}.");    
                }
                else
                {
                    inst = instances[0];
                    found = inst != null;
                }
            }
            else
            {
                //Debug.LogWarning($"Unable to find any {n} instances in the current scene.");
            }

            return inst;
        }

        protected void StartH() { EditorGUILayout.BeginHorizontal(); }
        protected void StartV() { EditorGUILayout.BeginVertical(); }
        protected void EndH() { EditorGUILayout.EndHorizontal(); }
        protected void EndV() { EditorGUILayout.EndVertical(); }
    }
}
#endif