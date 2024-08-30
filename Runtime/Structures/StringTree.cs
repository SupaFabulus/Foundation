using System;
using System.Collections.Generic;

namespace SupaFabulus.Dev.Foundation.Structures
{
    [Serializable]
    public class StringTree<TItem>
    {
        public Dictionary<string, StringTree<TItem>> SubTrees 
        { get; } = new Dictionary<string, StringTree<TItem>>();

        public TItem Value { get; private set; }

        public void Insert(string path, TItem value, int index = 0)
        {
            Internal_Insert(path.Split('/'), index, value);
        }

        private void Internal_Insert(string[] path, int index, TItem value)
        {
            if (index >= path.Length)
            {
                Value = value;
                return;
            }

            if (!SubTrees.ContainsKey(path[index]))
            {
                SubTrees.Add(path[index], new StringTree<TItem>());
            }
                
            SubTrees[path[index]].Internal_Insert(path, index + 1, value);
        }
    }
}