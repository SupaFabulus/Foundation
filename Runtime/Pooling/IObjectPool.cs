namespace SupaFabulus.Dev.Foundation.Pooling
{
    public interface IObjectPool<T> where T : IPoolable
    {
        void InitializePool();
        T GetInstance();
        void ReturnInstance(IPoolable instance);
        void ClearPool();
    }

}