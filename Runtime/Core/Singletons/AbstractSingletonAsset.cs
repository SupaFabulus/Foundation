using System;
using SupaFabulus.Dev.Foundation.Core.Signals;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace SupaFabulus.Dev.Foundation.Core.Singletons
{
    [Serializable]
    public abstract class AbstractSingletonAsset<TSingleton> : ScriptableObject
    
        where TSingleton : 
            ScriptableObject, ISingleton<TSingleton>

    {
        protected static object __instantiationLock = new object();
        protected static bool __initialized = false;
        protected static TSingleton __instance;

        public static bool IsInitialized => __initialized;

        protected static Signal<TSingleton> _onChange = new();
        public static Signal<TSingleton> OnChange => _onChange;

        public virtual void NotifyChange()
        {
            _onChange.Broadcast(__instance);
        }

        public static TSingleton ß
        {
            get
            {
                if (__instance == null)
                {
#if UNITY_EDITOR

                    TSingleton inst = null; //AssetUtils.GetSingletonAsset<TSingleton>();
                    if (inst != null)
                    {
                        __instance = inst;
                    }
#endif
                }

                return __instance;
            }
        }

        protected static bool InitSingletonInstance(TSingleton instance)
        {
            if(__instance == null)
            {
                if (instance != null)
                {
                    __instance = instance;
                    __initialized = true;
                }
                else
                {
#if UNITY_EDITOR
                    string n = typeof(TSingleton).Name;
                    Debug.Log($"Searching for types of: {n}");
                    string[] guids = AssetDatabase.FindAssets(
                        $"t:{n}",
                        new []
                        {
                            "Assets/Settings",
                            "Assets/_MAIN/Settings",
                            "Assets/_INTERNAL"
                        });

                    int i;
                    int c = guids.Length;
                    string guid;
                    string path;
                    TSingleton asset = null;
                    
                    for (i = 0; i < c; i++)
                    {
                        guid = guids[i];
                        path = AssetDatabase.GUIDToAssetPath(guid);
                        if (path != null)
                        {
                            asset = AssetDatabase.LoadAssetAtPath<TSingleton>(path);
                        }

                        if (asset != null)
                        {
                            Debug.Log($"Found asset '{asset.name}'[{asset.GetType().Name}], at {path}");
                            InitSingletonInstance(asset);
                            
                        }
                        else
                        {
                            Debug.Log($"Unable to find asset at path: [{path}]");
                        }
                    }
#endif
                }
            }
            else
            {
                __initialized = true;
            }

            return __initialized;
        }
        
        /*
        protected static TSingleton GetInstance()
        {
            if (__instance == null)
            {
                lock (__instantiationLock)
                {
                    if (__instantiationLock == null)
                    {
                        __initialized = true;
                    }
                }
            }

            return __instance;
        }
        */

        public static void DeInitializeSingleton()
        {
            if (__instance != null)
            {
                __instance = null;
                __initialized = false;
            }
        }
        
        
        [SerializeField]
        protected bool _notifyUpdateOnValidate = false;

        public bool NotifyUpdateOnValidate
        {
            get => _notifyUpdateOnValidate;
            set => _notifyUpdateOnValidate = value;
        }

        public void Awake()
        {
            Validate();
        }
        
        public void OnEnable()
        {
            Validate();
        }

        public virtual bool Validate()
        {
#if UNITY_EDITOR
            Debug.Log($"Validating Singleton: {this.name}\nAt: [{AssetDatabase.GetAssetPath(this)}]");
#endif
            return ValidateInstance(this as TSingleton, true);
        }

        public static bool FindInstance(bool destroyViolators = false)
        {
            return ValidateInstance(null, destroyViolators);
        }

        public static bool ValidateInstance(TSingleton instance, bool destroyViolators = false)
        {
            string path = "[OOPS, NULL FILE PATH, IDIOT (Or maybe at runtime)]";


            
            
            
            if (__instance != null)
            {
                if (__instance != instance && instance != null)
                {
#if UNITY_EDITOR
                    path = AssetDatabase.GetAssetPath(instance);
                    string typeName = instance.GetType().FullName;
                    string errMsg = $"Cannot have more than one instance of Singleton [{typeName}]";
                    Debug.LogError(errMsg);
                    
                    if (destroyViolators)
                    {
                        errMsg = $"Destroying Asset: [{instance.name}]...";
                        Debug.LogWarning(errMsg);
                        
                        #if UNITY_EDITOR
                        if (AssetDatabase.DeleteAsset(path))
                        {
                            errMsg = $"Deleted Asset File at: [{path}]";
                            Debug.LogWarning(errMsg);
                        }
                        else
                        {
                            errMsg = $"Unable to delete Asset File at: [{path}]";
                            Debug.LogError(errMsg);
                        }
                        DestroyImmediate(instance);
                        #else
                        Destroy(this);
                        #endif
                    }
                    else
                    {
                        errMsg = $"WARNING: Duplicate Singleton [{instance.name}] was NOT deleted, this could lead to unexpected behavior!";
                        Debug.LogWarning(errMsg);
                        return false;
                    }
#endif

                    return false;
                }
            }
            else
            {
                return InitSingletonInstance(instance);
            }

            return false;
        }

        protected abstract bool InitInstance();
        protected abstract void DeInitInstance();
        
    }
}