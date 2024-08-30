namespace SupaFabulus.Dev.Foundation.Core.MVC.Common.View
{
    public interface IView : IMainComponent
    {
        void InitView();
        void DeInitView();

        void Activate();
        void Deactivate();
        void ResetView();

        void Show();
        void Hide();

        bool IsInitialized { get; }
        bool IsActive { get; }
        bool IsVisible { get; }
    }
}