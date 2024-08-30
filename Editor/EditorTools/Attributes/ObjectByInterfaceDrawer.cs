using System;
using UnityEngine;
using UnityEditor;

namespace SupaFabulus.Dev.Foundation.EditorTools.Attributes
{
    [CustomPropertyDrawer(typeof(ObjectByInterfaceAttribute), true)]
    public class ObjectByInterfaceDrawer : PropertyDrawer
    {
        public override void OnGUI
        (
            Rect position,
            SerializedProperty property,
            GUIContent label
        )
        {
            bool dirty = false;
            
            if (attribute is ObjectByInterfaceAttribute attr)
            {
                if (attr == null) return;
                
                UnityEngine.Object obj = property.objectReferenceValue;

                string id = (attr.ObjectType != null && attr.InterfaceType != null)
                    ? $"{attr.ObjectType.Name} : {attr.InterfaceType.Name}"
                    : $"[null]";
                
                UnityEngine.Object val = EditorGUI.ObjectField
                (
                    position,
                    $"{label} {id})",
                    obj,
                    typeof(UnityEngine.Object),
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

                Type i;
                GameObject gObj = (val as GameObject);

                if (gObj != null)
                {
                    Debug.Log($"GameObject");
                    Component c = gObj.GetComponent(attr.InterfaceType);
                    if (c != null)
                    {
                        obj = c;
                        dirty = true;
                    }
                }
                else
                {
                    Debug.Log($"Other");
                    i = property.GetType().GetInterface(nameof(attr.InterfaceType));

                    if (i != null)
                    {
                        Debug.Log($"Interface: {i}");
                        Debug.Log($"Interface: {i.Name}");
                        obj = val;
                        dirty = true;
                    }
                    else
                    {
                        Debug.Log($"Clearing Value...");
                        obj = null;
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