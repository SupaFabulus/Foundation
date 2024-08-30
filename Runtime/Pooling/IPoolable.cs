namespace SupaFabulus.Dev.Foundation.Pooling
{
    public delegate void PoolReturnDelegate(IPoolable poolableObject);

    public interface IPoolable
    {
        
        void ActivateFromPool();
        void ReturnToPool();
        PoolReturnDelegate ReturnInstanceToPoolDelegate { get; set; }


        // DO NOT DELETE YET
        /*
        void RegisterWithPool(IObjectPool<IPoolable> pool);
        void UnRegisterFromPool(IObjectPool<IPoolable> pool);
        
        PoolRegistrationDelegate<IPoolable> RegisterWithPoolDelegate { get; set; }
        PoolRegistrationDelegate<IPoolable> UnRegisterWithPoolDelegate { get; set; }

        void NotifyActivatedFromPool();
        void NotifyReturnedToPool();
        */
    }

}