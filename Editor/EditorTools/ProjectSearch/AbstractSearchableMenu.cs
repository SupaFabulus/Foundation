#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SupaFabulus.Dev.Foundation.Structures;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace SupaFabulus.Dev.Foundation.EditorTools.ProjectSearch
{
    public class TypeTree : StringTree<Type> {}

    public abstract class AbstractSearchableMenu<TAttribute>
        where TAttribute : Attribute, ISearchableAttribute
    {

        protected string _baseNamespace = null;
        protected bool _includeBaseNamespace = false;
        protected string _title = "Search";

        public AbstractSearchableMenu
        (
            string searchMenuTitle,
            bool includeBaseNamespace = false, 
            string baseNamespace = null
        )
        {
            _baseNamespace = baseNamespace;
            _includeBaseNamespace = includeBaseNamespace;
            _title = searchMenuTitle;
        }

        private TypeTree _types = new TypeTree();
        
        private void ExecuteSearchSelection(Type type)
        {
            EditorApplication.ExecuteMenuItem("Assets/Create/" + type.GetCustomAttribute<CreateAssetMenuAttribute>().menuName);
        }
        
        public virtual void ExecuteMenuItem()
        {
            
            TypeCache.TypeCollection tc = TypeCache.GetTypesWithAttribute<CreateAssetMenuAttribute>();
            foreach (Type type in tc)
            {
                if (type != null)
                {
                    IEnumerable<AbstractSearchableAttribute> attrs = type.GetCustomAttributes<AbstractSearchableAttribute>();
                    Debug.Log($"Attributes Collection: [{attrs}]");
                    Debug.Log($"Type: [{type}]");
                    if
                    (
                        (attrs != null && attrs.Any()) ||
                        _includeBaseNamespace &&
                        _baseNamespace != null &&
                        _baseNamespace != default &&
                        (type.GetCustomAttribute<CreateAssetMenuAttribute>().menuName != null && 
                         type.GetCustomAttribute<CreateAssetMenuAttribute>().menuName.Contains(_baseNamespace))
                    )
                    {
                        string name = type.GetCustomAttribute<CreateAssetMenuAttribute>().menuName;

                        int i = name.LastIndexOf('/');
                        name = (i == -1) ? name : name.Substring(0, i + 1) + type.Name;
                        _types.Insert(name, type, 1);
                    }
                }
            }
             
            
            /*
            foreach (var type in TypeCache.GetTypesWithAttribute<CreateAssetMenuAttribute>()
                .Where(t => 
                t.GetCustomAttributes<AbstractSearchableAttribute>(true).Any() ||
                (
                    _includeBaseNamespace && 
                    _baseNamespace != null && 
                    _baseNamespace != default &&
                    t.GetCustomAttribute<CreateAssetMenuAttribute>().menuName.Contains(_baseNamespace)
                )))
            {
                string name = type.GetCustomAttribute<CreateAssetMenuAttribute>().menuName;

                int i = name.LastIndexOf('/');
                name = (i == -1) ? name : name.Substring(0, i + 1) + type.Name;
                _types.Insert(name, type, 1);
            }
            */
            
            var projectBrowserType = typeof(Editor).Assembly.GetType("UnityEditor.ProjectBrowser");
            EditorWindow projectBrowser = EditorWindow.GetWindow(projectBrowserType);

            Vector2 xy = new Vector2
            (
                projectBrowser.position.x + 10, 
                projectBrowser.position.y + 60
            );
            
            Vector2 minSize = new Vector2
            (
                projectBrowser.position.width - 20, 
                projectBrowser.position.height - 80
            );

            SearchTypeDropdown<TAttribute> dropdown = new SearchTypeDropdown<TAttribute>
                (
                    new AdvancedDropdownState(), 
                    _types,
                    ExecuteSearchSelection,
                    _title
                )
                { MinimumSize = minSize };

            var rect = new Rect(xy.x, xy.y, projectBrowser.position.width - 20, projectBrowser.position.height);

            dropdown.Show(new Rect()); // don't use this to position the
            var window = typeof(SearchTypeDropdown<TAttribute>).GetField
            (
                "m_WindowInstance", 
                BindingFlags.Instance | BindingFlags.NonPublic
            )?.GetValue(dropdown) as EditorWindow;
            
            window.position = rect;
        }
    }
}
#endif