/*
 * DirectFixedUpdateManager.cs - Dispatches an event on FixedUpdate
 *
 */

namespace SupaFabulus.Dev.Foundation.Core.Cycle.Managers
{
    public class DirectFixedUpdateManager : DirectUpdateManagerBase
    {
        protected void FixedUpdate()
        {
            Execute();
        }
    }
}