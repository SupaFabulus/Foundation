/*
 * DirectUpdateManagerBase.cs - Base class for DirectUpdate Phase Managers
 *
 * TODO: Add prioritization and ordering
 */

using SupaFabulus.Dev.Foundation.Core.Signals;
using UnityEngine;

namespace SupaFabulus.Dev.Foundation.Core.Cycle.Managers
{
    public abstract class DirectUpdateManagerBase : MonoBehaviour
    {
        protected Signal _onExecute = new();
        public Signal OnExecute => _onExecute;

        /*
         * Called by subclasses in associated Update phases (Fixed, Late...)
         */
        public virtual void Execute()
        {
            _onExecute.Broadcast();
        }

    }
}