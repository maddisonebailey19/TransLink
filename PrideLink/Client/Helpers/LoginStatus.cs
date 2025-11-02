namespace PrideLink.Client.Helpers
{
    public class LoginStatus
    {
        public event Action? OnChange;

        public void NotifyStateChanged() => OnChange?.Invoke();
    }
}
