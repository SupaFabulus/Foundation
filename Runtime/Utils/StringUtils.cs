namespace SupaFabulus.Dev.Foundation.Utils
{
    public static class StringUtils
    {
        public static string DeClone(this string str)
        {
            const string c = "(Clone)";
            return str.Substring(0, str.LastIndexOf(c));
        }
    }
}