using System;
using SupaFabulus.Dev.Foundation.EditorTools.Common;
using SupaFabulus.Dev.Foundation.Utils;
using UnityEditor;
using UnityEngine;

namespace SupaFabulus.Dev.EditorTools.Components
{
    [Serializable]
    [CustomEditor(typeof(SingleChildSelector))]
    public class SingleChildSelectorEditor : EditorBase
    {
        private SingleChildSelector tgt;
        
        public override bool RequiresConstantRepaint() => 
            HasTarget && tgt.keepInspectorUpdated;
        

        private bool HasTarget
        {
            get
            {
                if(tgt == null) tgt = target as SingleChildSelector;
                return tgt != null;
            }
        }

        private void SetAllVisible(bool visible)
        {
            if (!HasTarget || tgt.Container == null) return;
            Transform c;
            Transform p = tgt.Container;
            int i;
            int count = p.childCount;
            for (i = 0; i < count; i++)
            {
                c = p.GetChild(i);
                c.gameObject.SetActive(visible);
            }
        }

        private void ShowAll() => SetAllVisible(true);
        private void HideAll() => SetAllVisible(false);

        public override void OnInspectorGUI()
        {
            InitDefaults();
            InitEditorStyling();
            DrawDefaultInspector();

            if (!HasTarget) return;

            EditorGUILayout.BeginVertical();
            
            if (tgt == null)
            {
                ErrorLabel("Unable to find target for SingleChildSelector!");
                EditorGUILayout.EndVertical();
                return;
            }

            int i;
            int c;
            int selection = tgt.SelectedChildIndex;
            
            Rect r;
            Color buttonColor = Color.gray;
            Color buttonColorSelected = Color.gray.Mix(Color.black);
            GUIStyle btnStyle = new GUIStyle(GUI.skin.button);
            GUIStyle btnStyleSelected = new GUIStyle(GUI.skin.button);
            btnStyleSelected.fontStyle = FontStyle.Bold;
            
            btnStyle.alignment = TextAnchor.MiddleLeft;
            btnStyleSelected.alignment = TextAnchor.MiddleLeft;
            

            Color originalBG = GUI.backgroundColor;
            
            Transform tx;
            Transform child;
            Transform selectedChild = tgt.SelectedChild;

            GUIStyle s;
            
            bool selected = false;

            tx = tgt.Container;
            c = tx.childCount;

            if (c < 1)
            {
                ErrorLabel("No children found.");
                EditorGUILayout.EndVertical();
                return;
            }


            GUI.enabled = selectedChild != null;
            Rect btnRect = EditorGUILayout.GetControlRect
            (
                false, 20, 
                new GUIStyle(GUI.skin.button),
                new[]
                {
                    GUILayout.MinWidth(80),
                    GUILayout.MaxWidth(128),
                    GUILayout.ExpandWidth(true)
                }
            );

            if (GUI.Button(btnRect, "Clear Selection"))
            {
                tgt.SelectedChildIndex = -1;
                selectedChild = null;

            }

            GUI.enabled = true;

            EditorGUILayout.BeginHorizontal();
            Label("All:");
            Button("Show", ShowAll);
            Button("Hide", HideAll);
            EditorGUILayout.EndHorizontal();
            
            if (selectedChild != null)
            {
                if (selectedChild.parent != tgt.transform)
                {
                    EditorGUILayout.BeginHorizontal();
                    GUI.backgroundColor = Color.gray.Mix(Color.green).Mix(Color.cyan);
                    int h = 24;
                    
                    r = EditorGUILayout.GetControlRect(false, h, btnStyleSelected);
                    if (GUI.Button(r, selectedChild.name, btnStyleSelected))
                    {
                        selectedChild.SetParent(tgt.transform);
                        tgt.SelectedChildIndex = -1;
                        EditorGUILayout.EndHorizontal();
                        return;
                    }
                    
                    EditorGUILayout.EndHorizontal();
                }
            }

            c = tx.childCount;
            EditorGUILayout.Space(8);

            

            for (i = 0; i < c; i++)
            {
                
                //_newName = null;
                
                
                child = tx.GetChild(i);
                if (child != null)
                {
                    
                    selected = selection == i;
                    s = selected ? btnStyleSelected : btnStyle;

                    
                    EditorGUILayout.BeginHorizontal();
                    
                    int h = 24;
                    int bw = h * 2;
                    GUIStyle sMain = new GUIStyle(_styling.ButtonStyle_Glass);
                    

                    bool active = child.gameObject.activeSelf;
                    Color bg = GUI.backgroundColor;
                    GUI.backgroundColor = active 
                        ? (Color.red + Color.magenta + Color.gray) / 3f
                        : (Color.green + Color.cyan + Color.gray) / 3f;
                    string l = active ? "Hide" : "Show";
                    Rect src = EditorGUILayout.GetControlRect(false, h, sMain);
                    //r.x += r.width - bw;
                    r = src;
                    r.width = bw;
                    if (GUI.Button(r, l, sMain))
                    {
                        child.gameObject.SetActive(!active);
                    }
                    
                    GUI.backgroundColor = selected ? buttonColorSelected : buttonColor;
                    r = src;
                    r.x += bw;
                    r.width -= bw;
                    
                    
                    Event e = Event.current;
                    EventType t = e.type;
                    
                    /*
                    if 
                    (
                        t == EventType.MouseDown && 
                        _activeRenameSelection >= 0 && 
                        _activeRenameSelection < tgt.Container.childCount
                    )
                    {
                        _activeRenameSelection = -1;
                    }
                    */

                    if (i == _activeRenameSelection)
                    {
                        child.gameObject.name = GUI.TextField(r, child.gameObject.name);
                        bool clear = false;
                    
                        if (e.keyCode == KeyCode.KeypadEnter || e.keyCode == KeyCode.Return)
                        {
                            if (!string.IsNullOrWhiteSpace( child.gameObject.name) && !string.IsNullOrEmpty( child.gameObject.name))
                            {
                                child.gameObject.name = child.gameObject.name;
                            }

                            clear = true;
                        }
                        else if (e.keyCode == KeyCode.Escape)
                        {
                            clear = true;
                        }

                        if (clear)
                        {
                            _activeRenameSelection = -1;
                            Repaint();
                        }
                    }
                    else
                    {
                        GUI.backgroundColor = child.gameObject.activeSelf
                            ? Color.grey
                            : Color.grey.Mix(Color.black).Mix(Color.gray);
                        if (GUI.Button(r, child.name, s))
                        {
                            switch (e.button)
                            {
                                case 0:
                                    _activeRenameSelection = -1;
                                    if (tgt.SelectedChildIndex == i)
                                    {
                                        if (tgt.SelectedChild != null && tgt.SelectedChild.parent == tgt.Container)
                                        {
                                            tgt.SelectedChild.gameObject.SetActive(false);
                                            tgt.SelectedChildIndex = -1;
                                        }
                                    }
                                    else
                                    {
                                        tgt.SelectedChildIndex = i;
                                    }
                                    break;
                                
                                case 1:
                                    _activeRenameSelection = i;
                                    child = null;
                                    Repaint();
                                    break;
                                case 2:
                                    child.gameObject.SetActive(!child.gameObject.activeSelf);
                                    break;
                                default:
                                    break;
                            }
                        }
                    }

                    

                    EditorGUILayout.EndHorizontal();
                    child = null;
                }
                child = null;
            }
            
            
            
            
            
            
            EditorGUILayout.EndVertical();
            child = null;
        }

        //private string _newName;
        private int _activeRenameSelection = -1;
    }
}