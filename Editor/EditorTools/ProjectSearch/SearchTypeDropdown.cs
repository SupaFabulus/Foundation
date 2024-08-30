#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using SupaFabulus.Dev.Foundation.Structures;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace SupaFabulus.Dev.Foundation.EditorTools.ProjectSearch
{
    public class SearchTypeDropdown<TAttribute> : AdvancedDropdown
        where TAttribute : Attribute
    {
        private const string _kDEFAULT_TITLE = "Search";
        private readonly StringTree<Type> _list;
        private readonly Action<Type> _func;
        private string _title = _kDEFAULT_TITLE;
        private readonly List<Type> _lookup = new List<Type>();

        public Vector2 MinimumSize
        {
            get => minimumSize;
            set => minimumSize = value;
        }

        public SearchTypeDropdown
        (
            AdvancedDropdownState state, 
            StringTree<Type> list, 
            Action<Type> func,
            string customTitle = _kDEFAULT_TITLE
        ) : base(state)
        {
            _list = list;
            _func = func;
            _title = customTitle;
        }

        protected override void ItemSelected(AdvancedDropdownItem item)
        {
            base.ItemSelected(item);
            _func?.Invoke(_lookup[item.id]);
        }

        private void Render(StringTree<Type> tree, string key, AdvancedDropdownItem parentGroup)
        {
            if (tree.Value != null)
            {
                _lookup.Add(tree.Value);
                parentGroup.AddChild(new AdvancedDropdownItem(key) { id = _lookup.Count - 1 });
            }
            else
            {
                var self = new AdvancedDropdownItem(key);
                foreach (var subtree in tree.SubTrees)
                {
                    Render(subtree.Value, subtree.Key, self);
                }

                parentGroup.AddChild(self);
            }
        }

        protected override AdvancedDropdownItem BuildRoot()
        {
            var root = new AdvancedDropdownItem(_title);

            if (_list == null)
            {
                EditorUtility.DisplayDialogComplex
                (
                    "No Matching Items Found", 
                    $"Unable to find any assets marked with the [{typeof(TAttribute).Name}] Attribute.", 
                    "Shit", "Fuck", "My Dick Shot Off!");

                return null;
            }
            else
            {
                foreach (var subtree in _list.SubTrees)
                {
                    Render(subtree.Value, subtree.Key, root);
                }
            }

            return root;
        }
    }
}
#endif