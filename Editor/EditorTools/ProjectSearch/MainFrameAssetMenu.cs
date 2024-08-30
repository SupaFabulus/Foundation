#if UNITY_EDITOR
using UnityEditor;

namespace SupaFabulus.Dev.Foundation.EditorTools.ProjectSearch
{
    public static class MainFrameAssetMenuAction
    {
        private const string __MENU_TITLE = "Assets/Create/MainFrame Asset Search &`";
        private const bool __USE_BASE_NAME_SPACE = true;
        private const string __BASE_NAMESPACE = "Buganamo";
        private const string __EDITOR_TITLE = "MainFrame Asset Search";

        private class MainFrameAssetMenu : AbstractSearchableMenu<SearchableAssetAttribute>
        {
            public MainFrameAssetMenu(string menuTitle = __EDITOR_TITLE, bool useBaseNameSpace = false, string baseNamespace = null)
                : base(menuTitle, useBaseNameSpace, baseNamespace) {}
        }


        [MenuItem(
            itemName: __MENU_TITLE,
            isValidateFunction: false,
            priority: -1
        )]
        public static void DoMainFrameAssetSearch()
        {
            MainFrameAssetMenu m = new MainFrameAssetMenu
            (
                __EDITOR_TITLE,
                __USE_BASE_NAME_SPACE,
                __BASE_NAMESPACE
            );
            m.ExecuteMenuItem();
        }
        
        

    }
}
#endif