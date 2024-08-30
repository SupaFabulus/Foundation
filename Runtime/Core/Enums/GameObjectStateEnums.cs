using System;

namespace SupaFabulus.Dev.Foundation.Core.Enums
{
    [Serializable]
    [Flags]
    public enum GameObjectStateEventInterest : ushort
    {
        FixedUpdate		 = (1 << 0),
        Update			 = (1 << 1),
        LateUpdate		 = (1 << 2),
        OnCollisionEnter = (1 << 3),
        OnCollisionStay	 = (1 << 4),
        OnCollisionExit	 = (1 << 5),
        OnTriggerEnter	 = (1 << 6),
        OnTriggerStay	 = (1 << 7),
        OnTriggerExit	 = (1 << 8)
    }
}