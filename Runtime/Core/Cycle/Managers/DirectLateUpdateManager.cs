/*
 * DirectLateUpdateManager.cs - Dispatches an event on LateUpdate
 *
 */

namespace SupaFabulus.Dev.Foundation.Core.Cycle.Managers
{
    public class DirectLateUpdateManager : DirectUpdateManagerBase
    {
        protected void LateUpdate()
        {
            Execute();
        }
    }
}