using System;
using Object = System.Object;

namespace SupaFabulus.Dev.Foundation.Core.Extensions
{
    public static class ObjectExtensions
    {
        public static bool IsAssignableByInterface(this Object thisObject, Type interfaceType)
        {
            return thisObject.GetType().IsAssignableFrom(interfaceType.GetType());
        }
        
        public static bool IsInstanceOf(this Object thisObject, Type interfaceType)
        {
            return interfaceType is Object;
        }
        
        public static bool ImplementsInterface(this Object thisObject, Type interfaceType)
        {
            return thisObject.GetType().GetInterface(interfaceType.GetType().FullName) != null;
        }
        
        public static bool IsA<TInterface>(this Object thisObject, TInterface interfaceType)
        where TInterface : Type
        {
            return (thisObject as TInterface) != null;
        }
    }
}