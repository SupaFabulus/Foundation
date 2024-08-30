using System;
using SupaFabulus.Dev.Foundation.Core.Interfaces;
using UnityEngine;

namespace SupaFabulus.Dev.Foundation.Components.AutoReset
{
    [Serializable]
    public abstract class AbstractResettableState<TStateValue, TInitSource> : IInitWith<TInitSource>
    {
        [SerializeField]
        protected TStateValue _initialState;
        [SerializeField]
        protected TStateValue _resetState;
        [SerializeField]
        protected TStateValue _mostRecentState;
        [SerializeField]
        protected TStateValue _currentState;
        
        
        public abstract void InitWith(TInitSource initSource, bool local = true);

        public abstract void ApplyState(TInitSource target);
        
        
        public void SetAll(TStateValue state)
        {
            _initialState = state;
            _resetState = state;
            _mostRecentState = state;
            _currentState = state;
        }

        public void RestoreInitialState()
        {
            _currentState = _initialState;
        }
        public void RestoreResetState()
        {
            _currentState = _resetState;
        }
        public void RestoreMostRecentState()
        {
            _currentState = _mostRecentState;
        }
        public void CacheMostRecentState()
        {
            _mostRecentState = _currentState;
        }
        public void SaveResetState()
        {
            _resetState = _currentState;
        }


        public TStateValue InitialState
        {
            get => _initialState;
            set => _initialState = value;
        }
        public TStateValue ResetState
        {
            get => _resetState;
            set => _resetState = value;
        }
        public TStateValue MostRecentState
        {
            get => _mostRecentState;
            set => _mostRecentState = value;
        }
        public TStateValue CurrentState
        {
            get => _currentState;
            set => _currentState = value;
        }
    }
}