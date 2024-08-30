using System;
using UnityEngine;

namespace SupaFabulus.Dev.Foundation.EditorTools.Attributes
{
    [AttributeUsage(
        AttributeTargets.Property | AttributeTargets.Field, 
        Inherited = true, AllowMultiple = true
    )]
    public abstract class PrefabReferenceAttribute : PropertyAttribute
    {
        protected Type[] _componentTypes;
        public Type[] ComponentTypes { get; }

        public PrefabReferenceAttribute
        (
            params Type[] componentTypes
        )
        {
            _componentTypes = componentTypes;
        }
    }
}