using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace SupaFabulus.Dev.Foundation.EditorTools.ProjectSearch
{
    public static class NamespaceFinder
    {
        
        private static bool _namespacesInitialized = false;
        private static Assembly[] _assemblies;
        
        private static IEnumerable<string> _namespaces = new List<string>();
        private static IEnumerable<string> _names;

        private static Tuple<bool, string> predictedNamespace = default;
        private static string _lastPrediction = null;
        private static int _lastCount = 0;
        private static int _matchCount = 0;
        
        private static List<string> _matches = new List<string>();
        
        private static string[] _ns;
        private static int _matchID = -1;


        public static int LastNamespaceCount => _lastCount;
        public static string[] AllNamespaces => _namespaces.ToArray();


        static NamespaceFinder()
        {
            Refresh();
        }


        public static void Refresh()
        {
            InitNamespaces();
        }

        private static bool _debug = true;
        private static StringBuilder _sb = new ();
        private static void InitNamespaces()
        {
            bool debug = true;
            _assemblies = AppDomain.CurrentDomain.GetAssemblies();

            foreach (Assembly assembly in _assemblies)
            {
                _names = assembly.GetTypes()
                    .Select(t => t.Namespace)
                    .Distinct();

                _namespaces = _namespaces.Concat(_names).Distinct();
            }

            _ns = _namespaces.ToArray();
            _lastCount = _ns.Length;

            if (debug)
            {
                _sb.Clear();
                _sb.Append("All Namespaces:\n\n");
                foreach (string ns in _ns)
                {
                    _sb.Append(ns).Append('\n');
                }
            
                Debug.Log($"<color=#{Color.green}>NamespaceFinder Initialized with {_namespaces.Count()} namespaces</color>");
                Debug.Log(_sb.ToString());
            }
        }

        private static bool IsValidString(string str)
        {
            return (str != null && !string.IsNullOrEmpty(str) && !string.IsNullOrWhiteSpace(str));
        }

        public static List<string> GetPossibleMatches(string src)
        {
            _matches.Clear();
            Debug.Log($"Getting matches from list of {_namespaces.Count()}");
            foreach (string ns in _ns)
            {
                //Debug.Log($"Namespace: {ns}");
                if (IsValidString(ns) && ns.Contains(src))
                {
                    _matches.Add(ns);
                }
            }
            _matchCount = _matches.Count;
            Debug.Log($"Found {_matchCount} match(es)");
            return _matchCount > 0 ? _matches : null;
        }

        private static string[] _list;
        public static Tuple<int, string> FindClosestNamespaceMatch(string src)
        {
            //Debug.Log("Finding matching namespaces");
            int i;
            int c = _matches.Count;
            string ns;

            for (i = 0; i < c; i++)
            {
                ns = _matches[i];
                if (IsValidString(ns) && ns.Contains(src))
                {
                    return new Tuple<int, string>(i, ns);
                }
            }
            
            return new Tuple<int, string>(-1, src);
        }
    }
}