using System;
using UnityEngine;

namespace SupaFabulus.Dev.Foundation.EditorTools.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
    public class ObjectByInterfaceAttribute : PropertyAttribute
    {
        protected Type _interfaceType;
        protected Type _objectType;

        public Type InterfaceType => _interfaceType;
        public Type ObjectType => _objectType;

        public ObjectByInterfaceAttribute() { }

        public ObjectByInterfaceAttribute
        (
            Type interfaceType,
            Type objectType = null
        )
        {
            _interfaceType = interfaceType;
            _objectType = objectType;
        }
    }
}