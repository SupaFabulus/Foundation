using System;
using System.Collections.Generic;
using System.Text;
using SupaFabulus.Dev.Foundation.Core.Singletons;
using UnityEngine;

namespace SupaFabulus.Dev.Foundation.EditorTools.ProjectSearch
{
    [Serializable]
    [SearchableAsset]
    [CreateAssetMenu(
        fileName = "NamespaceProvider", 
        menuName = "Buganamo/MainFrame/Namespace Provider"
    )]
    public class NamespaceProvider : AbstractSingletonAsset<NamespaceProvider>,
        ISingleton<NamespaceProvider>
    {
        [SerializeField][HideInInspector]
        private bool _autoRefresh = true;
        [SerializeField]
        private string _targetNamespace;
        [SerializeField][HideInInspector]
        private string _newNamespace;
        [SerializeField][HideInInspector]
        private int _lastTargetNamespaceLength;
        [SerializeField][HideInInspector]
        private bool _targetNamespaceDidChange = false;

        [SerializeField][HideInInspector]
        private Tuple<int, string> _closestMatch = null;
        [SerializeField][HideInInspector]
        private List<string> _possibleMatches = new List<string>();
        [SerializeField][HideInInspector]
        private string _exactMatch = null;
        [SerializeField][HideInInspector]
        private int _matchID;
        [SerializeField][HideInInspector]
        private int _selectionID;
        
        [SerializeField][HideInInspector]
        private bool _namespacesInitialized = false;
        
        
        public bool NamespacesInitialized => _namespacesInitialized;
        public bool TargetNamespaceDidChange => _targetNamespaceDidChange;


        public List<string> PossibleMatches
        {
            get => _possibleMatches;
            set => _possibleMatches = value;
        }
        public Tuple<int, string> ClosestMatch => _closestMatch;
        public string ExactMatch => _exactMatch;
        public int MatchID => _matchID;

        public int SelectionID
        {
            get => _selectionID;
            set => _selectionID = value;
        }


        public bool AutoRefresh
        {
            get => _autoRefresh;
            set => _autoRefresh = value;
        }
        
        public string TargetNamespace
        {
            get => _targetNamespace;
            set
            {
                _targetNamespaceDidChange = _targetNamespace != value;
                _targetNamespace = value;

                if (_targetNamespaceDidChange && _autoRefresh)
                {
                    Refresh();
                }
            }
        }
        public string NewNamespace
        {
            get => _newNamespace;
            set
            {
                _newNamespace = value;
                TargetNamespace = _newNamespace;
            }
        }
        
        private string[] _all;
        public void Init()
        {
            
            NamespaceFinder.Refresh();
            _all = NamespaceFinder.AllNamespaces;
            
            //Debug.Log($"Namespaces: {_lastCount}");

            _namespacesInitialized = _all != null && _all.Length > 0;
        }

        private StringBuilder _sb = new StringBuilder();
        public void Refresh()
        {
            _possibleMatches = NamespaceFinder.GetPossibleMatches(_targetNamespace);
            _closestMatch = NamespaceFinder.FindClosestNamespaceMatch(_targetNamespace);
            _matchID = _closestMatch.Item1;
            _exactMatch = _matchID >= 0 ? _closestMatch.Item2 : null;
               
            _targetNamespaceDidChange = false;

            if (_possibleMatches == null)
            {
                Debug.LogError($"WHY THE FUCK ARE THERE NO POSSIBLE MATCHES FOR: {_targetNamespace}");
                return;
            }

            _sb.Clear();
            int i;
            int c = _possibleMatches.Count;

            for (i = 0; i < c; i++)
            {
                _sb.Append(_possibleMatches[i]).Append('\n');
            }
            Debug.Log($"Matches ({c}):\n{_sb.ToString()}");

            Debug.Log("Refresh");
        }

        protected override bool InitInstance()
        {
            Init();
            Validate();
            return _namespacesInitialized;
        }

        protected override void DeInitInstance()
        {
            
        }
    }
}