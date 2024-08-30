using System;
using SupaFabulus.Dev.Foundation.Core.Cycle.Enums;
using SupaFabulus.Dev.Foundation.Core.Cycle.Managers;
using UnityEngine;

namespace SupaFabulus.Dev.Foundation.Core.Cycle
{

    [Serializable]

    public class DirectUpdate : MonoBehaviour
    {
        private static DirectUpdate __instance;
        private static bool __isInitialized = false;

        public static DirectUpdate Instance
        {
            get => __instance;
        }

        static DirectUpdate()
        {
            Debug.Log($"DirectUpdate is ready? [{__isInitialized}]");
        }

        internal static bool InitializeInstance(DirectUpdate instance)
        {
            if (!__isInitialized && __instance == null && instance != null)
            {
                __instance = instance;
                __instance.InitializeDirectUpdateSystem();

                return true;
            }

            return false;
        }



        [SerializeField]
        protected bool _autoInitialize = false;
        [SerializeField]
        protected bool _autoStart = false;

        [SerializeField]
        protected AutoStartBehavior _autoStartFixedUpdate = AutoStartBehavior.Start;
        [SerializeField]
        protected DirectFixedUpdateManager _fixedUpdate;

        [SerializeField]
        protected AutoStartBehavior _autoStartUpdate = AutoStartBehavior.Start;
        [SerializeField]
        protected DirectUpdateManager _update;
        
        [SerializeField]
        protected AutoStartBehavior _autoStartLateUpdate = AutoStartBehavior.None;
        [SerializeField]
        protected DirectLateUpdateManager _lateUpdate;

        public DirectFixedUpdateManager fixedUpdate => _fixedUpdate;
        public DirectUpdateManager update => _update;
        public DirectLateUpdateManager lateUpdate => _lateUpdate;

        protected bool _isInitialized = false;
        protected bool _isActive = false;
        
        public bool IsInitialized => _isInitialized;
        public bool IsActive => _isActive;


        private void Awake()
        {
            if (__instance != null)
            {
                //Destroy(gameObject);
                Debug.LogError($"Cannot init two instances of DirectUpdate!");
                SetAllEnabled(false);
                enabled = false;
                gameObject.SetActive(false);
                return;
            }

            if (_autoInitialize && InitializeInstance(this) && _isInitialized && _autoStart)
            {
                AutoStartManagersForBehavior(AutoStartBehavior.Awake);
            }
        }

        private void Start()
        {
            if (_isInitialized && _autoStart)
            {
                AutoStartManagersForBehavior(AutoStartBehavior.Start);
            }
        }

        public void Init(bool initiallyEnabled = false)
        {
            // Strange, but true
            if (!_isInitialized && InitializeInstance(this) && _isInitialized)
            {
                SetAllEnabled(initiallyEnabled);
            }
        }


        internal void InitializeDirectUpdateSystem()
        {
            if (!_isInitialized)
            {
                _isInitialized = true;
            }
        }


        public void SetAllEnabled(bool allEnabled)
        {
            Debug.Log($"Setting All Update Systems Enabled: [{allEnabled}]");
            
            if (_fixedUpdate != null)
                _fixedUpdate.enabled = allEnabled;

            if (_update != null)
                _update.enabled = allEnabled;
            
            if (_lateUpdate != null)
                _lateUpdate.enabled = allEnabled;
            
            ShowSystemState();
            
        }

        protected void ShowSystemState()
        {
            Debug.Log($"System States:" +
                      $"\n - Fixed  >>  [{_fixedUpdate}]: [{_fixedUpdate != null && _fixedUpdate.enabled}]" +
                      $"\n - Update >>  [{_update}]: [{_update != null && _update.enabled}]" +
                      $"\n - Late   >>  [{_lateUpdate}]: [{_lateUpdate != null && _lateUpdate.enabled}]");
        }

        protected void AutoStartManagersForBehavior(AutoStartBehavior behavior)
        {
            Debug.Log($"AutoStarting for behavior: [{behavior}]");
            
            if (behavior == AutoStartBehavior.None) return;
            
            if (_fixedUpdate != null)
                _fixedUpdate.enabled = _autoStartFixedUpdate == behavior;
            
            if (_update != null)
                _update.enabled = _autoStartUpdate == behavior;
            
            if (_lateUpdate != null)
                _lateUpdate.enabled = _autoStartLateUpdate == behavior;
            
            ShowSystemState();
        }

    }
}