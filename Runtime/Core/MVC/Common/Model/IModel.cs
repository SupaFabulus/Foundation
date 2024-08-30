namespace SupaFabulus.Dev.Foundation.Core.MVC.Common.Model
{
    public interface IModel : IMainComponent
    {
        void InitModel();
        void DeInitModel();

        void ResetModel();

        bool IsInitialized { get; }
        bool ResetOnInit { get; set; }
    }
}