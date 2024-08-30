using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;

namespace SupaFabulus.Dev.Foundation.Core.Signals
{
    public interface ISignal
    {
        bool IsInitialized { get; }
        bool HasReceivers { get; }
    }

    public interface ISignal<in TSignalDelegate>
        where TSignalDelegate : Delegate
    {
        //bool IsInitialized { get; }
        bool HasReceivers { get; }

        //void Broadcast       (TPayload payload = default);
        bool DoesBroadcastTo(TSignalDelegate handle);
        bool AddReceiver(TSignalDelegate handle);
        bool RemoveReceiver(TSignalDelegate handle);
    }

    [Serializable]
    public struct SignalReceiverEntry
    {
        [SerializeField] 
        public UnityEngine.Object target;
        [SerializeField]
        public string methodName;
    }


    public delegate void SignalReceiverDelegate0();
    public delegate void SignalReceiverDelegate1<in T0>(T0 p0 = default);
    public delegate void SignalReceiverDelegate2<in T0, in T1>(T0 p0 = default, T1 p1 = default);
    public delegate void SignalReceiverDelegate3<in T0, in T1, in T2>(T0 p0 = default, T1 p1 = default, T2 p2 = default);
    public delegate void SignalReceiverDelegate4<in T0, in T1, in T2, in T3>(T0 p0 = default, T1 p1 = default, T2 p2 = default, T3 p3 = default);

    [Serializable]
    public abstract class AbstractSignal<TSignalDelegate> : ISignal<TSignalDelegate>
    where TSignalDelegate : Delegate
    {
        protected HashSet<TSignalDelegate> _receivers;
        protected TSignalDelegate[] _receiverBuffer;
        protected int _receiverCount;
        protected bool _closed = false;
        protected bool _bindingsProcessed = false;

        public bool IsClosed => _closed;

        

        protected void ProcessBindings<TUnityEvent>(TUnityEvent bindings)
        where TUnityEvent : UnityEventBase
        {
            if (bindings == null) return;
            if (_receivers == null) _receivers = new();

            int i;
            int c = bindings.GetPersistentEventCount();
            UnityEngine.Object tgt;
            string n;
            MethodInfo info;

            for (i = 0; i < c; i++)
            {
                tgt = bindings.GetPersistentTarget(i);
                n = bindings.GetPersistentMethodName(i);
                if (tgt == null)
                {
                    Debug.LogWarning($"No target found at index {i}");
                    continue;
                }

                if (string.IsNullOrEmpty(n) || string.IsNullOrWhiteSpace(n))
                {
                    Debug.LogWarning($"No method name found at index {i}");
                    continue;
                }
                info = tgt.GetType().GetMethod(n);
                if (info == null)
                {
                    Debug.LogWarning($"No method info found for name [{n}] on target [{tgt}: {tgt.name}] at index {i}");
                    continue;
                }
                
                TSignalDelegate method = (TSignalDelegate)System.Delegate.CreateDelegate(typeof(TSignalDelegate), tgt, info);
                Debug.Log($"Found method with signature: [{method}]");

                if(!_receivers.Contains(method)) _receivers.Add(method);
            }

            _bindingsProcessed = true;
        }

        public void Open()
        {
            _receiverBuffer = null;
            _receiverCount = 0;
            _closed = false;
        }

        public void Close()
        {
            int i = 0;
            _receiverCount = _receivers.Count;
            _receiverBuffer = new TSignalDelegate[_receiverCount];

            foreach (TSignalDelegate r in _receivers)
            {
                _receiverBuffer[i] = r;
            }

            _closed = true;
        }
        
        

        public bool HasReceivers => _receivers.Count > 0;
        
        public bool DoesBroadcastTo(TSignalDelegate handle)
        {
            if (_receivers == null) return false;
            return _receivers.Contains(handle);
        }

        public bool AddReceiver(TSignalDelegate handle)
        {
            if (_receivers == null) _receivers = new();
            if (CheckClosed()) return false;
            if (!_receivers.Contains(handle))
            {
                _receivers.Add(handle);
                return true;
            }

            return false;
        }

        public bool RemoveReceiver(TSignalDelegate handle)
        {
            if (_receivers == null) return false;
            if (CheckClosed()) return false;
            if (_receivers.Contains(handle))
            {
                _receivers.Remove(handle);
                return true;
            }

            return false;
        }

        public void Clear()
        {
            if (CheckClosed()) return;
            if(_receivers != null) _receivers.Clear();
        }

        protected bool CheckClosed()
        {
            if (_closed)
            {
                Debug.LogError($"Signal is Closed!");
            }

            return _closed;
        }
    }

    [Serializable]
    public class Signal : AbstractSignal<SignalReceiverDelegate0>
    {
        [SerializeField]
        private UnityEvent _bindings;

        public Signal()
        {
            if (_receivers == null) _receivers = new();
        }

        public virtual void Broadcast()
        {
            if (_closed)
            {
                int i;
                SignalReceiverDelegate0 r;

                for (i = 0; i < _receiverCount; i++)
                {
                    r = _receiverBuffer[i];
                    if (r != null) r();
                }
            }
            else
            {
                if(!_bindingsProcessed) ProcessBindings(_bindings);
                foreach (SignalReceiverDelegate0 r in _receivers) r();
            }
        }
    }
    
    [Serializable]
    public class Signal<T0> : AbstractSignal<SignalReceiverDelegate1<T0>>
    {
        [SerializeField]
        private UnityEvent<T0> _bindings;
        
        public virtual void Broadcast(T0 p0)
        {
            if (_closed)
            {
                int i;
                SignalReceiverDelegate1<T0> r;

                for (i = 0; i < _receiverCount; i++)
                {
                    r = _receiverBuffer[i];
                    if (r != null) r(p0);
                }
            }
            else
            {
                if(!_bindingsProcessed) ProcessBindings(_bindings);
                foreach (SignalReceiverDelegate1<T0> r in _receivers) r(p0);
            }
        }
    }
    
    [Serializable]
    public class Signal<T0, T1> : AbstractSignal<SignalReceiverDelegate2<T0, T1>>
    {
        [SerializeField]
        private UnityEvent<T0, T1> _bindings;

        public virtual void Broadcast(T0 p0, T1 p1)
        {
            if (_closed)
            {
                int i;
                SignalReceiverDelegate2<T0, T1> r;

                for (i = 0; i < _receiverCount; i++)
                {
                    r = _receiverBuffer[i];
                    if (r != null) r(p0, p1);
                }
            }
            else
            {
                if(!_bindingsProcessed) ProcessBindings(_bindings);
                foreach (SignalReceiverDelegate2<T0, T1> r in _receivers) r(p0, p1);
            }
        }
    }

    [Serializable]
    public class Signal<T0, T1, T2> : AbstractSignal<SignalReceiverDelegate3<T0, T1, T2>>
    {
        [SerializeField]
        private UnityEvent<T0, T1, T2> _bindings;

        public virtual void Broadcast(T0 p0, T1 p1, T2 p2)
        {
            if (_closed)
            {
                int i;
                SignalReceiverDelegate3<T0, T1, T2> r;

                for (i = 0; i < _receiverCount; i++)
                {
                    r = _receiverBuffer[i];
                    if (r != null) r(p0, p1, p2);
                }
            }
            else
            {
                if(!_bindingsProcessed) ProcessBindings(_bindings);
                foreach (SignalReceiverDelegate3<T0, T1, T2> r in _receivers) r(p0, p1, p2);
            }
        }
    }

    [Serializable]
    public class Signal<T0, T1, T2, T3> : AbstractSignal<SignalReceiverDelegate4<T0, T1, T2, T3>>
    {
        [SerializeField]
        private UnityEvent<T0, T1, T2, T3> _bindings;

        public virtual void Broadcast(T0 p0, T1 p1, T2 p2, T3 p3)
        {
            if (_closed)
            {
                int i;
                SignalReceiverDelegate4<T0, T1, T2, T3> r;

                for (i = 0; i < _receiverCount; i++)
                {
                    r = _receiverBuffer[i];
                    if (r != null) r(p0, p1, p2, p3);
                }
            }
            else
            {
                if(!_bindingsProcessed) ProcessBindings(_bindings);
                foreach (SignalReceiverDelegate4<T0, T1, T2, T3> r in _receivers) r(p0, p1, p2, p3);
            }
        }
    }
}