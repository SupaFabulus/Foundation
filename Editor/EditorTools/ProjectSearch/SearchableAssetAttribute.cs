
using System;

namespace SupaFabulus.Dev.Foundation.EditorTools.ProjectSearch
{
    
    public abstract class AbstractSearchableAttribute : Attribute, ISearchableAttribute
    {
        
    }
    
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public class SearchableAssetAttribute : AbstractSearchableAttribute
    {
        
    }
    
    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class SearchableMethodAttribute : AbstractSearchableAttribute
    {
        
    }
}