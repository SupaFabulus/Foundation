using System;
using System.Collections.Generic;
using UnityEngine;


namespace SupaFabulus.Dev.Foundation.Pooling
{

    public class ObjectPool<T> : System.Object, IDisposable, IObjectPool<T> where T : IPoolable
    {
        protected bool _disposed = false;

        public virtual void Dispose()
        {
            if (_disposed) return;

            ClearPool();
            _activeInstances = null;
            _inactiveInstances = null;

            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;
            if (!disposing) return;

            ClearPool();

            if(_activeInstances != null) _activeInstances.Clear();
            if(_inactiveInstances != null) _inactiveInstances.Clear();

            _activeInstances = null;
            _inactiveInstances = null;

            _inflatable = default(bool);
            _deflatable = default(bool);
            _deflationInterval = default(float);

            _size = default(int);

            _disposed = true;
        }

        /*--------------------------------------------------------------------*/


        public delegate T PoolObjectInstantiatorDelegate();

        public delegate void PoolObjectDestructorDelegate(T instanceToDestroy);

        public delegate T PoolObjectActivationDelegate(T instanceToActivate);

        public delegate void PoolObjectDeactivatorDelegate(T instanceToDeactivate);

        public PoolObjectInstantiatorDelegate InstantiationDelegate;
        public PoolObjectDestructorDelegate DestructionDelegate;
        public PoolObjectActivationDelegate ActivationDelegate;
        public PoolObjectDeactivatorDelegate DeactivationDelegate;


        protected LinkedList<T> _inactiveInstances;
        protected LinkedList<T> _activeInstances;

        protected bool _inflatable;
        protected bool _deflatable;
        protected float _deflationInterval;

        protected bool _initialized = false;

        protected ObjectPool<IPoolable> _poolablePool;

        protected int _size;

        public bool IsInitialized
        {
            get => _initialized;
        }

        public int size
        {
            get { return (_disposed) ? default(int) : _size; }
        }

        public int InactiveCount
        {
            get { return _inactiveInstances.Count; }
        }

        public int ActiveCount
        {
            get { return _activeInstances.Count; }
        }

        public LinkedList<T> ActiveInstances
        {
            get => _activeInstances;
        }

        public LinkedList<T> InactiveInstances
        {
            get => _activeInstances;
        }



        public ObjectPool(
            int initialSize,
            PoolObjectInstantiatorDelegate instantiation,
            PoolObjectDestructorDelegate destruction,
            bool poolCanInflate = false,
            bool poolCanDeflate = false,
            float deflationInterval = 60f

        ) : base()
        {
            _size = initialSize;

            InstantiationDelegate = instantiation;
            DestructionDelegate = destruction;

            _deflationInterval = deflationInterval;
            _deflatable = poolCanDeflate;
            _inflatable = poolCanInflate;

            _poolablePool = this as ObjectPool<IPoolable>;
        }

        public void InitializePool()
        {
            if (_initialized) return;
            //Debug.Log("Initializing Pool...");
            CreateReserves();
            _initialized = true;
        }

        protected void CreateReserves()
        {
            if (_disposed) return;

            _inactiveInstances = new LinkedList<T>();
            _activeInstances = new LinkedList<T>();

            CreateInstances(_size);
        }

        protected virtual T CreateInstance()
        {
            return InstantiationDelegate();
        }

        protected void CreateInstances(int count)
        {
            if (_disposed) return;

            T instance;


            int remaining = count;

            while (remaining > 0)
            {
                instance = CreateInstance();
                instance.ReturnInstanceToPoolDelegate = ReturnInstance;
                //instance.RegisterWithPool(_poolablePool);

                _inactiveInstances.AddLast(instance);
                //Debug.Log((instance as MonoBehaviour).gameObject.name);
                remaining--;

            }
        }

        public T GetInstance()
        {
            if (_disposed) return default(T);

            T nil = default(T);
            if (_inactiveInstances == null) return default(T);
            if (_activeInstances == null) return default(T);

            T instance;
            LinkedListNode<T> node;
            int count = _inactiveInstances.Count;
            int last = count - 1;

            if (count > 0)
            {
                node = _inactiveInstances.Last;
                instance = node.Value;
                _inactiveInstances.RemoveLast();
                _activeInstances.AddLast(instance);

                if (ActivationDelegate != null)
                {
                    //Debug.Log("ObjectPool calling activation delegate");
                    (instance as IPoolable).ActivateFromPool();
                    instance = ActivationDelegate(instance);
                }
                else
                {
                    Debug.Log("ObjectPool has no activation delegate");
                }

                return instance;
            }

            return nil;
        }

        public void ReturnInstance(IPoolable poolable)
        {
            if (_disposed) return;
            T instance = (T) poolable;
            LinkedListNode<T> node;

            if (poolable != null)
            {
                try
                {
                    node = _activeInstances.Find(instance);
                    if (node != null && node != default(LinkedListNode<T>))
                    {
                        instance = node.Value;
                        if (instance != null)
                        {
                            _activeInstances.Remove(instance);
                            _inactiveInstances.AddLast(instance);
                        }
                    }

                    if (DeactivationDelegate != null)
                    {
                        //Debug.Log("ObjectPool calling deactivation delegate");
                        DeactivationDelegate(instance);
                    }
                    else
                    {
                        Debug.Log("ObjectPool has no activation delegate");
                    }
                }
                catch (InvalidCastException e)
                {
                    System.Console.WriteLine(e);
                    throw;
                }
            }
            else
            {
                Debug.LogError("Unable to return object to pool because of a failed type cast.");
            }
        }

        public void ClearPool()
        {
            if (_disposed) return;
            if (_inactiveInstances == null || _activeInstances == null) return;

            T instance;
            LinkedListNode<T> node;
            
            node = _inactiveInstances.Last;

            while (node != null)
            {
                instance = node.Value;
                _inactiveInstances.RemoveLast();
                DestructionDelegate(instance);
                instance = default(T);
                node = _inactiveInstances.Last;
            }
            
            node = _activeInstances.Last;

            while (node != null)
            {
                instance = node.Value;
                _activeInstances.RemoveLast();
                DestructionDelegate(instance);
                instance = default(T);
                node = _activeInstances.Last;
            }

            _inactiveInstances.Clear();
            _activeInstances.Clear();
        }
    }

}