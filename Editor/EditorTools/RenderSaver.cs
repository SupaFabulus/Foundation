#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SupaFabulus.Dev.Foundation.EditorTools.Common
{
    [Serializable]
    public class RenderSaver : EditorWindow
    {
        
        [SerializeField]
        private static readonly string[] _titles = new string[] {
            "RenderSaver",
            "Null",
            "Adobe Photoshop",
            "Porno Generator",
            "BigPipi",
            "PoopLunch",
            "Ass Inspector",
            "Cannabis Calibrator",
            "Just Makes Farts...",
            "Who Cares?",
            "I'm a Doctor Too...",
            "For Your Health!",
            "Brown One",
            "Brown Two",
            "Dr. Drongey Brunger",
            "Prinkles",
            "BOBODDY"
        };

        [MenuItem("Window/Buganamo/MainFrame/RenderSaver")]
        private static void ShowWindow()
        {
            string[] titles = _titles;
            int count = titles.Length;
            float rnd = Random.value;
            int index = Mathf.FloorToInt(rnd * count);
            string title = titles[index];
            
            RenderSaver window = CreateInstance<RenderSaver>();
            
            window.titleContent = new GUIContent("RenderSaver");
            window.Show();
        }
    }
}
#endif