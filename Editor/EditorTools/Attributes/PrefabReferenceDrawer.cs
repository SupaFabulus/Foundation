using System;
using System.Text;
using UnityEditor;
using UnityEngine;
//using UnityEngine.AddressableAssets;

namespace SupaFabulus.Dev.Foundation.EditorTools.Attributes
{
    
    
    [CustomPropertyDrawer(typeof(PrefabReferenceAttribute), true)]
    public class PrefabReferenceDrawer : PropertyDrawer
    {
        public override void OnGUI
        (
            Rect position,
            SerializedProperty property,
            GUIContent label
        )
        {
            bool dirty = false;
            
            Debug.Log($"Property Type: {property.type}");
            
            if (attribute is PrefabReferenceAttribute attr)
            {
                UnityEngine.Object obj = null; // = property.objectReferenceValue;
                int instanceID = property.objectReferenceInstanceIDValue;
                
                /*
                AsyncOperationHandle<GameObject> h = Addressables.LoadAssetAsync<GameObject>()
                AssetReferenceGameObject assetRef = property.boxedValue as AssetReferenceGameObject;
                Type fType = assetRef.GetType();
                if (fType != typeof(AssetReferenceGameObject))
                {
                    EditorGUILayout.LabelField("Invalid Attribute Usage");
                    return;
                }
                */
                
                Type[] types = attr.ComponentTypes;
                Type t;
                int i;
                int c = types.Length;
                string constraintLabel = null;
                StringBuilder b = new();
                bool invalid = false;

                
                for (i = 0; i < c; i++)
                {
                    t = types[i];
                    if (t != null)
                    {
                        b.Append(t.Name);
                        if (i < c - 1)
                        {
                            b.Append(", ");
                        }
                        
                        //invalid |= 
                    }

                        
                }
                
                EditorGUI.PropertyField(position, property, label, true);

                string id = ""; //(attr.ComponentType != null)
                  //  ? $"{attr.ComponentType.Name}"
                  //  : $"[null]";
                
                GameObject val = (GameObject)EditorGUI.ObjectField
                (
                    position,
                    $"{label} {id})",
                    obj,
                    typeof(GameObject),
                    true
                );

                if (val == obj)
                {
                    if (val != null)
                    {
                        UnityEditor.Editor.CreateEditor(val);
                    }

                    return;
                }

                GameObject gObj = (val as GameObject);

                if (gObj != null)
                {
                    Debug.Log($"GameObject");
                    Component component = null; //gObj.GetComponent(attr.ComponentType);
                    if (component != null)
                    {
                        obj = component;
                        dirty = true;
                    }
                }
                
                if (dirty)
                {
                    
                    Debug.Log($"Set");
                    property.objectReferenceValue = obj;
                    property.serializedObject.ApplyModifiedProperties();
                    property.serializedObject.Update();
                }
                else
                {
                    Debug.Log($"Interface not found");
                }
            }
            else
            {
                Debug.Log($"Invalid Attribute Type: {attribute}");
            }
            GUIUtility.ExitGUI();
        }                  
    }
}