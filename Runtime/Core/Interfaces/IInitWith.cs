namespace SupaFabulus.Dev.Foundation.Core.Interfaces
{
    public interface IInitWith<in TInitSource>
    {
        void InitWith(TInitSource initSource, bool local = true);
    }
}