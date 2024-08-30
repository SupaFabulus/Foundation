#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using SupaFabulus.Dev.Foundation.EditorTools.Common;
using UnityEditor;
using UnityEngine;

namespace SupaFabulus.Dev.Foundation.EditorTools.ProjectSearch
{
    [Serializable]
    [CustomEditor(typeof(NamespaceProvider))]
    public class NamespaceProviderEditor : EditorBase
    {

        private NamespaceProvider _ns;
        private Tuple<bool, string> predictedNamespace = default;
        private string _lastPrediction = null;
        private int _lastCount = 0;
        private int _matchCount;
        private int _matchID = -1;
        
        private SerializedObject obj;
        private SerializedProperty _pTargetNamespace;
        private SerializedProperty _pPossibleMatches;
        private string _tgtNs;
        private SerializedProperty _pSelectedID;
        private int _selectedID;

        private Rect r;
        [SerializeField]
        private List<string> _m;
        private GUILayoutOption[] _noLayout;

        private string[] _names = Array.Empty<string>();

        public override void OnInspectorGUI()
        {
            _noLayout = Array.Empty<GUILayoutOption>();
            obj = serializedObject;
            _ns = NamespaceProvider.ß;
            
            InitDefaults();
            InitEditorStyling();

            _pTargetNamespace = obj.FindProperty("_targetNamespace");
            _pSelectedID = obj.FindProperty("_selectionID");
            _pPossibleMatches = obj.FindProperty("_possibleMatches");

            if
            (
                _ns == null || 
                _pTargetNamespace == null ||
                _pSelectedID == null ||
                _pPossibleMatches == null
            )
            {
                ErrorLabel($"Namespace Provider: {_ns}," +
                           $" Target Property: {_pTargetNamespace}" +
                           $" SelectionID: {_selectedID}");
                return;
            }

            /*
            if (_selectedID != _pSelectedID.intValue)
            {
                //_tgtNs = _ns.PossibleMatches[_ns.SelectionID];
            }
            else
            {
                _tgtNs = _pTargetNamespace.stringValue;
            }
            */
            _tgtNs = _pTargetNamespace.stringValue;
            _tgtNs = WideLabeledTextField("Namespace!", _tgtNs, false, 0, false, 2f, 2f);

            if (_tgtNs != _pTargetNamespace.stringValue)
            {
                _ns.TargetNamespace = _tgtNs;
                _pTargetNamespace.stringValue = _tgtNs;
                _ns.Refresh();
                _m = _ns.PossibleMatches;
                //obj.ApplyModifiedProperties();
                _matchID = _ns.MatchID;

                bool hasMatches = _ns.PossibleMatches != null;
                _m = hasMatches ? _m : new (){"FART!"};
                GUI.enabled = hasMatches;

                int i;
                int c = _ns.PossibleMatches.Count;
                string n;
                _names = new string[c];
                Debug.Log($"Count: [{_ns.PossibleMatches.Count}]");

                for (i = 0; i < c; i++)
                {
                    n = _ns.PossibleMatches[i];
                    //Debug.Log($"Name: [{n}]");
                    _names[i] = n;
                }
            }

            Debug.Log($"SIZE: {_names.Length} / {_ns.PossibleMatches.Count}");
            Rect r = EditorGUILayout.GetControlRect(true, 20, _o);
            int s = EditorGUI.Popup
            (
                r,
                "Available?:",
                _pSelectedID.intValue,
                _names,
                GUI.skin.GetStyle("Popup")
            );
            _selectedID = s;
            GUI.enabled = true;

            if (_selectedID != _pSelectedID.intValue && _ns.PossibleMatches[_ns.SelectionID] == _ns.ExactMatch)
            {
                //_tgtNs = _ns.PossibleMatches[_ns.SelectionID];
                //_pSelectedID.intValue = _selectedID;
            }

            _pTargetNamespace.stringValue = _tgtNs;
            obj.ApplyModifiedProperties();
            SaveChanges();

            /*
            r = EditorGUILayout.GetControlRect(false, 20, Array.Empty<GUILayoutOption>());
            bool validate = GUI.Button(r, "Validate Namespace", GUI.skin.button);
            
            if (validate)
            {
                _ns.Refresh();
            }
            */
        }
        
        private string WideLabeledTextField
        (
            string label, 
            string content,
            bool showSelection = false,
            int selectionLength = 0,
            bool autoLayout = true, 
            float spaceBefore = 0f, 
            float spaceAfter = 0f
        )
        {
            const float w = 128f;
            const float h = 20f;
            
            GUIStyle s = new GUIStyle(GUI.skin.label);
            GUILayoutOption[] o = new []
            {
                GUILayout.MinWidth(w), GUILayout.MinHeight(h),
                GUILayout.MaxHeight(h)
            };
            
            //s.normal.textColor = Color.gray;

            if(spaceBefore != 0f) EditorGUILayout.Space(spaceBefore);
            if (autoLayout) EditorGUILayout.BeginVertical();
            EditorGUILayout.LabelField(label, s, o);
            s = new GUIStyle(GUI.skin.textField);
            Rect r = EditorGUILayout.GetControlRect(false, h, s, o);
            if (showSelection)
            {
                s.DrawWithTextSelection(r, new GUIContent(content), -1, 0, selectionLength);
            }
            string result = GUI.TextField(r, content, s);
            if (autoLayout) EditorGUILayout.EndVertical();
            if(spaceAfter != 0f) EditorGUILayout.Space(spaceAfter);

            return result;
        }

    }
}
#endif