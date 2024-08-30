using System;
using SupaFabulus.Dev.Foundation.Core.MVC.Common.Model;
using UnityEngine;

namespace SupaFabulus.Dev.Foundation.Core.MVC.Bases
{
    [Serializable]
    public abstract class ModelBase : AbstractAssetModel, IModel
    {
        public bool IsInitialized => _isInitialized;

        public bool ResetOnInit
        {
            get => _resetOnInit;
            set => _resetOnInit = value;
        }


        protected virtual void OnEnable()
        {
            Debug.Log($"AppModel::OnEnable");
        }

        protected virtual  void OnDisable()
        {
            Debug.Log($"AppModel::OnDisable");
        }

        protected virtual  void OnValidate()
        {
            Debug.Log($"AppModel::OnValidate");
        }

        protected virtual  void Awake()
        {
            Debug.Log($"AppModel::Awake");
        }

        protected virtual  void Reset()
        {
            Debug.Log($"AppModel::Reset");
        }


        public virtual void ResetModel()
        {
            Debug.Log($"AppModel.ResetModel");
        }

        public virtual void InitModel()
        {
            if (!_isInitialized)
            {
                if (_resetOnInit)
                {
                    ResetModel();
                }

                _isInitialized = true;
            }
        }

        public virtual void DeInitModel()
        {
            _isInitialized = false;
        }
    }
}