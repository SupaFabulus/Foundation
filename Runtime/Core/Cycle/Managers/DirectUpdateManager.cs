/*
 * DirectUpdateManager.cs - Dispatches an event on Update
 *
 */

namespace SupaFabulus.Dev.Foundation.Core.Cycle.Managers
{
    public class DirectUpdateManager : DirectUpdateManagerBase
    {
        protected void Update()
        {
            Execute();
        }
    }
}